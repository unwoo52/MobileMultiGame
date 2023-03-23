using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFSM : MonoBehaviour, ILostTarget
{
    [SerializeField] private ZombieFSM_Idle _zombieFSM_Idle;
    [SerializeField] private ZombieFSM_Tracking _zombieFSM_Tracking;
    [SerializeField] private AnimContoller _anim;
    [SerializeField] private TrackingScript trackingScript;
    public AnimContoller Anim { get => _anim; }
    public TrackingScript TrackingScript => trackingScript;
    public enum ZOMBIEBEHAVIOR
    {
        Start,
        Idle, // 배회. 주변을 배회
        curiosity, // 호기심. 소음에 이끌려 좀비가 모일 때 발동
        Attck, // 플레이어가 설치한 오브젝트를 발견하고 공격할 때
        Tracking,//추적. 적을 인지해서 추적하는 상황
        Moribund,//시체상태. 부활의 여지가 있는 상황
        Death//온전한 사망. 잠시 후 시체가 사라짐
    }

    private ZOMBIEBEHAVIOR _currentState = ZOMBIEBEHAVIOR.Start;

    private void Start()
    {
        if(!Initialize()) 
            Debug.LogError("Init() 실패! 컴포넌트를 찾지 못했습니다.");

        ChangerState(ZOMBIEBEHAVIOR.Idle); 
    }

    private void Update()
    {
        ConditionMethod_StatusChange();
        StateMachine();
    }
    public void ChangerState(ZOMBIEBEHAVIOR newState)
    {
        EndStateBehavior();
        _currentState = newState;
        StartStateBehavior();
    }


    public void StateMachine()
    {
        switch (_currentState)
        {
            case ZOMBIEBEHAVIOR.Start:
                break;
            case ZOMBIEBEHAVIOR.Idle:
                _zombieFSM_Idle.StateMachine();
                break;
            case ZOMBIEBEHAVIOR.curiosity:
                break;
            case ZOMBIEBEHAVIOR.Attck:
                break;
            case ZOMBIEBEHAVIOR.Tracking:
                _zombieFSM_Tracking.StateMachine();
                break;
            case ZOMBIEBEHAVIOR.Moribund:
                break;
            case ZOMBIEBEHAVIOR.Death:
                break;
        }
    }

    private void EndStateBehavior()
    {
        switch (_currentState)
        {
            case ZOMBIEBEHAVIOR.Start:
                break;
            case ZOMBIEBEHAVIOR.Idle:
                _zombieFSM_Idle.EndStateBehavior();
                break;
            case ZOMBIEBEHAVIOR.curiosity:
                break;
            case ZOMBIEBEHAVIOR.Attck:
                break;
            case ZOMBIEBEHAVIOR.Tracking:
                _zombieFSM_Tracking.EndStateBehavior();
                break;
            case ZOMBIEBEHAVIOR.Moribund:
                break;
            case ZOMBIEBEHAVIOR.Death:
                break;
        }
    }

    private void StartStateBehavior()
    {
        switch (_currentState)
        {
            case ZOMBIEBEHAVIOR.Start:
                break;
            case ZOMBIEBEHAVIOR.Idle:
                _zombieFSM_Idle.StartStateBehavior();
                break;
            case ZOMBIEBEHAVIOR.curiosity:
                break;
            case ZOMBIEBEHAVIOR.Attck:
                break;
            case ZOMBIEBEHAVIOR.Tracking:
                _zombieFSM_Tracking.StartStateBehavior();
                break;
            case ZOMBIEBEHAVIOR.Moribund:
                break;
            case ZOMBIEBEHAVIOR.Death:
                break;
        }
    }

    private void ConditionMethod_StatusChange()
    {
        if (trackingScript.GameOnjects.Count > 0 && _currentState == ZOMBIEBEHAVIOR.Idle)
        {
            ChangerState(ZombieFSM.ZOMBIEBEHAVIOR.Tracking);
        }
    }

    /* codes */

    private bool Initialize()
    {
        if (TryGetComponent(out AnimContoller animContoller)) _anim = animContoller; else return false;
        if (TryGetComponent(out ZombieFSM_Idle zombieFSM_Idle)) _zombieFSM_Idle = zombieFSM_Idle; else return false;
        if (TryGetComponent(out ZombieFSM_Tracking zombieFSM_Tracking)) _zombieFSM_Tracking = zombieFSM_Tracking; else return false;

        return true;
    }

    public void LostTarget(GameObject gameobject)
    {
        if (trackingScript.GameOnjects == null) ChangerState(ZOMBIEBEHAVIOR.Idle);
        //else 다른 타겟이 있다면 타겟 변경
        //else 공격할 오브젝트가 있다면 attack으로 상태 변경 후 추적 실행
    }
}
