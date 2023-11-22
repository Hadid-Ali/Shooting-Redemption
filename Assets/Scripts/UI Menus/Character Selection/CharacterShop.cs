using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterShop : UIMenuBase
{
    [SerializeField] private Button m_CloseBtn;
    [SerializeField]private Text CoinTxt;

    private void Start()
    {
        m_CloseBtn.onClick.AddListener(OnCloseBtnTap);
        
        updateCoins();
    }

    public void updateCoins()
    {
        CoinTxt.text = SaveLoadData.GameData.m_Coins.ToString();
    }
    public void OnCloseBtnTap()
    {
        ChangeMenuState(MenuName.MainMenu);
    }
}
