using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class PlaneDetector : MonoBehaviour
{
    private bool isGroundPlaneCreated;
    private GameObject groundPlane;
    private List<DetectedPlane> newPlanes = new List<DetectedPlane>();
    public GameObject PlanePrefab;
    void Start()
    {
        
    }
    public bool GetIsGroundPlaneCreated()
    {
        return isGroundPlaneCreated;
    }
    public float GetYValue()
    {
        return (groundPlane.transform.position.y + 0.1f);
    }
    void Update()
    {
        if (Session.Status != SessionStatus.Tracking)
        {
            return;
        }
        if (isGroundPlaneCreated)
        {
            return;
        }
        Session.GetTrackables<DetectedPlane>(newPlanes, TrackableQueryFilter.New);
        for (int i = 0; i < newPlanes.Count; i++)
        {
            if (!isGroundPlaneCreated)
            {
                if (newPlanes[i].PlaneType == DetectedPlaneType.HorizontalUpwardFacing || newPlanes[i].PlaneType == DetectedPlaneType.HorizontalDownwardFacing)
                {
                    groundPlane = Instantiate(PlanePrefab, newPlanes[i].CenterPose.position, newPlanes[i].CenterPose.rotation, transform);
                    isGroundPlaneCreated = true;
                }
            }
        }
    }
}
