using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{
    protected TaskManager TaskManager;
    public bool IsTaskComplete = false;
    public Task(TaskManager taskManager)
    {
        this.TaskManager = taskManager;
    }
    public virtual bool Start()
    {
        return true;
    }
    public virtual bool End()
    {
        return true;
    }
}
