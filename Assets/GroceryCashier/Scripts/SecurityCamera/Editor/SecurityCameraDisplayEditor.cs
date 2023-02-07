using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SecurityCameraDisplay))]
public class SecurityCameraDisplayEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Previous"))
        {
            SecurityCameraDisplay display = (SecurityCameraDisplay)target;
            display.PreviousCamera();
        }

        if(GUILayout.Button("Next"))
        {
            SecurityCameraDisplay display = (SecurityCameraDisplay)target;
            display.NextCamera();
        }
    }
}
