using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GamWin : MonoBehaviour
{
    [SerializeField] private GameObject _screen;

    [SerializeField] private GameObject[] Levels;

    public int selectedLevel;


    private void Awake()
    {
        AIGroupsHandler.AllGroupsCCleared.Register(OnGameWon);
        LoadLevel(PlayerPrefs.GetInt("SelectedLevel", 0));
        
    }

    private void OnDestroy()
    {
        AIGroupsHandler.AllGroupsCCleared.Unregister(OnGameWon);
        
    }

    public void OnGameWon()
    {
        StartCoroutine(wait());
    }

    public void OnLevelSelect(int i)
    {
        PlayerPrefs.SetInt("SelectedLevel", i);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex) ;
    }
    
    public void LoadLevel(int i)
    {
        Levels[i].SetActive(true);
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1f);
        _screen.SetActive(true);
    }
    
    public void LoadScene()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
