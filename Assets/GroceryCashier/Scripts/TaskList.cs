using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TaskList : MonoBehaviour
{

    public string[] tagsToCheck;  // list of tags to check
    public Text taskListText;    // reference to the Text object that displays the task list
    public float taskSpacing = 10f;  // spacing between tasks in pixels
    private List<string> taskList = new List<string>();  // list of tasks to display

    void Update()
    {
        // clear the task list
        taskList.Clear();

        // loop through the tags to check
        foreach (string tag in tagsToCheck)
        {
            // find all objects with the tag
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);

            // loop through the objects with the tag
            foreach (GameObject obj in objectsWithTag)
            {
                // check if the object is active
                if (obj.activeInHierarchy)
                {
                    // add the task text to the task list
                    taskList.Add(GetTaskText(tag));
                }
            }
        }

        // update the task list display
        UpdateTaskListText();
    }

    // returns the task text for a given tag
    private string GetTaskText(string tag)
    {
        // add additional cases here as needed
        switch (tag)
        {
            case "RingOfFire":
                return "Put out fire";
            case "Vomit":
                return "Clean up vomit";
            default:
                return "Unknown task";
        }
    }

    // updates the task list display
    private void UpdateTaskListText()
    {
        // combine the task list into a single string
        string taskListString = string.Join("\n", taskList);

        // split the task list into separate lines
        string[] taskLines = taskListString.Split('\n');

        // update the task list text with the separated lines
        taskListText.text = "";
        for (int i = 0; i < taskLines.Length; i++)
        {
            if (i > 0)
            {
                taskListText.text += "\n";
            }
            taskListText.text += taskLines[i];
            if (i < taskLines.Length - 1)
            {
                taskListText.text += "\n";
                taskListText.text += new string(' ', (int)taskSpacing);
            }
        }
    }
}
