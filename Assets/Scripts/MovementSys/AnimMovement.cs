using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AnimMovement : MonoBehaviour
{
    private Coroutine _lookCorutine = null;
    public Coroutine LookCorutine { get { return _lookCorutine; } }
    [SerializeField]
    private Animator _animator;


    private void Start()
    {
        if (!Initialize())
            Debug.LogError("ERROR! 컴포넌트를 찾지 못했습니다.");
    }

    protected void Hit() { }

    protected void FootL() { }

    protected void FootR() { }

    protected void Attack()
    {
        _animator.SetTrigger("TriggerAttack");
    }

    protected void OnceLookat(Vector3 toTarget, float lookspeed = 1)
    {
        if (_lookCorutine != null) StopCoroutine(_lookCorutine);
        _lookCorutine = StartCoroutine(LookatCorutine(toTarget, lookspeed));
    }

    protected void Lookat(Vector3 toTarget, float rotatespeed = 20f)
    {
        Quaternion targetRotation = Quaternion.LookRotation(toTarget - transform.position);
        transform.rotation =
            Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotatespeed);
    }

    protected void Move(Vector3 toTarget)
    {
        Lookat(toTarget);
        _animator.SetBool("IsMove", true);
    }

    protected void OnlyMove()
    {
        _animator.SetBool("IsMove", true);

    }

    protected void StopMove()
    {
        _animator.SetBool("IsMove", false);
    }

    protected void StopLookat()
    {
        if (_lookCorutine != null) StopCoroutine(_lookCorutine);
    }

    protected void Scream()
    {
        _animator.SetTrigger("TriggerScream");
    }

    protected void WanderWalk()
    {
        _animator.SetTrigger("WanderWalk");
    }

    protected void ExitWanderWalk()
    {
        _animator.SetTrigger("ExitWanderWalk");
    }
    protected void SetVerticalParam(float value)
    {
        _animator.SetFloat("Vertical", value);
    }
    protected void SetHorizontalParam(float value)
    {
        _animator.SetFloat("Horizontal", value);
    }


    IEnumerator LookatCorutine(Vector3 Target, float lookspeed)
    {
        // 현재 객체의 위치와 타겟 위치를 기반으로 회전 방향을 계산합니다.
        Vector3 dir = Target - transform.position;
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
                float delta = lookspeed * Time.deltaTime;
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


    private bool Initialize()
    {
        if (TryGetComponent(out Animator animator)) _animator = animator; else return false;

        return true;
    }
}
