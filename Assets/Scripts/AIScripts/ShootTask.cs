using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootTask : Task
{
    private Animator anim;
    private NavMeshAgent navAgent;
    private Vector3 bulletOrigin;
    private Vector3 target;
    private ParticleSystem flash;
    private ParticleSystem smoke1;
    private ParticleSystem smoke2;
    private float shotDamage;
    private AudioSource soundPlayer;
    private AudioClip fireSound;
    private float aimTime;
    public ShootTask(TaskManager taskManager, Animator anim, NavMeshAgent navAgent, Vector3 bulletOrigin, Vector3 target, ParticleSystem flash, ParticleSystem smoke1, ParticleSystem smoke2, float shotDamage, AudioSource soundPlayer, AudioClip fireSound) : base(taskManager)
    {
        this.anim = anim;
        this.navAgent = navAgent;
        this.bulletOrigin = bulletOrigin;
        this.target = target;
        this.flash = flash;
        this.smoke1 = smoke1;
        this.smoke2 = smoke2;
        this.shotDamage = shotDamage;
        this.soundPlayer = soundPlayer;
        this.fireSound = fireSound;
        aimTime = Random.Range(0.5f, 1.5f);
    }
    public override bool Start()
    {
        navAgent.ResetPath();
        navAgent.speed = 0;
        navAgent.isStopped = true;
        var direction = (TaskManager.gameObject.transform.position - target).normalized;
        Quaternion directionalRotaion = Quaternion.LookRotation(-(new Vector3(direction.x, 0, direction.z)));
        TaskManager.gameObject.transform.rotation = Quaternion.Slerp(TaskManager.gameObject.transform.rotation, directionalRotaion, 1);
        anim.SetBool("Aim", true);
        TaskManager.StartCoroutine(StartShooting());
        return true;
    }
    IEnumerator StartShooting()
    {
        yield return new WaitForSeconds(aimTime);
            RaycastHit shot;
            anim.SetBool("Fire", true);
            flash.Play();
            smoke1.Play();
            smoke2.Play();
            soundPlayer.PlayOneShot(fireSound);
            if (Physics.Raycast(bulletOrigin, TaskManager.gameObject.transform.forward, out shot))
            {
                Debug.Log(shot.transform.gameObject.name);
                if (shot.transform.parent.GetComponent<FpsAttributes>())
                    shot.transform.parent.GetComponent<HealthSystem>().DecreaseHealth(shotDamage);
            }
        yield return new WaitForSeconds(0.5f);
            anim.SetBool("Fire", false);
            flash.Stop();
            smoke1.Stop();
            smoke2.Stop();
        IsTaskComplete = true;
    }
    public override bool End()
    {
        return IsTaskComplete;
    }
}
