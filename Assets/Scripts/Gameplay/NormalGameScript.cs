using Gameplay.Core;
using UnityEngine;

namespace Gameplay.Main
{
    public class NormalGameScript : IGameScript
    {
        public IGameController GameController { get; private set; }

        public NormalGameScript(IGameController gameController)
        {
            GameController = gameController;
        }

        public void OnBallHitBottomEdge(IBall ball)
        {
            ball.SetDead();
            GameController.AliveBalls.Remove(ball);
        }

        public void OnBallHitDynamicIsland(IBall ball)
        {
            GameController.SetScore(GameController.Score + 1);
        }

        public void OnBallHitEdge(IBall ball, Collision2D collision)
        {
            var currentDirection = ball.CurrentDirection;
            var normal = collision.contacts[0].normal;
            Debug.Log($"normal: {normal.x} {normal.y}");
            var newDirection = Vector2.Reflect(currentDirection, normal);
            ball.SetCurrentDirection(newDirection.normalized);
        }

        public bool IsGameOver()
        {
            return GameController.AliveBalls.Count == 0;
        }

        public void OnUpdate(float delta)
        {
            if (!GameController.GameStarted) { return; }
            foreach (var ball in GameController.AliveBalls)
            {
                if (ball.IsDead) { continue; }
                ball.OnUpdate(delta);
            }
        }

        public void OnFixUpdate(float fixDelta)
        {
            if (!GameController.GameStarted) { return; }
            foreach (var ball in GameController.AliveBalls)
            {
                if (ball.IsDead) { continue; }
                ball.OnFixUpdate(fixDelta);
            }
        }
    }
}