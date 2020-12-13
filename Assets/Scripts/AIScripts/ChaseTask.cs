using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseTask : Task
{
    private NavMeshAgent navAgent;
    private Animator anim;
    private Vector3 destination;
    public ChaseTask(TaskManager taskManager, Animator anim, NavMeshAgent navAgent, Vector3 destination) : base(taskManager)
    {
        this.navAgent = navAgent;
        this.anim = anim;
        this.destination = destination;
    }
    public override bool Start()
    {
        anim.SetBool("Aim", false);
        navAgent.isStopped = false;
        navAgent.speed = 2.5f;
        navAgent.SetDestination(destination);
        var direction = (TaskManager.gameObject.transform.position - destination).normalized;
        Quaternion directionalRotaion = Quaternion.LookRotation(-(new Vector3(direction.x, 0, direction.z)));
        TaskManager.gameObject.transform.rotation = Quaternion.Slerp(TaskManager.gameObject.transform.rotation, directionalRotaion, 1); TaskManager.gameObject.transform.rotation = Quaternion.Slerp(TaskManager.gameObject.transform.rotation, directionalRotaion, 1);
        return true;
    }
    public override bool End()
    {
        return true;
    }
}
