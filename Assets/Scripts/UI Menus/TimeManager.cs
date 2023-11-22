using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonobehaviourSingleton<TimeManager>
{
    private TimeSpan timeRemaining;
    public DateTime _DateTime;
    public Text timeTxt;
    private void Start()
    {
        SaveLoadData.SaveData();
        
        string timeString = SaveLoadData.GameData.DateTime;
        long binaryTime = Convert.ToInt64(timeString);
        _DateTime = DateTime.FromBinary(binaryTime);
    }

    private void Update()
    {
        TimeSpan timePassed = DateTime.Now.Subtract(_DateTime) ;
        timeRemaining = TimeSpan.FromDays(1) - timePassed; // 1 day is 24 hours

        // Display or use timePassed and timeRemaining as needed
        //Debug.Log($"Time Passed: {timePassed.Hours} hours, {timePassed.Minutes} minutes");
        //Debug.Log($"Time Remaining: {timeRemaining.Hours} hours, {timeRemaining.Minutes} minutes");

        timeTxt.text = $"Reset IN: {timeRemaining.Hours}:{timeRemaining.Minutes}:{timeRemaining.Seconds} ";
    }

    public int GetTime()
    {
        return timeRemaining.Seconds;
    }
}
