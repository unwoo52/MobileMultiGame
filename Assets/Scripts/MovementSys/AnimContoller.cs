using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimContoller : AnimMovement
{
    public Action OnAttack => Attack;
    public Action<Vector3> OnMove => Move;
    public Action OnOnlyMove => OnlyMove;
    public Action OnStopMove => StopMove;
    public Action<Vector3, float> OnOnceLookat => OnceLookat;
    public Action OnStopLookat => StopLookat;
    public Action OnWanderWalk => WanderWalk;
    public Action OnExitWanderWalk => ExitWanderWalk;
    public Action<Vector3, float> OnLookat => Lookat;
    public Action OnScream => Scream;
}
