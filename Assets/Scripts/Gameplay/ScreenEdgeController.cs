using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Main;
using UnityEngine.UI;

public class ScreenEdgeController : MonoBehaviour
{
    public static ScreenEdgeController Instance { get; private set; }
    public float CanvasScaleFactor { get; private set; }
    public Vector2 CanvasResolution { get; private set; }
    [SerializeField] private ScreenEdge[] edges;
    [SerializeField] private Canvas canvas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        CanvasScaleFactor = canvas.scaleFactor;
        CanvasResolution = canvas.GetComponent<CanvasScaler>().referenceResolution;
    }
    private void OnDestroy()
    {
        Instance = null;
    }

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
                edge.SetSize(size.x, 5);
            }
            else if (edge.EdgeType == ScreenEdgeType.Right || edge.EdgeType == ScreenEdgeType.Left)
            {
                edge.SetSize(5, size.y);
            }
        }
    }
}
