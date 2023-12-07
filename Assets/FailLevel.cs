using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FailLevel : UIMenuBase
{
    [SerializeField] private Button mainMenubtn;
    [SerializeField] private Button RestartScenebtn;
    private void Start()
    {
        mainMenubtn.onClick.AddListener(OnLoadScene);
        RestartScenebtn.onClick.AddListener(OnRestartScene);
    }

    private void OnEnable()
    {
        GameAdEvents.MenuEvents.LevelFail.Register(OnLevelFail);
    }

    private void OnDisable()
    {
        GameAdEvents.MenuEvents.LevelFail.Unregister(OnLevelFail);
    }

    public void OnLevelFail()
    {
        ChangeMenuState(MenuName.GameplayLevelFailed);
    }
    

    public void OnRestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    private void OnLoadScene()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
