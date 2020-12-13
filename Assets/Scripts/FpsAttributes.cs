using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FpsAttributes : MonoBehaviour
{
    private float alphaValue;
    private HealthSystem healthSystem;
    public Controller ARController;
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
    void Update()
    {
        if (healthSystem.GetHealth() == 0 && IsAlive)
        {
            IsAlive = false;
            ARController.Scroller.SetIndex(-1);
            ARController.Scroller.enabled = false;
            BGAudio.Stop();
            UIAudioPlayer.PlayOneShot(youDied);
            Invoke("LoadScreen", 4);
        }
        if (!IsAlive)
        {
            Color color = Black.color;
            color.a = Mathf.Clamp(alphaValue + Time.deltaTime / 4, 0, 0.5f); 
            Black.color = color;
        }
    }
}
