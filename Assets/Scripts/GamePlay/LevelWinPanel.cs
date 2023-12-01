using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelWinPanel : UIMenuBase
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button nextButton;
    

    protected override void OnMenuContainerEnable()
    {
        Time.timeScale = 0.001f;

        IncrementProgressLevel();
    }

    protected override void OnMenuContainerDisable()
    {
        Time.timeScale = 1;
    }

    private void Start()
    {
        mainMenuButton.onClick.AddListener(OnClickMainMenu);
        restartButton.onClick.AddListener(OnClickRestart);
        nextButton.onClick.AddListener(OnClickResume);
    }

    private void OnClickResume() => OnNextClicked();
    private void OnClickRestart() => OnRestartClicked();
    private void OnClickMainMenu() => OnMainMenuClicked();
    private void OnNextClicked()
    {
        int currentLevel = Dependencies.GameDataOperations.GetSelectedLevel();
        currentLevel++;
        Dependencies.GameDataOperations.SetSelectedLevel(currentLevel);
        ChangeMenuState(MenuName.None);
    }
    private void OnRestartClicked()
    {
        SceneManager.LoadScene("LoadingScreen");
    }
    public void IncrementProgressLevel()
    {
        int currentLevel = Dependencies.GameDataOperations.GetSelectedLevel();
        int currentEpisode = Dependencies.GameDataOperations.GetSelectedEpisode();
        
        if (currentLevel < 4)
        {
            Dependencies.GameDataOperations.SetUnlockedLevels(currentEpisode, currentLevel);
        }
        if (currentLevel >= 4)
        {
            if (!Dependencies.GameDataOperations.GetUnlockedEpisodes(currentEpisode))
            {
                currentEpisode++;
                Dependencies.GameDataOperations.SetUnlockedEpisodes(currentEpisode);
            }
        }
    }
    
    private void OnMainMenuClicked()
    {
        Dependencies.GameDataOperations.SetSceneToLoadName(SceneName.MainMenu);
        SceneManager.LoadScene("LoadingScreen");
    }
    
}
