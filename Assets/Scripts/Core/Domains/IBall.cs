using UnityEngine;

namespace Gameplay.Core
{
    public interface IBall : IUpdate
    {
        bool IsDead { get; }
        float CurrentForce { get; }
        Vector3 Position { get; }
        Vector2 CurrentDirection { get; }
        IColliderDetector ColliderDetector { get; }

        void SetCurrentForce(float amount);
        void SetCurrentDirection(Vector2 newDirection);
        void SetDead();
        void Move();
        void ForceBall(Vector2 force);
        Vector2 GetCurrentDirection();
        void OnCollisionWith(Collision2D other);
    }
}