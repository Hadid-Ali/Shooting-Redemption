using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EpisodePanel : UIMenuBase
{
    [SerializeField] private List<Button> LevelBtns;
    
    protected override void OnMenuContainerEnable()
    {
        for (int i = 0; i < LevelBtns.Count; i++)
        {
            int j = i;
            LevelBtns[j].interactable = Dependencies.GameDataOperations.GetUnlockedEpisodes(j);
            LevelBtns[j].onClick.AddListener((() => OpenEpisode(j)));
            LevelBtns[j].onClick.AddListener(() => Dependencies.SoundHandler.BtnClickSound(ButtonType.Play));
        }
        GameEvents.GamePlayEvents.mainMenuButtonTap.Register(ButtonsOnClickExecution);
    }

    protected override void OnMenuContainerDisable()
    {
        for (int i = 0; i < LevelBtns.Count; i++)
        {
            int j = i;
            LevelBtns[j].onClick.RemoveAllListeners();
        }
        GameEvents.GamePlayEvents.mainMenuButtonTap.UnRegister(ButtonsOnClickExecution);
    }

    private void ButtonsOnClickExecution(ButtonType type)
    {
        switch (type)    
        {
            case ButtonType.Exit:
                ChangeMenuState(MenuName.MainMenu);
                break;
        }
    }


    public void OpenEpisode(int i)
    {
        Dependencies.GameDataOperations.SetSelectedEpisode(i);
        ChangeMenuState(MenuName.LevelSelection);
    }
    
}
