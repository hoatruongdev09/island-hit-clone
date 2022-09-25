using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Core
{
    public interface ISpawnController
    {
        IBall CreateBall();
    }
}