using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private Transform cameraTransform;
    private void Awake()
    {
        if (Camera.main != null) cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // Face the camera
        var rotation = cameraTransform.rotation;
        
        transform.LookAt(transform.position + rotation * Vector3.forward,
            rotation * Vector3.up);
    }
    
}
