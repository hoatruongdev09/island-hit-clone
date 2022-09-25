namespace Gameplay.Core
{
    public interface IScreenEdge
    {
        ScreenEdgeType EdgeType { get; }
        void SetSize(float width, float height);
    }
}