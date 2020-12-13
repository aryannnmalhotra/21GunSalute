using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScroll : MonoBehaviour
{
    private bool isSubsequentEnable;
    private bool isFreeRoamEnabled;
    private int currentWeapon;
    private AudioSource soundPlayer;
    private Vector2 touchStart;
    private Vector2 touchEnd;
    private Touch touch;
    public AudioClip WeaponSwitch;
    void Start()
    {
        currentWeapon = -1;
        soundPlayer = GetComponent<AudioSource>();
        SelectWeapon();
    }
    private void OnEnable()
    {
        if (!isSubsequentEnable)
        {
            currentWeapon = 0;
            soundPlayer = GetComponent<AudioSource>();
            isSubsequentEnable = true;
        }
        SelectWeapon();
    }
    void SelectWeapon()
    {
        int i = 0; 
        foreach(Transform weapon in transform)
        {
            if (currentWeapon == i)
            {
                weapon.gameObject.SetActive(true);
                soundPlayer.PlayOneShot(WeaponSwitch);
            }
            else
            {
                if (i != 5)
                    weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
    public void SetIndex(int weaponIndex)
    {
        currentWeapon = weaponIndex;
        SelectWeapon();
    }
    public void SetFreeRoam()
    {
        isFreeRoamEnabled = true;
        currentWeapon = 0;
        SelectWeapon();
    }
    void Update()
    {
        if (isFreeRoamEnabled)
        {
            if (Input.touchCount == 1)
            {
                touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    touchStart = touch.position;
                    touchEnd = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    touchEnd = touch.position;
                    float verticalSwipeDistance = Mathf.Abs(touchStart.y - touchEnd.y);
                    float horizontalSwipeDistance = Mathf.Abs(touchStart.x - touchEnd.x);
                    if (verticalSwipeDistance >= 20 || horizontalSwipeDistance >= 20)
                    {
                        if (verticalSwipeDistance >= horizontalSwipeDistance)
                        {
                            if (touchStart.y > touchEnd.y)
                            {

                                //SwipeDown
                                if (currentWeapon > 0)
                                    currentWeapon--;
                                else
                                    currentWeapon = 4;
                                SelectWeapon();
                            }
                            else
                            {
                                //Swipe Up
                                if (currentWeapon < 4)
                                    currentWeapon++;
                                else
                                    currentWeapon = 0;
                                SelectWeapon();
                            }
                        }
                    }
                }
            }
        }
    }
}
