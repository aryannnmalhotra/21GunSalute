using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    private bool isAlive = true;
    private bool isActive;
    private HealthSystem healthSystem;
    private Animator anim;
    private NavMeshAgent navAgent;
    private TaskManager taskManager;
    private Vector3 startPosition;
    private Transform playerReference;
    public AudioSource SoundPlayer;
    public float ShotDamage = 10;
    public float SpotRange = 10;
    public float FireRange = 4;
    public float EscapeRange = 16;
    public float PatrolRadius = 6;
    public GameObject Weapon;
    public GameObject DieEffect;
    public Transform BulletOrigin;
    public ParticleSystem Flash;
    public ParticleSystem Smoke1;
    public ParticleSystem Smoke2;
    public AudioClip FireSound;
    public AudioClip Spotted;
    public AudioClip EnemyDown;
    void Start()
    {
        isActive = false;
        healthSystem = GetComponent<HealthSystem>();
        anim = GetComponent<Animator>();
        taskManager = GetComponent<TaskManager>();
        navAgent = GetComponent<NavMeshAgent>();
        startPosition = transform.position;
        GameObject player = GameObject.FindWithTag("Player");
        int i = 0;
        foreach (Transform child in player.transform)
        {
            if (i == 0)
            {
                playerReference = child.gameObject.GetComponent<Camera>().transform;
            }
            i++;
        }
        InvokeRepeating("PatrolCall", 0.2f, 3);
    }
    void PatrolCall()
    {
        if (healthSystem.GetHealth() > 0)
        {
            if (!isActive)
            {
                float xToAdd = Random.Range(-PatrolRadius, PatrolRadius);
                float zToAdd = Random.Range(-PatrolRadius, PatrolRadius);
                if (xToAdd >= -1.5f && xToAdd <= 1.5f)
                    xToAdd *= 3;
                if (zToAdd >= -1.5f && zToAdd <= 1.5f)
                    zToAdd *= 3;
                Vector3 patrolPoint = startPosition + new Vector3(xToAdd, 0, -(zToAdd));
                taskManager.StartTask(new PatrolTask(taskManager, anim, navAgent, patrolPoint));
            }
        }
    }
    public void DamageInflicted(float damage)
    {
        if (healthSystem.GetHealth() > 0)
        {
            navAgent.ResetPath();
            navAgent.isStopped = true;
            taskManager.PriorityStartTask(new HurtTask(taskManager, anim, healthSystem, damage));
        }
    }
    void Update()
    {
        anim.SetFloat("WalkSpeed", navAgent.velocity.magnitude);
        if (FpsAttributes.IsAlive)
        {
            if (healthSystem.GetHealth() > 0)
            {
                if (Vector3.Distance(transform.position, playerReference.position) <= SpotRange && !isActive)
                {
                    isActive = true;
                    SoundPlayer.PlayOneShot(Spotted);
                    navAgent.ResetPath();
                }
                if (Vector3.Distance(transform.position, playerReference.position) > EscapeRange && isActive)
                    isActive = false;
                if (isActive)
                {
                    if (Vector3.Distance(transform.position, playerReference.position) <= FireRange)
                    {
                        FireRange = SpotRange - SpotRange / 5;
                        taskManager.StartTask(new ShootTask(taskManager, anim, navAgent, BulletOrigin.position, playerReference.position, Flash, Smoke1, Smoke2, ShotDamage, SoundPlayer, FireSound));
                    }
                    if (Vector3.Distance(transform.position, playerReference.position) > FireRange)
                    {
                        taskManager.StartTask(new ChaseTask(taskManager, anim, navAgent, playerReference.position));
                    }
                }
            }
            if (healthSystem.GetHealth() <= 0 && isAlive)
            {
                isAlive = false;
                navAgent.ResetPath();
                navAgent.isStopped = true;
                taskManager.PriorityStartTask(new DieTask(taskManager, anim, DieEffect, SoundPlayer, EnemyDown));
            }
        }
        else
            isActive = false;
    }
}
