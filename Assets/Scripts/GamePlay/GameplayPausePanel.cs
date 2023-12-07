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
        Time.timeScale = 0.001f;
        sound.isOn = Dependencies.GameDataOperations.GetSoundStatus();

        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().SetTrigger(Play);
    }

    protected override void OnMenuContainerDisable()
    {
        Time.timeScale = 1;
        GetComponent<Animator>().enabled = false;
    }
    
}
