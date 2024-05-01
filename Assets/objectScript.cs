using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class objectScript : MonoBehaviour
{
    public float raycastDistance = 1000f;
    public GameObject obj;
    public NavMeshSurface surface;

    public void startGame()
    {
        Start();
    }

    void Start()
    {
        PositionRaycast();
        surface.BuildNavMesh();
    }

    void PositionRaycast()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, raycastDistance))
        {
            Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
            obj.transform.rotation = spawnRotation;
            Debug.Log("Working!!!");
        }
        else;
            Debug.Log("not working");
    }
}