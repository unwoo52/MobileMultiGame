using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFSM_Idle : ZombieFSM_Base
{
    private Coroutine idleCorutine = null;
    public override void EndStateBehavior()
    {
        StopCoroutine(idleCorutine);
        _zombieFSM.Anim.OnExitWanderWalk();
        //OnStopLookat
        //StopCoroutine(_zombieFSM.AnimMove.LookCorutine);
    }

    public override void StartStateBehavior()
    {
        if(idleCorutine != null) { StopCoroutine(idleCorutine); }
        idleCorutine = StartCoroutine(IdelRandomMove());
    }
    public override void StateMachine()
    {
    }

    IEnumerator IdelRandomMove()
    {
        yield return null;
        while (true)
        {
            float Ran = Random.Range(4f, 12f);
            yield return new WaitForSeconds(Ran);
            float walkTime = Random.Range(8f, 14.5f);
            RandomWalk();
            yield return new WaitForSeconds(walkTime);
            _zombieFSM.Anim.OnExitWanderWalk();
        }
    }

    private void RandomWalk()
    {
        Vector3 rvec = Quaternion.Euler(0f, Random.Range(0, 360f), 0f) * Vector3.forward;

        _zombieFSM.Anim.OnOnceLookat(transform.position + rvec, 35);
        _zombieFSM.Anim.OnWanderWalk();
    }
}
