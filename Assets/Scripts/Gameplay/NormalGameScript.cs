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
                GameController.SetBallsForce(GameController.CurrentBallsForce + GameController.GetForceIncreasePerStep());
                GameController.AddBall();
                GameController.BackgroundController.ChangeColorByLevel((uint)(GameController.Score / GameController.GetForceIncreasePerStep()));
            }
        }

        public void OnBallHitEdge(IBall ball, Collision2D collision)
        {
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
            var currentDirection = ball.CurrentDirection;
            Vector2 newDirection = new Vector2();
            foreach (var contact in collision.contacts)
            {
                var normal = contact.normal;
                newDirection += Vector2.Reflect(currentDirection, normal);

            }
            ball.SetCurrentDirection((newDirection / collision.contactCount + new Vector2(bonusDirect, 0)).normalized);
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