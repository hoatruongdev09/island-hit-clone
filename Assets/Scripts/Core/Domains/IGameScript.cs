using UnityEngine;

namespace Gameplay.Core
{
    public interface IGameScript : IUpdate
    {
        IGameController GameController { get; }
        void OnBallHitEdge(IBall ball, Collision2D collision);
        void OnBallHitDynamicIsland(IBall ball);
        void OnBallHitBottomEdge(IBall ball);

        bool IsGameOver();
    }
}