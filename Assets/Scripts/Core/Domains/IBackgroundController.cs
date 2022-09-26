using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Core
{
    public interface IBackgroundController
    {
        void ChangeColorByLevel(uint level);
        void ChangeColor(Color newColor);
        void ResetToDefaultColor();
    }
}