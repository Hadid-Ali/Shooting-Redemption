using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameAdEvents
{
    public static GameEvent InitAds = new();
    public static GameEvent ShowBannerAd = new();
    public static GameEvent ShowRectBannerAd = new();
    public static GameEvent HideBannerAd = new();
    public static GameEvent HideRectBannerAd = new();
    public static GameEvent ShowRInterstitialAd = new();
    public static GameEvent ShowAppOpenAd = new();
    public static GameEvent<Action> ShowRewardedAd = new();

    public static GameEvent OnAdsInitialized = new();
}
