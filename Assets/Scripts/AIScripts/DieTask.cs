using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieTask : Task
{
    Animator anim;
    private GameObject dieEffect;
    private AudioSource soundPlayer;
    private AudioClip enemyDown;
    public DieTask(TaskManager taskManager, Animator anim, GameObject dieEffect, AudioSource soundPlayer, AudioClip enemyDown) : base(taskManager)
    {
        this.anim = anim;
        this.dieEffect = dieEffect;
        this.soundPlayer = soundPlayer;
        this.enemyDown = enemyDown;
    }
    public override bool Start()
    {
        anim.SetBool("Die", true);
        TaskManager.StartCoroutine(SpawnCollectibles());
        return true;
    }
    IEnumerator SpawnCollectibles()
    {
        yield return new WaitForSeconds(3);
        soundPlayer.PlayOneShot(enemyDown);
        yield return new WaitForSeconds(1);
        TaskManager.Instantiate(dieEffect, TaskManager.gameObject.transform.position, Quaternion.LookRotation(Vector3.up));
        yield return new WaitForSeconds(0.5f);
        TaskManager.Destroy(TaskManager.gameObject);
        GameObject.FindWithTag("Controller").GetComponent<Controller>().SignifyDeath();
    }

    public override bool End()
    {
        return false;
    }
}
