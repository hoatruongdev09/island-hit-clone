using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdServerController : MonoBehaviour
{
    public static AdServerController Instance { get; private set; }

    private InterstitialAd interstitial;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void OnDestroy()
    {
        Instance = null;
    }


    public void Start()
    {
        MobileAds.Initialize((initStatus) =>
        {
            RequestInterstitial();
        });
    }

    public void ShowInterstitialAd()
    {
        if (interstitial == null)
        {
            RequestInterstitial();
            return;
        }
        if (!interstitial.IsLoaded())
        {
            Debug.LogError("interstitial ad is not loaded");
            LoadInterstitialAd();
        }
        else
        {
            interstitial.Show();
        }
    }
    private void LoadInterstitialAd()
    {
        var request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);
    }
    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-1102815452468328/2649626441";
#elif UNITY_IOS || UNITY_IPHONE
        string adUnitId = "ca-app-pub-1102815452468328/1165455704";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        if (interstitial != null)
        {
            interstitial.OnAdClosed -= OnInterstitialClose;
            interstitial.OnAdFailedToLoad -= OnInterstitialFailedToLoad;
            interstitial.OnAdFailedToShow -= OnInterstitialAdFailedToShow;
            interstitial.Destroy();
            interstitial = null;
        }
        this.interstitial = new InterstitialAd(adUnitId);
        interstitial.OnAdClosed += OnInterstitialClose;
        interstitial.OnAdFailedToLoad += OnInterstitialFailedToLoad;
        interstitial.OnAdFailedToShow += OnInterstitialAdFailedToShow;
        LoadInterstitialAd();
    }

    private void OnInterstitialClose(object sender, EventArgs e)
    {
        Debug.Log($"interstitial close");
        LoadInterstitialAd();
    }

    private void OnInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        Debug.LogError($"interstitial ad failed to load: {e.LoadAdError.GetMessage()}");
        RequestInterstitial();
    }

    private void OnInterstitialAdFailedToShow(object sender, AdErrorEventArgs e)
    {
        Debug.LogError($"interstitial ad failed to show: {e.AdError.GetMessage()}");
        LoadInterstitialAd();
    }
}
