using System;

public static class AdHandler
{
    public static void InitializeAds()
    {
            GameEvents.InitAds.Raise();
    }

    public static void ShowBanner()
    {
            GameEvents.ShowBannerAd.Raise();
    }

    public static void ShowRectBanner()
    {
            GameEvents.ShowRectBannerAd.Raise();
    }

    public static void HideBanner()
    {
            GameEvents.HideBannerAd.Raise();
    }

    public static void HideRectBanner()
    {
            GameEvents.HideRectBannerAd.Raise();
    }

    public static void ShowInterstitial()
    {
            GameEvents.ShowRInterstitialAd.Raise();
    }

    public static void ShowRewarded(Action reward)
    {
            GameEvents.ShowRewardedAd.Raise(reward);
    }
    public static void ShowAppOpen()
    {
            GameEvents.ShowAppOpenAd.Raise();
    }
}
