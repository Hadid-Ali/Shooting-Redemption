using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayPausePanel : UIMenuBase
{
    [SerializeField] private Toggle sound;
    private static readonly int Play = Animator.StringToHash("Play");

    protected override void OnMenuContainerEnable()
    {
        sound.isOn = Dependencies.GameDataOperations.GetSoundStatus();

        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().SetTrigger(Play);
        
        AdHandler.ShowInterstitial();
        
        PlayerCanvasScipt.DeActive();
    }

    protected override void OnMenuContainerDisable()
    {
        GetComponent<Animator>().enabled = false;
        PlayerCanvasScipt.SetActive();
    }
    
}
