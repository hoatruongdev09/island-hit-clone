using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FadeInTransition : BasePanelTransition
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeTime = 0.3f;
    public override void Transition(Action callback)
    {
        canvasGroup.DOFade(1, fadeTime).OnComplete(() => callback?.Invoke());
    }
}
