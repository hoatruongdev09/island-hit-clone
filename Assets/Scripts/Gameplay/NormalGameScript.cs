using deVoid.Utils;
using Gameplay.Core;
using UnityEngine;

namespace Gameplay.Main
{
    public class NormalGameScript : IGameScript
    {
        public IGameController GameController { get; private set; }
        private GameAPI.OnBallHitBottomEdge onBallHitBottomEdge;

        public NormalGameScript(IGameController gameController)
        {
            GameController = gameController;
            onBallHitBottomEdge = Signals.Get<GameAPI.OnBallHitBottomEdge>();
        }

        public void OnBallHitBottomEdge(IBall ball)
        {
            ball.SetDead();
            GameController.AliveBalls.Remove(ball);
        }

        public void OnBallHitDynamicIsland(IBall ball)
        {
            GameController.SetScore(GameController.Score + 1);
            if (GameController.Score != 0 && GameController.Score % GameController.GetScoreStep() == 0)
            {
                if (GameController.CurrentBallsForce >= GameController.MaxBallForce)
                {
                    GameController.SetBallsForce(GameController.MaxBallForce);
                }
                else
                {
                    GameController.SetBallsForce(GameController.CurrentBallsForce + GameController.GetForceIncreasePerStep());
                }
                if (GameController.AliveBalls.Count < GameController.MaxBallInTime)
                {
                    GameController.AddBall();
                }
                GameController.BackgroundController.ChangeColorByLevel((uint)(GameController.Score / GameController.GetForceIncreasePerStep()));
            }
        }

        public void OnBallHitEdge(IBall ball, Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ball")) { return; }
            var edge = collision.gameObject.GetComponent<IScreenEdge>();
            if (edge != null && edge.EdgeType == ScreenEdgeType.Bottom)
            {
                onBallHitBottomEdge.Dispatch(ball);
                return;
            }
            var handleBar = collision.gameObject.GetComponent<IHandleBar>();
            float bonusDirect = 0;
            if (handleBar != null)
            {
                bonusDirect = handleBar.GetHorizontalForce();
            }
            var currentDirection = ball.GetCurrentDirection();
            var contact = collision.GetContact(0).point;
            // var inDirection = new Vector2(contact.x - ball.Position.x, contact.y - ball.Position.y);
            var normal = collision.GetContact(0).normal;
            var newDirection = Vector2.Reflect(currentDirection, normal);
            var finalDirection = (newDirection + new Vector2(bonusDirect, 0)).normalized;

            var inAngle = Vector2.Angle(currentDirection, normal);
            if (inAngle < 90f) { return; }

            ball.SetCurrentDirection(finalDirection);
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