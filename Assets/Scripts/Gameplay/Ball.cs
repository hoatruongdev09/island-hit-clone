using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Core;
using deVoid.Utils;

namespace Gameplay.Main
{
    public class Ball : MonoBehaviour, IBall
    {
        public bool IsDead { get; private set; }
        [field: SerializeField] public float CurrentForce { get; private set; }

        [field: SerializeField] public Vector2 CurrentDirection { get; private set; }

        public IColliderDetector ColliderDetector => colliderDetector;


        [SerializeField] private ColliderDetector colliderDetector;
        [SerializeField] private Rigidbody2D rigidbody;
        private GameAPI.OnBallHitDynamicIsland onBallHitDynamicIsland;
        private GameAPI.OnBallHitEdge onBallHitEdge;

        private void Start()
        {
            onBallHitDynamicIsland = Signals.Get<GameAPI.OnBallHitDynamicIsland>();
            onBallHitEdge = Signals.Get<GameAPI.OnBallHitEdge>();
        }

        public void OnFixUpdate(float fixDelta)
        {

        }

        public void OnUpdate(float delta)
        {
            Move();
        }

        public void Move()
        {
            // rigidbody.AddForce(CurrentDirection * CurrentForce, ForceMode2D.Impulse);
            rigidbody.velocity = (CurrentDirection * CurrentForce);
        }
        public void SetCurrentDirection(Vector2 newDirection)
        {
            CurrentDirection = newDirection;
        }

        public void SetCurrentForce(float amount)
        {
            CurrentForce = amount;
        }

        public void OnCollisionWith(Collision2D other)
        {
            if (IsDead) { return; }
            if (other.gameObject.CompareTag("DynamicIsland"))
            {
                onBallHitDynamicIsland.Dispatch(this);
                onBallHitEdge.Dispatch(this, other);
            }
            else if (other.gameObject.CompareTag("ScreenEdge"))
            {
                onBallHitEdge.Dispatch(this, other);
            }
            else if (other.gameObject.CompareTag("HandleBar"))
            {
                onBallHitEdge.Dispatch(this, other);
            }
            else
            {
                Debug.Log($"No case for tag {other.gameObject.tag}");
            }
        }

        public void SetDead()
        {
            IsDead = true;
            Destroy(gameObject, 3f);
        }

    }
}