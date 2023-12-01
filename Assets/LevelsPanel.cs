using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsPanel : UIMenuBase
{
    public List<Button> buttons;
    
    protected override void OnMenuContainerEnable()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            int j = i;
            buttons[j].interactable = j <= Dependencies.GameDataOperations.GetUnlockedLevels(Dependencies.GameDataOperations.GetSelectedEpisode());
            buttons[j].onClick.AddListener(()=> SetLevelNum(j));
        }
        
    }

    protected override void OnMenuContainerDisable()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            int j = i;
            buttons[j].onClick.RemoveAllListeners();
        }
    }

    public void SetLevelNum(int i)
    {
        int selectedEpisode = Dependencies.GameDataOperations.GetSelectedEpisode();
        
        if(selectedEpisode == 0)
            Dependencies.GameDataOperations.SetSceneToLoadName(SceneName.Chapter1);
        if(selectedEpisode == 1)
            Dependencies.GameDataOperations.SetSceneToLoadName(SceneName.Chapter2);
        if(selectedEpisode == 2)
            Dependencies.GameDataOperations.SetSceneToLoadName(SceneName.Chapter3);
        
        Dependencies.GameDataOperations.SetSelectedLevel(i);

        SceneManager.LoadScene("LoadingScreen");
    }

    public void OnCloseButtonClicked()
    {
        ChangeMenuState(MenuName.EpisodeSelection);
    }
}
