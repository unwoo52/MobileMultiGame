using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeCallBack : MonoBehaviour
{
    public UnityEvent callbackEvent;


    private DayNightCycle timeManager;

    private void Start()
    {
        timeManager = DayNightCycle.Instance;
        InvokeRepeating("CallListener", 0f, 1f);
    }

    private void CallListener()
    {
        //float currentTime = Time.time - timeManager._baseTime;
        // Ư�� �ð��� Listener�� ȣ���ϴ� ���� �ۼ�
        // Listener?.Invoke(); // Listener�� null�� �ƴ� ���� ȣ���ϴ� �ڵ�
    }
}