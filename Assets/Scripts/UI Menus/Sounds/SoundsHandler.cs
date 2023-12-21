using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsHandler : MonoBehaviour, ISoundHandler
{
    [SerializeField] private AudioSource AS;
    [SerializeField] private AudioSource Bg;
    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private AudioClip changeSelectionSound;
    [SerializeField] private AudioClip _coinsSound;

    
    void Awake()
    {
        Bg.Play();
        if(Dependencies.SoundHandler == null)
            Dependencies.SoundHandler = this;
    }

    private void Start()
    {
        UpdateSoundStatus();
    }
    
    private void UpdateSoundStatus()
    {
        if (Dependencies.GameDataOperations.GetSoundStatus())
        {
            AS.mute = false;
            Bg.mute = false;
        }
        else
        {
            AS.mute = true;
            Bg.mute = true;
        }
    }
    
    public void BtnClickSound(ButtonType buttonType)
    {
        UpdateSoundStatus();
        switch (buttonType)
        {
            case ButtonType.Play:
            case ButtonType.CharactersPanel:
            case ButtonType.GunsPanel:
            case ButtonType.SelectLevel:
            case ButtonType.SelectEpisode:
            case ButtonType.AddCoins:
            case ButtonType.Buy:
            case ButtonType.TryForFree:
            case ButtonType.Exit:
            case ButtonType.Settings:
                AS.Stop();
                AS.clip = buttonSound;
                AS.Play();
                break;
            case ButtonType.ScrollLeft:
            case ButtonType.ScrollRight:
                AS.Stop();
                AS.clip = changeSelectionSound;
                AS.Play();
                break;
        }
    }

    public void PlayCoinsSound()
    {
        AS.Stop();
        AS.clip = _coinsSound;
        AS.Play();
    }
}
