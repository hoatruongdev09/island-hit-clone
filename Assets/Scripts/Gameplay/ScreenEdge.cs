using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Core;

namespace Gameplay.Main
{
    public class ScreenEdge : MonoBehaviour, IScreenEdge
    {
        [field: SerializeField] public ScreenEdgeType EdgeType { get; set; }

        [SerializeField] private BoxCollider2D boxCollider;

        public void SetSize(float width, float height)
        {
            var size = boxCollider.size;
            size.x = width;
            size.y = height;
            boxCollider.size = size;
        }
    }
}