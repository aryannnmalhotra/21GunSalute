using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    private float health = 100;
    private AudioSource soundPlayer;
    public AudioClip Hurt;
    public AudioClip Heal;
    public Image Healthbar;
    public float GetHealth()
    {
        return health;
    }
    public void IncreaseHealth(float factor)
    {
        health = Mathf.Clamp(health + factor, 0, 100);
        Healthbar.fillAmount = health / 100;
        soundPlayer.PlayOneShot(Heal);
    }
    public void DecreaseHealth(float factor)
    {
        health = Mathf.Clamp(health - factor, 0, 100);
        Healthbar.fillAmount = health / 100;
        soundPlayer.PlayOneShot(Hurt);
    }
    private void Start()
    {
        soundPlayer = GetComponent<AudioSource>();
    }
}
