using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    private Task currentTask = null;
    private void Start()
    {
        StartTask(new Task(this));
    }
    public void StartTask(Task task)
    {
        if (task == null)
            return;
        if (currentTask != null && currentTask != task)
        {
            if (currentTask.End())
            {
                currentTask = task;
                currentTask.Start();
            }
        }
        else
        {
            currentTask = task;
            currentTask.Start();
        }
    }
    public void PriorityStartTask(Task task)
    {
        if (task == null)
            return;
        if (currentTask != null && currentTask != task)
        {
            currentTask = task;
            currentTask.Start();
        }
        else
        {
            currentTask = task;
            currentTask.Start();
        }
    }
    public void EndTask(Task task)
    {
        if (currentTask != null && currentTask == task)
            currentTask.End();
        else
            return;
    }
}
