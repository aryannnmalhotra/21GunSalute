using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FpsAttributes : MonoBehaviour
{
    private HealthSystem healthSystem;
    public static bool IsAlive;
    void Start()
    {
        IsAlive = true;
        healthSystem = GetComponent<HealthSystem>();
    }
    void Update()
    {
        if (healthSystem.GetHealth() == 0 && IsAlive)
        {
            IsAlive = false;
            SceneManager.LoadScene("LaunchScene"); 
        }
    }
}
