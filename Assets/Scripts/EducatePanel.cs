using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EducatePanel : MonoBehaviour
{
    private Camera aRCamera;
    private Touch touch;
    private Vector2 touchStart;
    private Vector2 touchStartOG;
    private Touch touch1;
    private Touch touch2;
    private Vector2 touchStart1;
    private Vector2 touchStart2;
    private Vector2 currentDistance;
    private Vector2 previousDistance;
    private Vector2 touchEnd1;
    private Vector2 touchEnd2;
    private Vector2 touchEnd;
    private AudioSource soundPlayer;
    private AudioSource zoomAudioPlayer;
    public AudioClip Shifting;
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        int i = 0;
        foreach (Transform child in player.transform)
        {
            if (i == 0)
            {
                aRCamera = child.gameObject.GetComponent<Camera>();
            }
            i++;
        }
        soundPlayer = GetComponent<AudioSource>();
        zoomAudioPlayer = GameObject.FindWithTag("Zoom").GetComponent<AudioSource>();
    }
    void Update()
    {
        if (Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                soundPlayer.Stop();
                zoomAudioPlayer.Stop();
                touchStart = touch.position;
                touchStartOG = touchStart;
                touchEnd = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                touchEnd = touch.position;
                float verticalSwipeDistance = Mathf.Abs(touchStart.y - touchEnd.y);
                float horizontalSwipeDistance = Mathf.Abs(touchStart.x - touchEnd.x);
                if (verticalSwipeDistance >= 30)
                {
                    transform.RotateAround(transform.position, aRCamera.transform.right, 2 * Time.deltaTime * touch.deltaPosition.y);
                    soundPlayer.PlayOneShot(Shifting);
                }
                if (horizontalSwipeDistance >= 30)
                {
                    transform.RotateAround(transform.position, aRCamera.transform.up, -(2 * Time.deltaTime * touch.deltaPosition.x));
                    soundPlayer.PlayOneShot(Shifting);
                }
            }
        }
        else if (Input.touchCount == 2)
        {
            touch1 = Input.GetTouch(0);
            touch2 = Input.GetTouch(1);
            if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                currentDistance = touch1.position - touch2.position;
                previousDistance = (touch1.position - touch1.deltaPosition) - (touch2.position - touch2.deltaPosition);
                float touchDelta = currentDistance.magnitude - previousDistance.magnitude;
                if (touchDelta > 0)
                {
                    zoomAudioPlayer.Play();
                    transform.position -= aRCamera.transform.forward * Time.deltaTime;
                }
                else
                {
                    zoomAudioPlayer.Play();
                    transform.position += aRCamera.transform.forward * Time.deltaTime;
                }
            }
        }
        if (Input.touchCount != 2)
        {
            zoomAudioPlayer.Stop();
        }
    }
}
