using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchUI : MonoBehaviour
{
    public AudioSource UIAudioPlayer;
    public GameObject HelpScreen;
    public GameObject ControlsScreen;
    public AudioClip Button;
    public void Help()
    {
        UIAudioPlayer.Stop();
        UIAudioPlayer.PlayOneShot(Button);
        HelpScreen.SetActive(true);
    }
    public void Controls()
    {
        UIAudioPlayer.Stop();
        UIAudioPlayer.PlayOneShot(Button);
        ControlsScreen.SetActive(true);
    }
    public void Home()
    {
        UIAudioPlayer.Stop();
        UIAudioPlayer.PlayOneShot(Button);
        HelpScreen.SetActive(false);
        ControlsScreen.SetActive(false);
    }
    public void StartExperience()
    {
        UIAudioPlayer.Stop();
        UIAudioPlayer.PlayOneShot(Button);
        SceneManager.LoadScene("ARExperience");
    }
}