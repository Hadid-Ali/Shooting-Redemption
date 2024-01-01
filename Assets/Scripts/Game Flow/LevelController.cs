using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelController : MonoBehaviour
{
    [SerializeField] private float countdownTimer;
    public Ambience ambience;

    private Timer _timer;

    private void Awake()
    {
        Dependencies.SoundHandler.PlayAmbienceSound(ambience);
        GameEvents.GamePlayEvents.OnLevelPause.Register(OnLevelPause);
        GameEvents.GamePlayEvents.OnLevelResumed.Register(OnLevelResume);
        GameEvents.GamePlayEvents.OnPlayerReachedCover.Register(OnCoverReached);
    }

    private void OnDestroy()
    {
        GameEvents.GamePlayEvents.OnLevelPause.Unregister(OnLevelPause);
        GameEvents.GamePlayEvents.OnLevelResumed.Unregister(OnLevelResume);
        GameEvents.GamePlayEvents.OnPlayerReachedCover.Unregister(OnCoverReached);
    }

    private void OnLevelPause()
    {
        Dependencies.SoundHandler.MuteAmbience();
        Dependencies.SoundHandler.PlayBGMusic();
    }

    private void OnLevelResume()
    {
        Dependencies.SoundHandler.PlayAmbienceSound(ambience);
        Dependencies.SoundHandler.MuteBgMusic();
    }

    private void OnCoverReached()
    {
        Dependencies.SoundHandler.PlayAmbienceSound(ambience);
        Dependencies.SoundHandler.MuteBgMusic();
    }

    private void Start()
    {
        _timer = GetComponent<Timer>();
        _timer.countdownTime = countdownTimer + 30;
        _timer.Initialize();

        string episode = "Episode : " + Dependencies.GameDataOperations.GetSelectedEpisode();
        string level = "Level : " + Dependencies.GameDataOperations.GetSelectedLevel();
        FirebaseEvents.logEvent($"{episode} {level} Started");
    }
}
