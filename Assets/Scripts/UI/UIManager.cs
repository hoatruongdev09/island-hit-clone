using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PanelGame panelGame;
    [SerializeField] private PanelMain panelMain;
    [SerializeField] private PanelGameOver panelGameOver;
    private void Awake()
    {
        Signals.Get<GameAPI.OnStartPlay>().AddListener(OnStartPlay);
        Signals.Get<GameAPI.OnReplay>().AddListener(OnReplay);
        Signals.Get<GameAPI.OnGameEnd>().AddListener(OnGameEnd);
    }
    private void OnDestroy()
    {
        Signals.Get<GameAPI.OnStartPlay>().RemoveListener(OnStartPlay);
        Signals.Get<GameAPI.OnReplay>().RemoveListener(OnReplay);
        Signals.Get<GameAPI.OnGameEnd>().RemoveListener(OnGameEnd);
    }

    private void OnGameEnd()
    {
        panelGame.Hide(() =>
        {
            panelGameOver.Show();
        });
    }

    private void OnStartPlay()
    {
        panelMain.Hide(() =>
        {
            panelGame.Show();
        });
    }

    private void OnReplay()
    {
        panelGameOver.Hide(() =>
        {
            panelGame.Show();
        });
    }
}
