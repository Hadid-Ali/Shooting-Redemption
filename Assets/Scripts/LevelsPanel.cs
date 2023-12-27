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
            bool unlocked = j <= Dependencies.GameDataOperations.GetUnlockedLevels(Dependencies.GameDataOperations.GetSelectedEpisode());
            buttons[j].interactable = unlocked;
            
            buttons[j].transform.GetChild(0).gameObject.SetActive(!unlocked);
            buttons[j].transform.GetChild(1).gameObject.SetActive(unlocked); //For lock image
            
            buttons[j].onClick.AddListener(()=> SetLevelNum(j));
            buttons[j].onClick.AddListener(()=>Dependencies.SoundHandler.PlaySFXSound(SFX.ButtonClick));
        }
        GameEvents.GamePlayEvents.mainMenuButtonTap.Register(ButtonsOnClickExecution);
        
    }

    protected override void OnMenuContainerDisable()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            int j = i;
            buttons[j].onClick.RemoveAllListeners();
        }
        GameEvents.GamePlayEvents.mainMenuButtonTap.UnRegister(ButtonsOnClickExecution);
    }

    public void SetLevelNum(int i)
    {
        int selectedEpisode = Dependencies.GameDataOperations.GetSelectedEpisode();
        
        if(selectedEpisode == 0)
            SessionData.Instance.sceneToLoad = SceneName.Chapter1;
        if(selectedEpisode == 1)
            SessionData.Instance.sceneToLoad = SceneName.Chapter2;
        if(selectedEpisode == 2)
            SessionData.Instance.sceneToLoad = SceneName.Chapter3;
        if(selectedEpisode == 3)
            SessionData.Instance.sceneToLoad = SceneName.Chapter4;
        if(selectedEpisode == 4)
            SessionData.Instance.sceneToLoad = SceneName.Chapter5;
            
        
        Dependencies.SoundHandler.MuteBgMusic();
        Dependencies.GameDataOperations.SetSelectedLevel(i);
        SceneManager.LoadScene("LoadingScreen");
    }
    
    private void ButtonsOnClickExecution(ButtonType type)
    {
        switch (type)    
        {
            case ButtonType.Exit:
                ChangeMenuState(MenuName.EpisodesSelection);
                break;
        }
    }

}
