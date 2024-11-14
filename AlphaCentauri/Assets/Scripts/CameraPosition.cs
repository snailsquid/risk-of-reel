using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public Transform cameraposition;
    void Start()
    {
        
    }
    void Update()
    {
        transform.position = cameraposition.position;
    }
}
