using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowOffset : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    private void Awake()
    {
        if (playerTransform == null)
            playerTransform = FindObjectOfType<AIPlayerMovement>().transform;
    }

    void Update()
    {
        transform.position = playerTransform.position + Vector3.up;
    }
}
