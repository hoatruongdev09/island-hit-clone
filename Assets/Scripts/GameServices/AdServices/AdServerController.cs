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
        if (interstitial == null) { return; }
        if (!interstitial.IsLoaded())
        {
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
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);
        LoadInterstitialAd();
    }
}
