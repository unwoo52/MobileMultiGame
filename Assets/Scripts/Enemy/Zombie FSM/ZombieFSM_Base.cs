using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFSM_Base : MonoBehaviour
{

    [SerializeField] protected ZombieFSM _zombieFSM;
    private void Start()
    {
        if (!Initialized())
            Debug.LogError("Init() ����! ������Ʈ�� ã�� ���߽��ϴ�.");
    }
    private bool Initialized()
    {
        bool temp = true;
        if (!InitZombieFSM()) temp = false;

        return temp;
    }

    private bool InitZombieFSM()
    {
        if (_zombieFSM == null) return true;
        if (TryGetComponent(out ZombieFSM zombieFSM)) _zombieFSM = zombieFSM; else return false;
        return true;
    }

    public virtual void EndStateBehavior()
    {
    }
    public virtual void StartStateBehavior()
    {
    }
    public virtual void StateMachine()
    {
    }
}