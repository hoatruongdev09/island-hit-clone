using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;
using Gameplay.Core;

public class GameAPI
{
    public class OnBallHitBottomEdge : ASignal<IBall> { }
    public class OnBallHitDynamicIsland : ASignal<IBall> { }
    public class OnBallHitEdge : ASignal<IBall, Collision2D> { }

    public class OnScoreChange : ASignal<uint, uint> { }
    public class OnStartPlay : ASignal { }
    public class OnReplay : ASignal { }
    public class OnGameEnd : ASignal { }
    public class OnGoToMenu : ASignal { }

    public class MenuToGameOver : ASignal { }

    public class OnChangeUseSfx : ASignal<bool> { }
    public class OnChangeUseHaptic : ASignal<bool> { }
}
