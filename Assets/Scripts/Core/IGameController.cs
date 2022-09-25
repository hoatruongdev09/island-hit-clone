using System.Collections.Generic;

namespace Gameplay.Core
{
    public interface IGameController : IUpdate
    {
        List<IBall> AliveBalls { get; }

        void AddBall();


    }
}