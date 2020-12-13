using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FpsAttributes : MonoBehaviour
{
    private float alphaValue;
    private HealthSystem healthSystem;
    public Image Black;
    public AudioSource UIAudioPlayer;
    public AudioSource BGAudio;
    public AudioClip youDied;
    public static bool IsAlive;
    void Start()
    {
        alphaValue = 0;
        IsAlive = true;
        healthSystem = GetComponent<HealthSystem>();
    }
    void LoadScreen()
    {
        SceneManager.LoadScene("LaunchScene");
    }
    void EndCue()
    {
        UIAudioPlayer.PlayOneShot(youDied);
    }
    void Update()
    {
        if (healthSystem.GetHealth() == 0 && IsAlive)
        {
            IsAlive = false;
            transform.GetChild(0).GetComponent<WeaponScroll>().SetIndex(-1);
            transform.GetChild(0).GetComponent<WeaponScroll>().enabled = false;
            BGAudio.Stop();
            Invoke("EndCue", 2);
            Invoke("LoadScreen", 4);
        }
        if (!IsAlive)
        {
            alphaValue = Mathf.Clamp(alphaValue + Time.deltaTime / 4, 0, 0.5f);
            Color color = Black.color;
            color.a = alphaValue;
            Black.color = color;
        }
    }
}
