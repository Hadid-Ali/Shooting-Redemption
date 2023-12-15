using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private float countdownTimer;

    private Timer _timer;

    private void Start()
    {
        _timer = GetComponent<Timer>();
        _timer.countdownTime = countdownTimer;
        _timer.Initialize();
    }
}
