using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Analytics;
using Gley.MobileAds;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    private int _currency = 0;
    public int currency
    {
        get { return _currency; }
        set
        {
            _currency = value;
            rewardText.SetText($"currency: {_currency}");
        }
    }
    public TextMeshProUGUI rewardText;

    void Start()
    {
        API.Initialize();
        rewardText.SetText($"currency: {currency}");
        if (API.IsInitialized()) API.ShowBanner(BannerPosition.Bottom, BannerType.Banner);
        else StartCoroutine(WaitAPI());
    }

    IEnumerator WaitAPI()
    {
        while (!API.IsInitialized())
        {
            yield return new WaitForEndOfFrame();
        }
        API.ShowBanner(BannerPosition.Bottom, BannerType.Banner);
    }

    public void ShowInterstitialAds()
    {
        if (API.IsInterstitialAvailable()) API.ShowInterstitial();
    }

    public void ShowRewardedAds()
    {
        if (API.IsRewardedVideoAvailable()) API.ShowRewardedVideo(OnGetReward);
    }

    private void OnGetReward(bool complete)
    {
        if (complete)
        {
            currency += 10;
            FirebaseManager.I.LogEvent("rewarded_ads", new Firebase.Analytics.Parameter[]
            {
                new (FirebaseAnalytics.ParameterItemID, "koin")
            });
        }
        else print("skipped");
    }

    public void HideBanner()
    {
        API.HideBanner();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus) API.ShowAppOpen();
    }

}
