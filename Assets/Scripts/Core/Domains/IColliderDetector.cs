using UnityEngine;
using UnityEngine.Events;
namespace Gameplay.Core
{
    public interface IColliderDetector
    {
        UnityEvent<Collision2D> OnCollision { get; }
        UnityEvent<Collision2D> OnCollisionStay { get; }
    }
}