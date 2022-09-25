using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PanelGame panelGame;
    [SerializeField] private PanelMain panelMain;

    private void Awake()
    {
        Signals.Get<GameAPI.OnStartPlay>().AddListener(OnStartPlay);
    }
    private void OnDestroy()
    {
        Signals.Get<GameAPI.OnStartPlay>().RemoveListener(OnStartPlay);
    }

    private void OnStartPlay()
    {
        panelMain.Hide(() =>
        {
            panelGame.Show();
        });
    }
}
