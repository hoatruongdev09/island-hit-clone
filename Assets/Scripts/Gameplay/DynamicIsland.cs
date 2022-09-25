using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Core;
using deVoid.Utils;

namespace Gameplay.Main
{

    public class DynamicIsland : MonoBehaviour, IDynamicIsland
    {

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
            Debug.Log("Do animation hit");
        }

    }
}