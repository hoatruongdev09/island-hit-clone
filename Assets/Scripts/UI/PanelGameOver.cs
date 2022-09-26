using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System;
using deVoid.Utils;

public class PanelGameOver : BasePanel
{
    [SerializeField] private TMP_Text textNewHighScore;
    [SerializeField] private TMP_Text textScore;
    [SerializeField] private TMP_Text textBestScore;

    [SerializeField] private Button buttonReplay;
    [SerializeField] private Button buttonShare;
    [SerializeField] private Button buttonMenu;

    [SerializeField] private RectTransform dynamicIslandTransform;
    [SerializeField] private GameObject scoreTextHolder;
    [SerializeField] private Transform buttonPlayBackgroundHolder;

    private bool buttonReplayClicked;
    private bool buttonShareClicked;
    private bool buttonMenuClicked;

    private void Awake()
    {
        buttonReplay.onClick.AddListener(OnReplayClicked);
        buttonShare.onClick.AddListener(OnButtonShareClicked);
        buttonMenu.onClick.AddListener(OnButtonMenuClicked);
    }
    private void OnDestroy()
    {
        buttonReplay.onClick.RemoveListener(OnReplayClicked);
        buttonShare.onClick.RemoveListener(OnButtonShareClicked);
        buttonMenu.onClick.RemoveListener(OnButtonMenuClicked);
    }


    public override void Show(Action callback = null)
    {
        base.Show(() =>
        {
            AnimateDynamicIsland(0.6f);
            AnimateButtonReplay(0.3f, 0.6f);
            AnimateButtonPlayGround(0.3f, 1.5f);
            AnimateButtonShare(0.3f, 1.2f);
            AnimateButtonMenu(0.3f, 1.5f);
            callback?.Invoke();
        });
        textNewHighScore.gameObject.SetActive(AccountData.isBestScore);
        textScore.text = AccountData.lastGameScore.ToString();
        textBestScore.text = AccountData.bestGameScore.ToString();
    }
    public override void Hide(Action callback = null)
    {
        base.Hide(() =>
        {
            callback?.Invoke();
            ResetPanel();
        });
    }


    private void OnReplayClicked()
    {
        if (buttonReplayClicked) { return; }
        buttonReplayClicked = true;
        Signals.Get<GameAPI.OnReplay>().Dispatch();
    }

    private void OnButtonShareClicked()
    {
        if (buttonShareClicked) { return; }
        buttonShareClicked = true;
    }

    private void OnButtonMenuClicked()
    {
        if (buttonMenuClicked) { return; }
        buttonMenuClicked = true;
    }

    private void AnimateDynamicIsland(float time = 0.6f, float delay = 0)
    {
        dynamicIslandTransform.DOSizeDelta(new Vector2(250, 400), 0.6f).SetDelay(delay).SetEase(Ease.OutBack).OnComplete(() =>
        {
            scoreTextHolder.gameObject.SetActive(true);
        });
    }
    private void AnimateButtonReplay(float time = 0.3f, float delay = 0.6f)
    {
        (buttonReplay.transform as RectTransform).DOAnchorPosX(0, time).SetDelay(delay);
    }
    private void AnimateButtonShare(float time = 0.3f, float delay = 0.6f)
    {
        (buttonShare.transform as RectTransform).DOAnchorPosX(0, time).SetDelay(delay);
    }
    private void AnimateButtonMenu(float time = 0.3f, float delay = 0.6f)
    {
        (buttonMenu.transform as RectTransform).DOAnchorPosX(0, time).SetDelay(delay);
    }
    private void AnimateButtonPlayGround(float time = .3f, float delay = .6f)
    {
        buttonPlayBackgroundHolder.DOScale(Vector3.one, time).SetDelay(delay).SetEase(Ease.OutBack);
    }

    private void ResetPanel()
    {
        dynamicIslandTransform.sizeDelta = new Vector2(122, 36);
        scoreTextHolder.gameObject.SetActive(false);
        buttonPlayBackgroundHolder.localScale = Vector3.zero;

        var rectTransform = (buttonReplay.transform as RectTransform);
        var anchorPoint = rectTransform.anchoredPosition;

        anchorPoint.x = 1000f;
        rectTransform.anchoredPosition = anchorPoint;

        rectTransform = (buttonShare.transform as RectTransform);
        anchorPoint = rectTransform.anchoredPosition;
        anchorPoint.x = 1000f;
        rectTransform.anchoredPosition = anchorPoint;

        rectTransform = (buttonMenu.transform as RectTransform);
        anchorPoint = rectTransform.anchoredPosition;
        anchorPoint.x = 1000f;
        rectTransform.anchoredPosition = anchorPoint;

        buttonReplayClicked = buttonShareClicked = buttonMenuClicked = false;
    }
}
