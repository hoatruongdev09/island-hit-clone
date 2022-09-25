using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Core;
using deVoid.Utils;
using DG.Tweening;

namespace Gameplay.Main
{

    public class DynamicIsland : MonoBehaviour, IDynamicIsland
    {
        [SerializeField] private Transform fadeTransform;

        private Tween tween;
        private void Awake()
        {
            Signals.Get<GameAPI.OnBallHitDynamicIsland>().AddListener(OnHitWithBall);
        }
        private void OnDestroy()
        {
            Signals.Get<GameAPI.OnBallHitDynamicIsland>().RemoveListener(OnHitWithBall);
        }
        private void OnHitWithBall(IBall ball)
        {
            DoAnimationHit();
        }
        public void DoAnimationHit()
        {
            if (tween != null)
            {
                tween.Kill();
            }
            tween = fadeTransform.DOScale(Vector3.one, .3f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                fadeTransform.DOScale(Vector3.zero, .15f);
            });
        }

    }
}