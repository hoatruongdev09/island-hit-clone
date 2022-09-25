using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Core;

namespace Gameplay.Main
{
    public class Ball : MonoBehaviour, IBall
    {
        public float CurrentForce => throw new System.NotImplementedException();

        public Vector2 CurrentDirection => throw new System.NotImplementedException();

        public IColliderDetector ColliderDetector => throw new System.NotImplementedException();

        public void FixUpdate(float fixDelta)
        {
            throw new System.NotImplementedException();
        }

        public void OnUpdate(float delta)
        {
            throw new System.NotImplementedException();
        }

        public void SetCurrentDirection(Vector2 newDirection)
        {
            throw new System.NotImplementedException();
        }

        public void SetCurrentForce(float amount)
        {
            throw new System.NotImplementedException();
        }

    }
}