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
    [SerializeField] private CanvasGroup contentGroup;

    private bool buttonReplayClicked;
    private bool buttonShareClicked;
    private bool buttonMenuClicked;

    private bool isBackFromMenu;

    private Tween tweenButtonReplay;
    private void Awake()
    {
        buttonReplay.onClick.AddListener(OnReplayClicked);
        buttonShare.onClick.AddListener(OnButtonShareClicked);
        buttonMenu.onClick.AddListener(OnButtonMenuClicked);

        Signals.Get<GameAPI.MenuToGameOver>().AddListener(OnMenuToGameOver);
    }
    private void OnDestroy()
    {
        buttonReplay.onClick.RemoveListener(OnReplayClicked);
        buttonShare.onClick.RemoveListener(OnButtonShareClicked);
        buttonMenu.onClick.RemoveListener(OnButtonMenuClicked);
        Signals.Get<GameAPI.MenuToGameOver>().RemoveListener(OnMenuToGameOver);
    }

    private void OnMenuToGameOver()
    {
        isBackFromMenu = true;
    }

    public override void Show(Action callback = null)
    {
        if (isBackFromMenu)
        {
            gameObject.SetActive(true);
            isBackFromMenu = false;
            callback?.Invoke();
            dynamicIslandTransform.sizeDelta = new Vector2(250, 400);
            textNewHighScore.gameObject.SetActive(AccountData.isBestScore);
            textScore.text = AccountData.lastGameScore.ToString();
            textBestScore.text = AccountData.bestGameScore.ToString();
            (buttonReplay.transform as RectTransform).DOAnchorPosX(0f, 0);
            (buttonMenu.transform as RectTransform).DOAnchorPosX(0f, 0);
            (buttonShare.transform as RectTransform).DOAnchorPosX(0f, 0);
            scoreTextHolder.gameObject.SetActive(true);
            AnimateButtonReplayGround(0.5f, 0f);
            contentGroup.DOFade(1, 0.3f);
        }
        else
        {

            base.Show(() =>
            {
                AnimateExpandDynamicIsland(0.6f);
                AnimateButtonReplay(0.3f, 0.3f);
                AnimateButtonReplayGround(0.5f, 0.6f);
                AnimateButtonShare(0.3f, 0.6f);
                AnimateButtonMenu(0.3f, 0.9f);
                callback?.Invoke();
            });
        }
        textNewHighScore.gameObject.SetActive(AccountData.isBestScore);
        textScore.text = AccountData.lastGameScore.ToString();
        textBestScore.text = AccountData.bestGameScore.ToString();
    }
    public override void Hide(Action callback = null)
    {
        if (buttonReplayClicked)
        {

            contentGroup.DOFade(0, 0.3f);
            AnimateMinimizeReplayDynamicIsland(.3f, callback: () =>
            {
                base.Hide(() =>
                {
                    callback?.Invoke();
                    ResetPanel();
                });
            });
        }
        else if (buttonMenuClicked)
        {
            contentGroup.DOFade(0, 0.3f);
            AnimateMinimizeMenu(.3f, callback: () =>
            {
                gameObject.SetActive(false);
                ResetPanel();
                callback?.Invoke();
                // base.Hide(() =>
                // {
                //     callback?.Invoke();
                //     ResetPanel();
                // });
            });
        }
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
        Signals.Get<GameAPI.OnGoToMenu>().Dispatch();
    }

    private void AnimateExpandDynamicIsland(float time = 0.6f, float delay = 0)
    {
        dynamicIslandTransform.DOSizeDelta(new Vector2(250, 400), 0.6f).SetDelay(delay).SetEase(Ease.OutBack).OnComplete(() =>
        {
            scoreTextHolder.gameObject.SetActive(true);
        });
    }
    private void AnimateMinimizeReplayDynamicIsland(float time = 0.6f, float delay = 0, Action callback = null)
    {
        scoreTextHolder.gameObject.SetActive(false);
        dynamicIslandTransform.DOSizeDelta(new Vector2(122, 36), 0.6f).SetDelay(delay).OnComplete(() =>
              {
                  callback?.Invoke();
              });
    }
    private void AnimateMinimizeMenu(float time = 0.6f, float delay = 0, Action callback = null)
    {
        scoreTextHolder.gameObject.SetActive(false);
        dynamicIslandTransform.DOSizeDelta(new Vector2(200, 200), 0.6f).SetDelay(delay).OnComplete(() =>
              {
                  callback?.Invoke();
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
    private void AnimateButtonReplayGround(float time = .3f, float delay = .6f)
    {
        buttonPlayBackgroundHolder.DOScale(Vector3.one, time).SetDelay(delay).SetEase(Ease.OutBack).OnComplete(() =>
        {
            tweenButtonReplay = buttonPlayBackgroundHolder
                .DOScale(new Vector3(1.05f, 1.05f, 1.05f), 1f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    buttonPlayBackgroundHolder.localScale = Vector3.zero;
                });
        });
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
        contentGroup.alpha = 1;

        if (tweenButtonReplay != null && tweenButtonReplay.IsPlaying())
        {
            tweenButtonReplay.Kill(true);
        }
    }
}
