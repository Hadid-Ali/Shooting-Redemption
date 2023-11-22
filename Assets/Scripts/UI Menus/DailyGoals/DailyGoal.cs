using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyGoal : UIMenuBase
{
    [SerializeField] private Button m_closeTap;
    [SerializeField]private Text CoinTxt;
    [SerializeField]private Text StarsTxt;

    private void Start()
    {
        m_closeTap.onClick.AddListener(OnCloseBtnTap);
        updateCoins();
    }
    public void updateCoins()
    {
        CoinTxt.text = SaveLoadData.GameData.m_Coins.ToString();
        StarsTxt.text = SaveLoadData.GameData.m_Stars.ToString();
        
    }

    void OnCloseBtnTap()
    {
        ChangeMenuState(MenuName.MainMenu);
    }
}
