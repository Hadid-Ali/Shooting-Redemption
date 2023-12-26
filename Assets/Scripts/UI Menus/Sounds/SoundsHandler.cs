using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class SfxSounds
{
    public AudioClip Clip;
    public SFX Sfx;
}
[Serializable]
public class AmbienceSounds
{
    public AudioClip Clip;
    public Ambience Sfx;
}
public class SoundsHandler : MonoBehaviour, ISoundHandler
{
    [SerializeField] private AudioSource sfx1;
    [SerializeField] private AudioSource sfx2;
    [SerializeField] private AudioSource bg;
    [SerializeField] private AudioSource ambience;

    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private List<SfxSounds> _sfxSounds;
    [SerializeField] private List<AmbienceSounds> _ambienceSounds;

    
    void Awake()
    {
        if(Dependencies.SoundHandler == null)
            Dependencies.SoundHandler = this;
        
    }

    private void Start()
    {
        UpdateSoundStatus();
        PlayBGMusic();
    }

    private void UpdateSoundStatus()
    {
        bool val = Dependencies.GameDataOperations.GetSoundStatus();
        sfx1.mute = !val;
        sfx2.mute = !val;
        bg.mute = !val;
        ambience.mute = !val;
        
        bg.loop = true;
        ambience.loop = true;
    }

    public void PlaySFXSound(SFX sfx)
    {
        UpdateSoundStatus();
        
        var clip = _sfxSounds?.Find(x => x.Sfx == sfx).Clip;
        
        if (!sfx1.isPlaying)
        {
            sfx1.Stop();
            sfx1.clip = clip;
            sfx1.Play();
        }
        else
        {
            sfx2.Stop();
            sfx2.clip = clip;
            sfx2.Play();
        }
    }

    public void PlayAmbienceSound(Ambience ambience)
    {
        UpdateSoundStatus();

        var clip = _ambienceSounds?.Find(x => x.Sfx == ambience).Clip;

        sfx1.Stop();
        sfx1.clip = clip;
        sfx1.Play();
    }
    public void MuteAll()
    {
        Dependencies.GameDataOperations.SetSoundStatus(false);
        UpdateSoundStatus();
    }

    public void UnMuteAll()
    {
        Dependencies.GameDataOperations.SetSoundStatus(true);
        UpdateSoundStatus();
    }

    public void MuteBgMusic()
    {
        bg.Stop();
        bg.clip = null;
    }

    public void MuteAmbience()
    {
        ambience.Stop();
        ambience.clip = null;
    }

    public void PlayBGMusic()
    {
        bg.clip = backgroundMusic;
        bg.Play();
    }
}
