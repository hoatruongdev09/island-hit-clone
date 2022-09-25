using System;
using System.Collections;
using System.Collections.Generic;
using deVoid.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PanelMain : BasePanel
{
    [SerializeField] private Button buttonPlay;


    private void Awake()
    {
        buttonPlay.onClick.AddListener(OnPlayClicked);
    }
    private void OnDestroy()
    {
        buttonPlay.onClick.RemoveListener(OnPlayClicked);
    }

    private void OnPlayClicked()
    {
        Signals.Get<GameAPI.OnStartPlay>().Dispatch();
    }
}
