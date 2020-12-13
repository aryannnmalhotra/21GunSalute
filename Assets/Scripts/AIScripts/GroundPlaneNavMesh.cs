using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundPlaneNavMesh : MonoBehaviour
{
    private NavMeshSurface groundPlaneNavMesh;
    void Start()
    {
        groundPlaneNavMesh = GetComponent<NavMeshSurface>();
        groundPlaneNavMesh.BuildNavMesh();
    }
    void Update()
    {
        
    }
}
