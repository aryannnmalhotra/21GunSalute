using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolTask : Task
{
    private NavMeshAgent navAgent;
    private Animator anim;
    private Vector3 patrolPoint;
    private float idleTime;
    public PatrolTask(TaskManager taskManager, Animator anim, NavMeshAgent navAgent, Vector3 patrolPoint) : base(taskManager)
    {
        this.navAgent = navAgent;
        this.anim = anim;
        this.patrolPoint = patrolPoint;
        idleTime = Random.Range(0.5f, 2);
    }
    public  override bool Start()
    {
        anim.SetBool("Aim", false);
        navAgent.isStopped = false;
        navAgent.speed = 0.5f;
        TaskManager.StartCoroutine(StartPatrol());
        return true;
    }
    IEnumerator StartPatrol()
    {
        yield return new WaitForSeconds(idleTime);
        Vector3 direction = (patrolPoint - navAgent.gameObject.transform.position).normalized;
        Quaternion directionalRotaion = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        navAgent.gameObject.transform.rotation = Quaternion.Slerp(navAgent.gameObject.transform.rotation, directionalRotaion, 1);
        navAgent.SetDestination(patrolPoint);
    }
    public override bool End()
    {
        return true;
    }
}
