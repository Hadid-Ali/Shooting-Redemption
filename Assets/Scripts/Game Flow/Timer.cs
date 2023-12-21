using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [HideInInspector] public float countdownTime; // Set the countdown time in seconds
    private float _currentTime;

    private bool _countdownCompleted;
    private bool _timerInitialized;

    public static GameEvent<string> OnTimerUIUpdate = new();
    public static GameEvent StopTimer = new();


    private void Awake()
    {
        GameEvents.GamePlayEvents.TimeOver.Register(UnInitilizeTimer);
        GameEvents.GamePlayEvents.OnAllGroupsCleared.Register(UnInitilizeTimer);
        StopTimer.Register(UnInitilizeTimer);
    }

    private void OnDestroy()
    {
        GameEvents.GamePlayEvents.TimeOver.Unregister(UnInitilizeTimer);
        GameEvents.GamePlayEvents.OnAllGroupsCleared.Unregister(UnInitilizeTimer);
        StopTimer.Unregister(UnInitilizeTimer);
    }
    public void UnInitilizeTimer()
    {
        _timerInitialized = false;
        print("TimerUninitialized");
    }

    public void Initialize()
    {
        _currentTime = countdownTime;
        _timerInitialized = true;
    }

    void Update()
    {
        if(_countdownCompleted || _timerInitialized == false)
            return;
        
        _currentTime -= Time.deltaTime;
        
        _currentTime = Mathf.Max(_currentTime, 0f);


        int minutes = Mathf.FloorToInt(_currentTime / 60);
        int seconds = Mathf.FloorToInt(_currentTime % 60);


        string timerText = string.Format("{0:00}:{1:00}", minutes, seconds);
        
        OnTimerUIUpdate.Raise(timerText);
        
        if (_currentTime <= 0f)
        {
            CountdownCompleted();
        }
    }

    void CountdownCompleted()
    {
        _countdownCompleted = true;
        GameEvents.GamePlayEvents.TimeOver.Raise();

        CharacterStates.gameState = GameStates.GameOver;
    }
}
