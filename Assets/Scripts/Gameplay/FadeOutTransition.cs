using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class FadeOutTransition : BasePanelTransition
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeTime = 0.3f;
    public override void Transition(Action callback)
    {
        canvasGroup.DOFade(0, fadeTime).OnComplete(() => callback?.Invoke());
    }
}
