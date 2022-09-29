using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ZoomButtonEffect : BaseButtonEffect
{
    private bool isPointerDown;
    private bool isPointerEnter;
    private Tween tweenZoom;

    [SerializeField] private RectTransform rectTransform;
    private void Start()
    {
        if (!rectTransform)
        {
            rectTransform = transform as RectTransform;
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        if (tweenZoom != null && tweenZoom.IsPlaying())
        {
            tweenZoom.Kill();
        }
        tweenZoom = rectTransform.DOScale(Vector3.one * 1.2f, .1f).SetEase(Ease.OutBack);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        if (tweenZoom != null && tweenZoom.IsPlaying())
        {
            tweenZoom.Kill();
        }
        tweenZoom = rectTransform.DOScale(Vector3.one, .15f).SetEase(Ease.OutBack); ;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        isPointerEnter = true;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        isPointerEnter = false;
    }

}
