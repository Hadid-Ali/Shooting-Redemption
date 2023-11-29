using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsPanel : UIMenuBase
{
    public void OnCloseButtonClicked()
    {
        ChangeMenuState(MenuName.EpisodeSelection);
    }
}
