using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EpisodePanel : UIMenuBase
{
    public void OnCloseButton()
    {
        ChangeMenuState(MenuName.MainMenu);
    }
}
