using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowOffset : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTransform.position + Vector3.up;
    }
}
