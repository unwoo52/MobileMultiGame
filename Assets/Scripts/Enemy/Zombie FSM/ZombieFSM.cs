using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFSM : MonoBehaviour, ILostTarget
{
    [SerializeField] private ZombieFSM_Base _zombieFSM_Start;
    [SerializeField] private ZombieFSM_Base _zombieFSM_Idle;
    [SerializeField] private ZombieFSM_Base _zombieFSM_Tracking;
    [SerializeField] private AnimContoller _anim;
    [SerializeField] private TrackingScript trackingScript;
    [SerializeField] private ZOMBIEBEHAVIOR InitState;
    public AnimContoller Anim { get => _anim; }
    public TrackingScript TrackingScript => trackingScript;
    public enum ZOMBIEBEHAVIOR
    {
        Start, // ���� �� ��
        Idle, // ��ȸ. �ֺ��� ��ȸ
        curiosity, // ȣ���. ������ �̲��� ���� ���� �� �ߵ�
        Attck, // �÷��̾ ��ġ�� ������Ʈ�� �߰��ϰ� ������ ��
        Tracking,//����. ���� �����ؼ� �����ϴ� ��Ȳ
        Moribund,//��ü����. ��Ȱ�� ������ �ִ� ��Ȳ
        Death//������ ���. ��� �� ��ü�� �����
    }

    private ZOMBIEBEHAVIOR _currentState;

    private void Start()
    {
        _currentState = InitState;
        if (!Initialize()) 
            Debug.LogError("Init() ����! ������Ʈ�� ã�� ���߽��ϴ�.");
        StartStateBehavior();
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
                _zombieFSM_Start.StateMachine();
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
                _zombieFSM_Start.EndStateBehavior();
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
                _zombieFSM_Start.StartStateBehavior();
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
        //else �ٸ� Ÿ���� �ִٸ� Ÿ�� ����
        //else ������ ������Ʈ�� �ִٸ� attack���� ���� ���� �� ���� ����
    }
}
