namespace Gameplay.Core
{
    public interface IUpdate
    {
        void OnUpdate(float delta);
        void OnFixUpdate(float fixDelta);
    }
}