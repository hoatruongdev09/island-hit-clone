namespace Gameplay.Core
{
    public interface IUpdate
    {
        void OnUpdate(float delta);
        void FixUpdate(float fixDelta);
    }
}