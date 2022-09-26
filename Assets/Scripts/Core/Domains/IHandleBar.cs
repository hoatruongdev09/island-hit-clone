namespace Gameplay.Core
{
    public interface IHandleBar
    {
        float CurrentSpeed { get; }
        void SetSpeed(float newSpeed);
        void Move(float direction);
        float GetHorizontalForce();
    }
}