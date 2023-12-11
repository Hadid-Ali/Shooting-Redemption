using System;

public static class AdHandler
{
    public static void InitializeAds()
    {
           // GameAdEvents.InitAds.Raise();
    }

    public static void ShowBanner()
    {
           // GameAdEvents.ShowBannerAd.Raise();
    }

    public static void ShowRectBanner()
    {
           // GameAdEvents.ShowRectBannerAd.Raise();
    }

    public static void HideBanner()
    {
           // GameAdEvents.HideBannerAd.Raise();
    }

    public static void HideRectBanner()
    {
            //GameAdEvents.HideRectBannerAd.Raise();
    }

    public static void ShowInterstitial()
    {
           // GameAdEvents.ShowRInterstitialAd.Raise();
    }

    public static void ShowRewarded(Action reward)
    {
           // GameAdEvents.ShowRewardedAd.Raise(reward);
    }
    public static void ShowAppOpen()
    {
           // GameAdEvents.ShowAppOpenAd.Raise();
    }
}
