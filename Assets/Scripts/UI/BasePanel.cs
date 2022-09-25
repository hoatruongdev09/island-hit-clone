using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    [SerializeField] private BasePanelTransition transitionIn;
    [SerializeField] private BasePanelTransition transitionOut;

    public virtual void Show(Action callback = null)
    {
        gameObject.SetActive(true);
        if (transitionIn)
        {
            transitionIn?.Transition(callback);
        }
        else
        {
            callback?.Invoke();
        }
    }
    public virtual void Hide(Action callback = null)
    {
        if (transitionOut)
        {
            transitionOut?.Transition(() =>
            {
                gameObject.SetActive(false);
                callback?.Invoke();
            });
        }
        else
        {
            gameObject.SetActive(false);
            callback?.Invoke();
        }
    }
}
