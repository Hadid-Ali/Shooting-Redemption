using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePlayPause : UIMenuBase
{
    [SerializeField] private Button m_Continue;
    [SerializeField] private Button m_MainMenu;
    [SerializeField] private Button m_RestartScene;

    private bool isPaused;


    private void Start()
    {
        m_Continue.onClick.AddListener(OnContinueBtnTap);
        m_MainMenu.onClick.AddListener(OnMainMenuBtnTap);
        m_RestartScene.onClick.AddListener(OnRestartScene);
    }

    public void OnContinueBtnTap()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }
    
    public void OnMainMenuBtnTap()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
    
    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
    }
    
    public void OnRestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
