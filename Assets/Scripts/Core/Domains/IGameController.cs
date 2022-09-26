using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Core
{
    public interface IGameController : IUpdate
    {
        List<IBall> AliveBalls { get; }
        IBackgroundController BackgroundController { get; }
        ISpawnController SpawnController { get; }
        IGameScript GameScript { get; }
        uint Score { get; }
        bool GameStarted { get; }
        float CurrentBallsForce { get; }

        uint GetScoreStep();
        float GetForceIncreasePerStep();
        void AddBall();
        void SetScore(uint newScore);
        void StartGame();
        void EndGame();
        void SetBallsForce(float newForce);
        void LoadGameScript();
        Vector2 RandomDirection();
        void ResetGame();

    }
}