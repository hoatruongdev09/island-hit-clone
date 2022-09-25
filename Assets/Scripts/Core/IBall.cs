using UnityEngine;

namespace Gameplay.Core
{
    public interface IBall : IUpdate
    {
        float CurrentForce { get; }
        Vector2 CurrentDirection { get; }
        IColliderDetector ColliderDetector { get; }

        void SetCurrentForce(float amount);
        void SetCurrentDirection(Vector2 newDirection);


    }
}