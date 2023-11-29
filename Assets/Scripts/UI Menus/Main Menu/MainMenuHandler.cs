using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuHandler : UIMenuBase
{
    [SerializeField] private Button m_PlayButon;
    [SerializeField] private Button m_DailyGoal;
    [SerializeField] private Button m_SettingPanel;
    [SerializeField] private Button m_QuitPanel;
    [SerializeField] private Button m_CharacterPanel;
    [SerializeField] private Button m_GunPanel;
    [SerializeField] private Button m_EpisodeSelectionPanel;
    [SerializeField] private Button m_AddCoins;

    [SerializeField] private TextMeshProUGUI CoinTxt;
    
    private void Start()
    {
        Initialize();
        updateCoins();
    }
    
    public void updateCoins()
    {
        CoinTxt.text = Dependencies.GameDataOperations.GetCredits().ToString();
    }

    private void Initialize()
    {
        m_PlayButon.onClick.AddListener(OnPlayBtnTap);
        m_DailyGoal.onClick.AddListener(OnDailyBtnTap);
        m_SettingPanel.onClick.AddListener(OnSettingBtnTap);
        m_QuitPanel.onClick.AddListener(OnQuitBtnTap);
        m_CharacterPanel.onClick.AddListener(OnCharacterBtnTap);
        m_GunPanel.onClick.AddListener(OnGunBtnTap);
        m_EpisodeSelectionPanel.onClick.AddListener(OnEpisodeSelectionBtnTap);
        m_AddCoins.onClick.AddListener(OnClickAddcoin);
    }

    private void OnClickAddcoin()
    {
        AdHandler.ShowRewarded(OnRewardedAddCoins);
    }

    private void OnRewardedAddCoins()
    {
        Dependencies.GameDataOperations.SetCredit(Dependencies.GameDataOperations.GetCredits() + 300);
        updateCoins();
    }

    private void OnPlayBtnTap()
    {
        ChangeMenuState(MenuName.EpisodeSelection);
    }
    
    private void OnDailyBtnTap()
    {
        ChangeMenuState(MenuName.DailyGoals);
    }

    private void OnSettingBtnTap()
    {
        ChangeMenuState(MenuName.SettingsMenu);
    }
    
    private void OnQuitBtnTap()
    {
        ChangeMenuState(MenuName.QuitPanel);
    }
    
    private void OnCharacterBtnTap()
    {
        ChangeMenuState(MenuName.CharacterShop);
    }
    
    private void OnGunBtnTap()
    {
        ChangeMenuState(MenuName.GunShop);
    }
    
    private void OnEpisodeSelectionBtnTap()
    {
        ChangeMenuState(MenuName.EpisodeSelection);
    }
    
}
