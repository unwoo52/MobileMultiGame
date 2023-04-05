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
        // 특정 시간에 Listener를 호출하는 로직 작성
        // Listener?.Invoke(); // Listener가 null이 아닐 때만 호출하는 코드
    }
}