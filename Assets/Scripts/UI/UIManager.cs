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
    [SerializeField] private PanelMenu panelMenu;
    private void Awake()
    {
        Signals.Get<GameAPI.OnStartPlay>().AddListener(OnStartPlay);
        Signals.Get<GameAPI.OnReplay>().AddListener(OnReplay);
        Signals.Get<GameAPI.OnGameEnd>().AddListener(OnGameEnd);
        Signals.Get<GameAPI.OnGoToMenu>().AddListener(OnGoToMenu);
        Signals.Get<GameAPI.MenuToGameOver>().AddListener(OnOpenGameOverFromMenu);
    }
    private void OnDestroy()
    {
        Signals.Get<GameAPI.OnStartPlay>().RemoveListener(OnStartPlay);
        Signals.Get<GameAPI.OnReplay>().RemoveListener(OnReplay);
        Signals.Get<GameAPI.OnGameEnd>().RemoveListener(OnGameEnd);
        Signals.Get<GameAPI.OnGoToMenu>().RemoveListener(OnGoToMenu);
        Signals.Get<GameAPI.MenuToGameOver>().RemoveListener(OnOpenGameOverFromMenu);
    }

    private void OnOpenGameOverFromMenu()
    {
        panelMenu.Hide(() =>
        {
            panelGameOver.Show();
        });
    }

    private void OnGoToMenu()
    {
        panelGameOver.Hide(() =>
        {
            panelMenu.Show();
        });
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
