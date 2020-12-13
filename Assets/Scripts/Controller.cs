using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    private bool isEnemyThere;
    private bool hasExperienceStarted;
    private bool isReticleEnabled;
    private bool ongoingEducationalExperience;
    private bool isFreeRoamingEnabled;
    private int experiencePhase;
    private int parametersPhase;
    private int educationalWeaponIndex;
    private float reticleFillAmount;
    private List<AugmentedImage> trackedMarkers = new List<AugmentedImage>();
    public float SpawnRadius;
    public Camera ARCamera;
    public GameObject[] EducationalPanels; 
    public GameObject PlaneDetectMessage;
    public GameObject StartMarkerMessage;
    public GameObject FreeRoamMessage;
    public GameObject HealthBar;
    public GameObject Enemy;
    public GameObject ARCoreDevice;
    public GameObject Parameters;
    public Image Reticle;
    public Image ReticleUpper;
    public Image ReticleLower;
    public PlaneDetector PlaneDetector;
    public WeaponScroll Scroller;
    void Start()
    {
        experiencePhase = 0;
        parametersPhase = 0;
        PlaneDetectMessage.SetActive(true);
    }
    void reticleCheck()
    {
        Ray ray = new Ray(ARCamera.transform.position, ARCamera.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.GetComponent<Gazeable>() != null)
            {
                if (hit.transform.gameObject.GetComponent<Gazeable>().IsAnswer)
                {
                    if (!hit.transform.gameObject.GetComponent<Gazeable>().HasBeenPicked())
                    {
                        if (!isReticleEnabled)
                        {
                            reticleFillAmount = 0;
                            ReticleUpper.fillAmount = (1 - reticleFillAmount);
                            ReticleLower.fillAmount = reticleFillAmount;
                            Reticle.enabled = true;
                            ReticleUpper.enabled = true;
                            ReticleLower.enabled = true;
                            isReticleEnabled = true;
                        }
                        else
                        {
                            reticleFillAmount = Mathf.Clamp(reticleFillAmount + Time.deltaTime / 3, 0, 1);
                            ReticleUpper.fillAmount = (1 - reticleFillAmount);
                            ReticleLower.fillAmount = reticleFillAmount;
                            if (reticleFillAmount >= 1)
                            {
                                Reticle.enabled = false;
                                ReticleUpper.enabled = false;
                                ReticleLower.enabled = false;
                                isReticleEnabled = false;
                                hit.transform.gameObject.GetComponent<Gazeable>().GazeAction(ARCamera);
                                if (experiencePhase == 1)
                                {
                                    parametersPhase++;
                                    if (parametersPhase == 3)
                                    {
                                        experiencePhase = 2;
                                        educationalWeaponIndex = 0;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (!isReticleEnabled)
                    {
                        reticleFillAmount = 0;
                        ReticleUpper.fillAmount = (1 - reticleFillAmount);
                        ReticleLower.fillAmount = reticleFillAmount;
                        Reticle.enabled = true;
                        ReticleUpper.enabled = true;
                        ReticleLower.enabled = true;
                        isReticleEnabled = true;
                    }
                    else
                    {
                        reticleFillAmount = Mathf.Clamp(reticleFillAmount + Time.deltaTime / 3, 0, 1);
                        ReticleUpper.fillAmount = (1 - reticleFillAmount);
                        ReticleLower.fillAmount = reticleFillAmount;
                        if (reticleFillAmount >= 1)
                        {
                            Reticle.enabled = false;
                            ReticleUpper.enabled = false;
                            ReticleLower.enabled = false;
                            isReticleEnabled = false;
                            hit.transform.gameObject.GetComponent<Gazeable>().GazeAction(ARCamera);
                            if (experiencePhase == 1)
                            {
                                parametersPhase++;
                                if (parametersPhase == 3)
                                {
                                    experiencePhase = 2;
                                    educationalWeaponIndex = 0;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Reticle.enabled = false;
                ReticleUpper.enabled = false;
                ReticleLower.enabled = false;
                isReticleEnabled = false;
            }
        }
        else
        {
            Reticle.enabled = false;
            ReticleUpper.enabled = false;
            ReticleLower.enabled = false;
            isReticleEnabled = false;
        }
    }
    private void nextEducatePanel()
    {
        GameObject educatePanel = Instantiate(EducationalPanels[educationalWeaponIndex]) as GameObject;
        educatePanel.transform.position = (ARCamera.transform.position) + (ARCamera.transform.forward * 1.5f) + (Vector3.up * 0.4f);
        Quaternion rotation = Quaternion.LookRotation(ARCamera.transform.right, -ARCamera.transform.forward);
        educatePanel.transform.rotation = rotation;
    }
    public void SignifyDeath()
    {
        if (!isFreeRoamingEnabled)
        {
            Scroller.SetIndex(-1);
            ongoingEducationalExperience = false;
            educationalWeaponIndex++;
            if (educationalWeaponIndex >= 5)
            {
                experiencePhase = 3;
                FreeRoamMessage.SetActive(true);
            }
            else
                nextEducatePanel();
        }
        else
            isEnemyThere = false;
    }
    public void GrantWeapon()
    {
        Scroller.SetIndex(educationalWeaponIndex);
        ongoingEducationalExperience = true;
    }
    public void SpawnEnemy()
    {
        float xToAdd = Random.Range(-SpawnRadius, SpawnRadius);
        float zToAdd = Random.Range(-SpawnRadius, SpawnRadius);
        if (xToAdd >= -0.4f && xToAdd <= 0.4f)
            xToAdd *= 3;
        if (zToAdd >= -0.4f && zToAdd <= 0.4f)
            zToAdd *= 3;
        Vector3 spawnPoint = ARCamera.transform.position + new Vector3(xToAdd, PlaneDetector.GetYValue(), -(zToAdd));
        Instantiate(Enemy, spawnPoint, Quaternion.identity);
    }
    void Update()
    {
        if (Session.Status != SessionStatus.Tracking)
        {
            return;
        }
        if (experiencePhase == 0)
        {
            if (PlaneDetector.GetIsGroundPlaneCreated())
            {
                PlaneDetectMessage.SetActive(false);
                StartMarkerMessage.SetActive(true);
                experiencePhase = 1;
            }
        }
        else if (experiencePhase == 1)
        {
            if (!hasExperienceStarted)
            {
                Session.GetTrackables<AugmentedImage>(trackedMarkers, TrackableQueryFilter.Updated);
                foreach (AugmentedImage marker in trackedMarkers)
                {
                    if (marker.Name == "StartMarker")
                    {
                        StartMarkerMessage.SetActive(false);
                        GameObject parameters = Instantiate(Parameters) as GameObject;
                        parameters.transform.position = (ARCamera.transform.position) + (ARCamera.transform.forward * 1.5f) + (Vector3.up * 0.2f);
                        Quaternion rotation = Quaternion.LookRotation(ARCamera.transform.right, -ARCamera.transform.forward);
                        parameters.transform.rotation = rotation;
                        hasExperienceStarted = true;
                    }
                }
            }
            else
            {
                reticleCheck();
            }
        }
        else if (experiencePhase == 2)
        {
            HealthBar.SetActive(true);
            if (!ongoingEducationalExperience)
            {
                reticleCheck();
            }
        }
        else if (experiencePhase == 3)
        {
            if (!isFreeRoamingEnabled)
            {
                Session.GetTrackables<AugmentedImage>(trackedMarkers, TrackableQueryFilter.Updated);
                foreach (AugmentedImage marker in trackedMarkers)
                {
                    if (marker.Name == "FreeRoamMarker")
                    {
                        FreeRoamMessage.SetActive(false);
                        Scroller.SetFreeRoam();
                        isFreeRoamingEnabled = true;
                    }
                }
            }
            else
            {
                if (Input.touchCount == 2 && !isEnemyThere)
                {
                    SpawnEnemy();
                    isEnemyThere = true;
                }
            }
        }
    }
}
