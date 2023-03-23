using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void MyAction();
public delegate void MyAction<T>(T t);

public class CharacterMovement : MonoBehaviour
{
    public Coroutine coMove = null;
    Coroutine coRot = null;
    Animator _anim = null;
    protected Animator myAnim
    {
        get
        {
            if (_anim == null)
            {
                _anim = GetComponent<Animator>();
                if (_anim == null)
                {
                    _anim = GetComponentInChildren<Animator>();
                }
            }
            return _anim;
        }
    }
    protected void MoveToPosition(Vector3 targetPos, float MovSpeed = 1.0f, float RotSpeed = 720.0f, MyAction done = null, float distance = 0.0f)
    {
        if (Vector3.Distance(targetPos, transform.position) < 0.01f)
        {
            done?.Invoke();
            return;
        }
        if (coMove != null) StopCoroutine(coMove);
        coMove = StartCoroutine(MovingToPosition(targetPos, MovSpeed, distance, done));
        if (coRot != null) StopCoroutine(coRot);
        coRot = StartCoroutine(Rotating(transform.GetChild(0), targetPos, RotSpeed));
    }

    IEnumerator MovingToPosition(Vector3 target, float MovSpeed, float distance, MyAction done = null)
    {
        Vector3 dir = target - transform.position;
        float dist = dir.magnitude;
        if (dist <= Mathf.Epsilon) yield break;
        dir.Normalize();

        myAnim.SetBool("IsMoving", true);

        while (dist > distance)
        {
                float delta = MovSpeed * Time.deltaTime;
                if (delta > dist)
                {
                    delta = dist;
                }
                dist -= delta;
                transform.Translate(dir * delta, Space.World);
            yield return null;
        }
        myAnim.SetBool("IsMoving", false);
        done?.Invoke();
    }

    public static IEnumerator Rotating(Transform transform, Vector3 target, float RotSpeed)
    {
        Vector3 dir = target - transform.position;
        if (dir.magnitude <= Mathf.Epsilon) yield break;
        dir.Normalize();

        //d = 내적
        float d = Vector3.Dot(dir, transform.forward);

        //r = radian값을 아크cos으로 변환한 값
        float r = Mathf.Acos(d);
        float angle = r * Mathf.Rad2Deg;

        if (angle > Mathf.Epsilon)
        {
            float rotDir = Vector3.Dot(dir, transform.right) < 0.0f ? -1.0f : 1.0f;
            while (angle > Mathf.Epsilon)
            {
                float delta = RotSpeed * Time.deltaTime;
                if (delta > angle)
                {
                    delta = angle;
                }
                angle -= delta;
                transform.Rotate(delta * rotDir * Vector3.up, Space.World);
                yield return null;
            }
        }
    }

    protected void FollowTarget(Transform target, float MovSpeed, float RotSpeed, MyAction reached = null)
    {
        if (coMove != null) StopCoroutine(coMove);
        coMove = StartCoroutine(FollowingTarget(target, MovSpeed, RotSpeed, reached));
        if (coRot != null) StopCoroutine(coRot);

    }

    IEnumerator FollowingTarget(Transform target, float MovSpeed, float RotSpeed, MyAction reached)
    {
        float AttackRange = 1.8f;
        while (target != null)
        {
            Vector3 dir = target.position - transform.position;
            dir.y = 0.0f;
            float dist = dir.magnitude;

            Vector3 rot = Vector3.RotateTowards(transform.forward, dir, RotSpeed * Mathf.Deg2Rad * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(rot);

            if (!(dist > (AttackRange + 0.01f)))
            {
                myAnim.SetBool("IsMoving", true);
                dir.Normalize();
                float delta = MovSpeed * Time.deltaTime;
                if (delta > dist - AttackRange)
                {
                    delta = dist - AttackRange;
                    myAnim.SetBool("IsMoving", false);
                }
                transform.Translate(dir * delta, Space.World);
            }
            else
            {
                myAnim.SetBool("IsMoving", false);
                reached?.Invoke();
            }
            yield return null;
        }
    }
    #region 도망가는 몹
    protected void RunAway(Transform target, float MovSpeed, float RotSpeed)
    {
        if (coMove != null) StopCoroutine(coMove);
        coMove = StartCoroutine(RunningAway(target, MovSpeed, RotSpeed));
        if (coRot != null) StopCoroutine(coRot);
    }

    IEnumerator RunningAway(Transform target, float MovSpeed, float RotSpeed)
    {
        while (target != null)
        {
            Vector3 dir = transform.position - target.position;
            dir.y = 0.0f;
            dir.Normalize();

            Vector3 rot = Vector3.RotateTowards(transform.forward, dir, RotSpeed * Mathf.Deg2Rad * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(rot);
            
            float delta = MovSpeed * Time.deltaTime;
            transform.Translate(dir * delta, Space.World);
            
            yield return null;
        }
    }

    protected bool StopMovement()
    {
        StopCoroutine(coMove);
        myAnim.SetBool("IsMoving", false);
        return true;
    }
    #endregion
}
