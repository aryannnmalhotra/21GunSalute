using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    private bool isReloading;
    private int currentAmmo;
    private float currentFireStamp;
    private Animator anim;
    private AudioSource soundPlayer;
    private Touch touch;
    public int AmmoPerRound = 5;
    public float ReloadTime = 3.5f;
    public float Damage = 10;
    public float Range = 30;
    public float ShotForce = 10;
    public float FireRate = 5;
    public Camera FpsCam;
    public GameObject OnShot;
    public ParticleSystem Flash;
    public ParticleSystem Smoke1;
    public ParticleSystem Smoke2;
    public AudioClip Reloading;
    public AudioClip Shooting;
    public AudioClip ReloadVoice;
    public WeaponScroll Scroller;
    private void Awake()
    {
        currentAmmo = AmmoPerRound;
    }
    void Start()
    {
        isReloading = false;
        currentFireStamp = 0;
        soundPlayer = GetComponent<AudioSource>();
    }
    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }
    private void OnEnable()
    {
        isReloading = false;
        anim = GetComponent<Animator>();
        anim.SetBool("Reload", false);
    }
    void Shoot()
    {
        anim.SetBool("Shoot", true);
        Invoke("ShootReset", 0.1f);
        soundPlayer.PlayOneShot(Shooting);
        Flash.Play();
        Smoke1.Play();
        Smoke2.Play();
        RaycastHit shot;
        if (Physics.Raycast(FpsCam.transform.position, FpsCam.transform.forward, out shot, Range))
        {
            Enemy shotEnem = shot.transform.GetComponent<Enemy>();
            if (shotEnem != null)
            {
                shotEnem.DamageInflicted(Damage);
            }
            if (shot.rigidbody != null)
                shot.rigidbody.AddForce(-shot.normal * ShotForce);
            Instantiate(OnShot, shot.point, Quaternion.LookRotation(shot.normal));
        }
        currentAmmo--;
    }
    void ShootReset()
    {
        anim.SetBool("Shoot", false);
    }
    IEnumerator Reload()
    {
        isReloading = true;
        Scroller.enabled = false;
        anim.SetBool("Reload", true);
        soundPlayer.PlayOneShot(Reloading);
        soundPlayer.PlayOneShot(ReloadVoice);
        Invoke("StopReloading", ReloadTime);
        yield return new WaitForSeconds(ReloadTime - 0.25f);
        anim.SetBool("Reload", false);
        yield return new WaitForSeconds(0.25f);
        Scroller.enabled = true;
        currentAmmo = AmmoPerRound;
        isReloading = false;
    }
    void StopReloading()
    {
        soundPlayer.Stop();
    }
    void Update()
    {
        if (isReloading)
            return;
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        if (Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Stationary)
            {
                if (Time.time >= currentFireStamp)
                {
                    currentFireStamp = Time.time + 1 / FireRate;
                    Shoot();
                }
            }
        }
    }
}
