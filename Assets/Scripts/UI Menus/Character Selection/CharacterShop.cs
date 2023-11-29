using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CharacterShop : UIMenuBase
{
    [SerializeField] private Button m_CloseBtn;
    [SerializeField]private TextMeshProUGUI CoinTxt;

    private void Start()
    {
        m_CloseBtn.onClick.AddListener(OnCloseBtnTap);
        
        updateCoins();
    }

    public void updateCoins()
    {
        CoinTxt.SetText(Dependencies.GameDataOperations.GetCredits().ToString()); 
    }
    public void OnCloseBtnTap()
    {
        ChangeMenuState(MenuName.MainMenu);
    }
}
