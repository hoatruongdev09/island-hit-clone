using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Main;
public class ScreenEdgeController : MonoBehaviour
{
    [SerializeField] private ScreenEdge[] edges;
    [SerializeField] private Canvas canvas;

    private void Start()
    {
        ConfigEdges();
    }

    private void ConfigEdges()
    {
        var size = canvas.GetComponent<RectTransform>().sizeDelta;
        foreach (var edge in edges)
        {
            if (edge.EdgeType == ScreenEdgeType.undefined) { continue; }
            if (edge.EdgeType == ScreenEdgeType.Top || edge.EdgeType == ScreenEdgeType.Bottom)
            {
                edge.SetSize(size.x, 2);
            }
            else if (edge.EdgeType == ScreenEdgeType.Right || edge.EdgeType == ScreenEdgeType.Left)
            {
                edge.SetSize(2, size.y);
            }
        }
    }
}
