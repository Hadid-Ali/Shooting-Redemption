using System;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;

public class OpenApp : MonoBehaviour
{
    [SerializeField] private string AdId;
    [SerializeField] private bool m_Initialize;
    [SerializeField] private bool m_ShowOnStateChange;

    AppOpenAd m_AppOpenAd;

    public bool IsAdAvailable
    {
        get
        {
            return m_AppOpenAd != null;
        }
    }
    private void Awake()
    {
        // Use the AppStateEventNotifier to listen to application open/close events.
        // This is used to launch the loaded ad when we open the app.
        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
    }

    private void OnDestroy()
    {
        // Always unlisten to events when complete.
        AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
    }
    
    public void LoadAD()
    {
        if (m_AppOpenAd != null)
        {
            m_AppOpenAd.Destroy();
            m_AppOpenAd = null;
        }

        Debug.Log("Loading the app open ad.");

        // Create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        AppOpenAd.Load(AdId, ScreenOrientation.Portrait, adRequest,
            (AppOpenAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("app open ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("App open ad loaded with response : "
                          + ad.GetResponseInfo());

                m_AppOpenAd = ad;
                RegisterEventHandlers(ad);
            });
    }

    public void ShowAppOpenAd()
    {
        if (m_AppOpenAd != null && m_AppOpenAd.CanShowAd())
        {
            Debug.Log("Showing app open ad.");
            m_AppOpenAd.Show();
        }
        else
        {
            Debug.LogError("App open ad is not ready yet.");
        }
    }
    private void OnAppStateChanged(AppState state)
    {
        if (!m_ShowOnStateChange)
        {
            return;
        }
        Debug.Log("App State changed to : "+ state);

        // if the app is Foregrounded and the ad is available, show it.
        if (state == AppState.Foreground)
        {
            if (IsAdAvailable)
            {
                ShowAppOpenAd();
            }
        }
    }
    private void RegisterEventHandlers(AppOpenAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("App open ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () => { Debug.Log("App open ad recorded an impression."); };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () => { Debug.Log("App open ad was clicked."); };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () => { Debug.Log("App open ad full screen content opened."); };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () => { Debug.Log("App open ad full screen content closed."); };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("App open ad failed to open full screen content " +
                           "with error : " + error);
        };
    }
    
    public bool IsInitialized() => m_Initialize;
}
