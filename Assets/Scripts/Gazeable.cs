using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gazeable : MonoBehaviour
{
    private bool hasBeenAnswered;
    private AudioSource uISoundPlayer;
    public bool IsAnswer;
    public bool IsRightAnswer;
    public bool IsNextQuestion;
    public bool IsWarningMessage;
    public GameObject NextToSpawn;
    public GameObject[] Questions;
    public AudioClip Button;
    void Start()
    {
        uISoundPlayer = GameObject.FindWithTag("UIPlayer").GetComponent<AudioSource>();
    }
    public bool HasBeenPicked()
    {
        return (hasBeenAnswered);
    }
    public void GazeAction(Camera aRCamera)
    {
        if (IsAnswer)
        {
            HealthSystem playerHealth = GameObject.FindWithTag("Player").GetComponent<HealthSystem>();
            if (IsRightAnswer)
            {
                playerHealth.IncreaseHealth(10);
                GameObject nextToSpawn = Instantiate(NextToSpawn) as GameObject;
                nextToSpawn.transform.position = aRCamera.transform.position + (aRCamera.transform.forward * 1.5f) + (Vector3.up * 0.4f);
                Quaternion rotation = Quaternion.LookRotation(aRCamera.transform.right, -aRCamera.transform.forward);
                nextToSpawn.transform.rotation = rotation;
                nextToSpawn.transform.Rotate(0, 0, -15);
                Destroy(transform.parent.gameObject);
            }
            else
            {
                if (!hasBeenAnswered)
                {
                    playerHealth.DecreaseHealth(20);
                    hasBeenAnswered = true;
                }
            }
        }
        else if (IsWarningMessage)
        {
            GameObject.FindWithTag("Controller").GetComponent<Controller>().GrantWeapon();
            GameObject.FindWithTag("Controller").GetComponent<Controller>().SpawnEnemy();
            uISoundPlayer.PlayOneShot(Button);
            Destroy(transform.parent.gameObject);
        }
        else
        {
            GameObject nextToSpawn;
            if (IsNextQuestion)
            {
                int i = Random.Range(0, 3);
                nextToSpawn = Instantiate(Questions[i]) as GameObject;
                nextToSpawn.transform.position = aRCamera.transform.position + (aRCamera.transform.forward * 1.5f) + (aRCamera.transform.up * 0.26f) + (Vector3.up * 0.4f);
            }
            else
            {
                nextToSpawn = Instantiate(NextToSpawn) as GameObject;
                nextToSpawn.transform.position = aRCamera.transform.position + (aRCamera.transform.forward * 1.5f) + (Vector3.up * 0.4f);
            }
            Quaternion rotation = Quaternion.LookRotation(aRCamera.transform.right, -aRCamera.transform.forward);
            nextToSpawn.transform.rotation = rotation;
            nextToSpawn.transform.Rotate(0, 0, -15);
            uISoundPlayer.PlayOneShot(Button);
            Destroy(transform.parent.gameObject);
        }
    }
    void Update()
    {
        
    }
}
