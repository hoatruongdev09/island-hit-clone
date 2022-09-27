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

        [SerializeField] private UnityEvent<Collision2D> onCollision = new UnityEvent<Collision2D>();

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!acceptedTag.Contains(other.gameObject.tag)) { return; }
            OnCollision?.Invoke(other);
        }

    }
}