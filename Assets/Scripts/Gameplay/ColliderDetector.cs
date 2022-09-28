using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Core;
using UnityEngine.Events;

namespace Gameplay.Main
{
    public class ColliderDetector : MonoBehaviour, IColliderDetector
    {

        [SerializeField] private List<string> acceptedTag = new List<string> { "HandleBar", "ScreenEdge", "DynamicIsland" };

        public UnityEvent<Collision2D> OnCollision { get => onCollision; }
        public UnityEvent<Collision2D> OnCollisionStay { get => onCollisionStay; }


        [SerializeField] private UnityEvent<Collision2D> onCollision = new UnityEvent<Collision2D>();
        [SerializeField] private UnityEvent<Collision2D> onCollisionStay = new UnityEvent<Collision2D>();
        private GameObject lastCollideObject = null;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!acceptedTag.Contains(other.gameObject.tag)) { return; }
            OnCollision?.Invoke(other);
            var contact = other.GetContact(0).point;
        }
        private void OnCollisionStay2D(Collision2D other)
        {
            if (!acceptedTag.Contains(other.gameObject.tag)) { return; }
            OnCollisionStay?.Invoke(other);
        }
    }
}