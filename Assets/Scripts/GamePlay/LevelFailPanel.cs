using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelFailPanel : UIMenuBase
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button restartButton;

    
    private void Awake()
    {
        GameEvents.GamePlayEvents.OnPlayerDead.Register(GameLost);
    }

    private void OnDestroy()
    {
        GameEvents.GamePlayEvents.OnPlayerDead.Unregister(GameLost);
    }

    private void GameLost()
    {
        ChangeMenuState(MenuName.GameplayLevelFailed);
    }
    protected override void OnMenuContainerEnable()
    {
        AdHandler.ShowInterstitial();
        
        Time.timeScale = 0.001f;
    }

    protected override void OnMenuContainerDisable()
    {
        Time.timeScale = 1f;
    }

    private void Start()
    {
        mainMenuButton.onClick.AddListener(OnClickMainMenu);
        restartButton.onClick.AddListener(OnClickRestart);
    }
    private void OnClickRestart() => OnRestartClicked();
    private void OnClickMainMenu() => OnMainMenuClicked();

    private void OnRestartClicked()
    {
        SceneManager.LoadScene("LoadingScreen");
    }
    
    private void OnMainMenuClicked()
    {
        SessionData.Instance.sceneToLoad = SceneName.MainMenu;
        SceneManager.LoadScene("LoadingScreen");
    }
}
