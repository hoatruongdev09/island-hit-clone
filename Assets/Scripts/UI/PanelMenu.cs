using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using System;
using deVoid.Utils;

public class PanelMenu : BasePanel
{
    [SerializeField] private TMP_Text textTitle;
    [SerializeField] private TMP_Text textButtonBack;
    [SerializeField] private TMP_Text textButtonLeaderBoard;
    [SerializeField] private TMP_Text textButtonSettings;
    [SerializeField] private RectTransform settingsHolder;
    [SerializeField] private CanvasGroup hiddenSettingsGroup;
    [SerializeField] private RectTransform dynamicIsland;
    [SerializeField] private Button buttonLeaderBoard;
    [SerializeField] private Button buttonSettings;
    [SerializeField] private Button buttonRemoveAds;
    [SerializeField] private Button buttonBack;
    [SerializeField] private CanvasGroup contentGroup;

    private bool isInSettings = false;
    private void Awake()
    {
        buttonLeaderBoard.onClick.AddListener(OnLeaderboardClicked);
        buttonSettings.onClick.AddListener(OnSettingsClicked);
        buttonRemoveAds.onClick.AddListener(OnRemoveAdsClicked);
        buttonBack.onClick.AddListener(OnBackClicked);
    }
    private void OnDestroy()
    {
        buttonLeaderBoard.onClick.RemoveListener(OnLeaderboardClicked);
        buttonSettings.onClick.RemoveListener(OnSettingsClicked);
        buttonRemoveAds.onClick.RemoveListener(OnRemoveAdsClicked);
        buttonBack.onClick.RemoveListener(OnBackClicked);
    }

    public override void Show(Action callback = null)
    {
        base.Show(() =>
        {
            callback?.Invoke();
            contentGroup.DOFade(1, 0.3f);
        });
    }
    public override void Hide(Action callback = null)
    {
        textTitle.gameObject.SetActive(false);
        contentGroup.DOFade(0, 0.3f);
        dynamicIsland.DOSizeDelta(new Vector2(250, 400), 0.3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            callback?.Invoke();
            ResetPanel();
        });

    }
    private void ResetPanel()
    {
        contentGroup.alpha = 0;
        textTitle.gameObject.SetActive(true);
        dynamicIsland.sizeDelta = new Vector2(200, 200);
    }

    private void OnMenuClicked()
    {
        isInSettings = false;
        textTitle.DOFade(0, .15f).OnComplete(() =>
        {
            textTitle.text = "menu";
            textTitle.DOFade(1, .15f).SetDelay(0.01f);
        });
    }

    private void OnShareClicked()
    {

    }

    private void OnLeaderboardClicked()
    {
        if (isInSettings)
        {
            AccountData.useSfx = !AccountData.useSfx;
            Signals.Get<GameAPI.OnChangeUseSfx>().Dispatch(AccountData.useSfx);
            textButtonLeaderBoard.DOFade(0, .15f).OnComplete(() =>
            {
                textButtonLeaderBoard.text = $"sfx {(AccountData.useSfx ? "on" : "off")}";
                textButtonLeaderBoard.DOFade(1, .15f).SetDelay(0.01f);
            });
            return;
        }
    }

    private void OnSettingsClicked()
    {
        if (isInSettings)
        {
            AccountData.useHaptic = !AccountData.useHaptic;
            Signals.Get<GameAPI.OnChangeUseHaptic>().Dispatch(AccountData.useHaptic);
            textButtonSettings.DOFade(0, .15f).OnComplete(() =>
            {
                textButtonSettings.text = $"haptic {(AccountData.useHaptic ? "on" : "off")}";
                textButtonSettings.DOFade(1, .15f).SetDelay(0.01f);
            });
            return;
        }
        isInSettings = true;
        textTitle.DOFade(0, .15f).OnComplete(() =>
        {
            textTitle.text = "settings";
            textTitle.DOFade(1, .15f).SetDelay(0.01f);
        });
        textButtonBack.DOFade(0, .15f).OnComplete(() =>
        {
            textButtonBack.text = "menu";
            textButtonBack.DOFade(1, .15f).SetDelay(0.01f);
        });
        textButtonLeaderBoard.DOFade(0, .15f).OnComplete(() =>
        {
            textButtonLeaderBoard.text = $"sfx {(AccountData.useSfx ? "on" : "off")}";
            textButtonLeaderBoard.DOFade(1, .15f).SetDelay(0.01f);
        });
        textButtonSettings.DOFade(0, .15f).OnComplete(() =>
        {
            textButtonSettings.text = $"haptic {(AccountData.useHaptic ? "on" : "off")}";
            textButtonSettings.DOFade(1, .15f).SetDelay(0.01f);
        });
        settingsHolder.DOAnchorPosY(100, 0.31f).SetEase(Ease.OutBack).OnComplete(() =>
        {

        });
        // hiddenSettingsGroup.gameObject.SetActive(true);
        // hiddenSettingsGroup.DOFade(1, .3f).SetDelay(0.1f);
    }

    private void OnRemoveAdsClicked()
    {

    }

    private void OnBackClicked()
    {
        if (isInSettings)
        {
            isInSettings = false;
            textTitle.DOFade(0, .15f).OnComplete(() =>
            {
                textTitle.text = "menu";
                textTitle.DOFade(1, .15f).SetDelay(0.01f);
            });
            textButtonBack.DOFade(0, .15f).OnComplete(() =>
            {
                textButtonBack.text = "back";
                textButtonBack.DOFade(1, .15f).SetDelay(0.01f);
            });
            textButtonLeaderBoard.DOFade(0, .15f).OnComplete(() =>
            {
                textButtonLeaderBoard.text = $"leaderboard";
                textButtonLeaderBoard.DOFade(1, .15f).SetDelay(0.01f);
            });
            textButtonSettings.DOFade(0, .15f).OnComplete(() =>
            {
                textButtonSettings.text = $"settings";
                textButtonSettings.DOFade(1, .15f).SetDelay(0.01f);
            });
            settingsHolder.DOAnchorPosY(37, 0.31f).SetEase(Ease.OutBack).OnComplete(() =>
            {

            });
            // hiddenSettingsGroup.DOFade(0, .1f).OnComplete(() =>
            // {
            //     hiddenSettingsGroup.gameObject.SetActive(false);
            // });
        }
        else
        {
            Signals.Get<GameAPI.MenuToGameOver>().Dispatch();
        }
    }
}
