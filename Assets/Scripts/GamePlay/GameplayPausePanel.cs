using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayPausePanel : UIMenuBase
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button resumeButton;

    [SerializeField] private Toggle sound;

    protected override void OnMenuContainerEnable()
    {
        Time.timeScale = 0.001f;
        sound.isOn = Dependencies.GameDataOperations.GetSoundStatus();
    }

    protected override void OnMenuContainerDisable()
    {
        Time.timeScale = 1;
    }

    private void Start()
    {
        mainMenuButton.onClick.AddListener(OnClickMainMenu);
        restartButton.onClick.AddListener(OnClickRestart);
        resumeButton.onClick.AddListener(OnClickResume);
    }

    private void OnClickResume() => OnResumeClicked();
    private void OnClickRestart() => OnRestartClicked();
    private void OnClickMainMenu() => OnMainMenuClicked();
    private void OnResumeClicked()
    {
        ChangeMenuState(MenuName.None);
    }
    private void OnRestartClicked()
    {
        SceneManager.LoadScene("LoadingScreen");
    }


    private void OnMainMenuClicked()
    {
        Dependencies.GameDataOperations.SetSceneToLoadName(SceneName.MainMenu);
        SceneManager.LoadScene("LoadingScreen");
    }
    
}
