using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsHandler : MonoBehaviour,ISoundHandler
{
    [SerializeField] private AudioSource AS;
    [SerializeField] private AudioSource Bg;
    [SerializeField] private AudioClip playClip;
    [SerializeField] private AudioClip SettingClip;
    /*[SerializeField] private AudioClip playClip;
    [SerializeField] private AudioClip playClip;
    [SerializeField] private AudioClip playClip;
    [SerializeField] private AudioClip playClip;*/
    
    void Awake()
    {
        
        Bg.Play();
        Dependencies.SoundHandler = this;
        
    }

    private void Start()
    {
        updateSoundStatus();
    }

    private void updateSoundStatus()
    {
        if (Dependencies.GameDataOperations.GetSound())
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
        updateSoundStatus();
        HapticHandler.Vibrate();
        switch (buttonType)
        {
            case ButtonType.play:
                AS.Stop();
                AS.clip = playClip;
                AS.Play();
                break;
            case ButtonType.setting:
                AS.Stop();
                AS.clip = SettingClip;
                AS.Play();
                break;
        }
    }
}
