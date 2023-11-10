using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float countdownTime = 60f; // Set the countdown time in seconds
    private float currentTime;

    void Start()
    {
        currentTime = countdownTime;
    }

    void Update()
    {
        // Update the timer
        currentTime -= Time.deltaTime;

        // Check if the timer has reached zero
        if (currentTime <= 0f)
        {
            
            ResetTimer();
        }
    }

    void ResetTimer()
    {
        currentTime = countdownTime;
    }
}
