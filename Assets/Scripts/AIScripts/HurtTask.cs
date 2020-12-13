using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtTask : Task
{
    private float damage;
    private Animator anim;
    private HealthSystem healthSystem;
    public HurtTask(TaskManager taskManager, Animator anim, HealthSystem healthSystem, float damage) : base(taskManager)
    {
        this.damage = damage;
        this.anim = anim;
        this.healthSystem = healthSystem;
    }
    public override bool Start()
    {
        healthSystem.DecreaseHealth(damage);
        TaskManager.StartCoroutine(StartAnimating());
        return true;
    }
    IEnumerator StartAnimating()
    {
        anim.SetBool("Hurt", true);
        yield return new WaitForSeconds(0.8f);
        anim.SetBool("Hurt", false);
        IsTaskComplete = true;
    }
    public override bool End()
    {
        return IsTaskComplete;
    }
}
