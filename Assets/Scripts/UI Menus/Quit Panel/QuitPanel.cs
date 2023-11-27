using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;

public class QuitPanel : UIMenuBase
{
    [SerializeField] private Button m_yesBtn;
    [SerializeField] private Button m_NoBtn;

    private void Start()
    {
        m_yesBtn.onClick.AddListener(QuitGame);
        m_NoBtn.onClick.AddListener(BackToMainMenu);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        ChangeMenuState(MenuName.MainMenu);
    }
}
