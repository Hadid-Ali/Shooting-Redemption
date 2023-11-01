using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GamWin : MonoBehaviour
{
    [SerializeField] private GameObject _screen;


    private void Awake()
    {
        AIGroupsHandler.AllGroupsCCleared.Register(OnGameWon);
    }

    private void OnDestroy()
    {
        AIGroupsHandler.AllGroupsCCleared.Unregister(OnGameWon);
        
    }

    public void OnGameWon()
    {
        StartCoroutine(wait());
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
