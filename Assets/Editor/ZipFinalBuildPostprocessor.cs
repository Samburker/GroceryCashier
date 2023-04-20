using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System.IO.Compression;

public class ZipFinalBuildPostprocessor
{
    [PostProcessBuild(1000)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
#if DEBUG
        return;
#endif
        // Processing only windows builds
        if (target != BuildTarget.StandaloneWindows && target != BuildTarget.StandaloneWindows64)
            return;

        string origTargetFolder = Path.GetFullPath(Path.Combine(pathToBuiltProject, ".."));
        string newTargetFolder = Path.GetFullPath(Path.Combine(pathToBuiltProject, "..", Application.productName + " " + Application.version));
        string zipPath = Path.Combine(origTargetFolder, Application.productName + " " + Application.version + ".zip");

        // If target directory exists, deleting and recreating it
        if (Directory.Exists(newTargetFolder))
            Directory.Delete(newTargetFolder);
        Directory.CreateDirectory(newTargetFolder);

        // If target zip exists, deleting it
        if (File.Exists(zipPath))
            File.Delete(zipPath);

        // Iterating throug all files
        foreach (var file in Directory.EnumerateFiles(origTargetFolder))
        {
            // Excluding zip files
            if (Path.GetExtension(file) == ".zip")
                continue;

            // Moving file
            File.Move(file, Path.Combine(newTargetFolder, Path.GetFileName(file)));
        }

        // Iterating throug all folders
        foreach (var dir in Directory.EnumerateDirectories(origTargetFolder))
        {
            // Excluding If this folder is current target or other exclude reasons
            if (Path.GetFullPath(dir).Equals(newTargetFolder))
                continue;
            if (dir.Contains("DontShipIt"))
                continue;
            if (dir.Contains("DoNotShip"))
                continue;

            // Moving folder
            string lastFolderName = Path.GetFileName(dir);
            Directory.Move(dir, Path.Combine(newTargetFolder, lastFolderName));
        }

        // Creating the final zip
        ZipFile.CreateFromDirectory(newTargetFolder, zipPath, System.IO.Compression.CompressionLevel.Optimal, false, null);
    }

}
