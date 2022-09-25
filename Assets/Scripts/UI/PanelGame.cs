using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using deVoid.Utils;
using System;

public class PanelGame : BasePanel
{
    [SerializeField] private TMP_Text textPoint;

    private void Awake()
    {
        Signals.Get<GameAPI.OnScoreChange>().AddListener(OnScoreChange);
    }
    private void OnDestroy()
    {
        Signals.Get<GameAPI.OnScoreChange>().RemoveListener(OnScoreChange);
    }

    private void OnScoreChange(uint lastScore, uint newScore)
    {
        textPoint.text = newScore.ToString();
    }
}
