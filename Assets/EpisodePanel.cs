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
        }
    }

    protected override void OnMenuContainerDisable()
    {
        for (int i = 0; i < LevelBtns.Count; i++)
        {
            int j = i;
            LevelBtns[j].onClick.RemoveAllListeners();
        }
    }

    public void OpenEpisode(int i)
    {
        print(Dependencies.GameDataOperations.GetSelectedEpisode());
        Dependencies.GameDataOperations.SetSelectedEpisode(i);
        ChangeMenuState(MenuName.LevelSelection);
    }

    public void OnCloseButton()
    {
        ChangeMenuState(MenuName.MainMenu);
    }
}
