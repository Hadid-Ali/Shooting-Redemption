using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class AdExamples : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button m_InitAds;
    [SerializeField] private Button m_ShowBanner;
    [SerializeField] private Button m_HideBanner;
    [SerializeField] private Button m_ShowRectBanner;
    [SerializeField] private Button m_HideRectBanner;
    [SerializeField] private Button m_ShowInterstitial;
    [SerializeField] private Button m_ShowAppOpen;
    [SerializeField] private Button m_ShowRewarded;

    private void Start()
    {
        m_InitAds.onClick.AddListener(InitAds);
        m_ShowBanner.onClick.AddListener(ShowBanner);
        m_HideBanner.onClick.AddListener(HideBanner);
        m_ShowRectBanner.onClick.AddListener(ShowRectBanner);
        m_HideRectBanner.onClick.AddListener(HideRectBanner);
        m_ShowInterstitial.onClick.AddListener(ShowInterstitial);
        m_ShowAppOpen.onClick.AddListener(ShowAppOpen);
        m_ShowRewarded.onClick.AddListener(ShowRewarded);
    }

    void InitAds() => AdHandler.InitializeAds();
    void ShowBanner() => AdHandler.ShowBanner();
    void HideBanner() => AdHandler.HideBanner();
    void ShowRectBanner() => AdHandler.ShowRectBanner();
    void HideRectBanner() => AdHandler.HideRectBanner();
    void ShowInterstitial() => AdHandler.ShowInterstitial();
    void ShowAppOpen() => AdHandler.ShowAppOpen();
    void ShowRewarded() => AdHandler.ShowRewarded(Reward);

    void Reward()
    {
        
    }

}
