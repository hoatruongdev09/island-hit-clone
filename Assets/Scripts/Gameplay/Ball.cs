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
        [SerializeField] private GameObject dieEffect;
        [SerializeField] private GameObject graphic;
        public IColliderDetector ColliderDetector => colliderDetector;

        public Vector3 Position => transform.position;

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
            if (IsDead) { return; }
            Move();
        }

        public void OnUpdate(float delta)
        {
        }

        public void Move()
        {
            rigidbody.velocity = (CurrentDirection * CurrentForce);
        }

        public void ForceBall(Vector2 force)
        {
            rigidbody.AddForce(force, ForceMode2D.Impulse);
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
                SfxManager.Instance.PlaySfx(SfxManager.SfxName.IslandHit);
            }
            else if (other.gameObject.CompareTag("ScreenEdge"))
            {
                SfxManager.Instance.PlaySfx(SfxManager.SfxName.WallHit);
            }
            else if (other.gameObject.CompareTag("HandleBar"))
            {
                SfxManager.Instance.PlaySfx(SfxManager.SfxName.WallHit);
            }
            else
            {
                Debug.Log($"No case for tag {other.gameObject.tag}");
            }
        }
        public void OnCollisionStayWith(Collision2D other)
        {
            if (IsDead) { return; }
            if (other.gameObject.CompareTag("DynamicIsland"))
            {
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
            SetCurrentDirection(Vector2.zero);
            SetCurrentForce(0);
            dieEffect.gameObject.SetActive(true);
            graphic.gameObject.SetActive(false);
            Destroy(gameObject, 3f);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawRay(transform.position, CurrentDirection * 10);
        }

        public Vector2 GetCurrentDirection()
        {
            return CurrentDirection;
        }
    }
}