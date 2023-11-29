using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShop : UIMenuBase
{
    [SerializeField] private Button m_CloseBtnTap;
    [SerializeField]private TextMeshProUGUI CoinTxt;
    
    public void Initailize()
    {
        m_CloseBtnTap.onClick.AddListener(onCloseButtonTap);
        updateCoins();
    }

    private void Start()
    {
        Initailize();
    }

    public void onCloseButtonTap()
    {
        ChangeMenuState(MenuName.MainMenu);
    }
    
    public void updateCoins()
    {
        if (CoinTxt != null)
        {
            CoinTxt.SetText(Dependencies.GameDataOperations.GetCredits().ToString());
        }
    }
    
}
