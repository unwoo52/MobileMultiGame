using MyNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class DayNightCycle : MonoBehaviour
{
    #region singleton
    private static DayNightCycle _instance = null;
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static DayNightCycle Instance
    {
        get
        {
            if (null == _instance)
            {
                return null;
            }
            return _instance;
        }
    }
    #endregion

    public UnityEvent MorningCallbackEvent;
    public UnityEvent NightCallbackEvent;
    [System.Serializable]
    public class TimePeriod
    {
        [Tooltip("시간대의 이름입니다.")]
        public string name;

        [Tooltip("시간대의 시작 시간입니다. (0~1)")]
        public float startTime;

        [Tooltip("시간대의 종료 시간입니다. (0~1)")]
        public float endTime;

        [Tooltip("시간대의 시작 각도입니다. (도)")]
        public float angleStart;

        [Tooltip("시간대의 종료 각도입니다. (도)")]
        public float angleEnd;

        [Tooltip("시간대 동안 빛의 밝기 강도의 시작값입니다.")]
        public float lightValueStart;

        [Tooltip("시간대 동안 빛의 밝기 강도의 끝값입니다.")]
        public float lightValueEnd;
    }

    [SerializeField] List<Component> CallBackBehaviorScripts;

    [SerializeField] Light directionalLight;
    [SerializeField] List<TimePeriod> timePeriods = new List<TimePeriod>();
    [SerializeField] float cycleDuration = 1.0f;

    private float _startTime;
    public float StartTime => _startTime;
    private float currentLightValue;
    private TimePeriod currentPeriod;

    private void Start()
    {
        _startTime = Time.time;
        currentPeriod = GetCurrentTimePeriod();

        StartCoroutine(TESTCORUTINE());
    }

    private void Update()
    {
        float timeOfDay = GetTimeofDay(cycleDuration);
        currentPeriod = GetCurrentTimePeriod();
        
        SetSunAngle(timeOfDay);


        UpdateLightValue(timeOfDay);
    }
    IEnumerator TESTCORUTINE()
    {
        while (true)
        {
            float timeOfDay = GetTimeofDay(cycleDuration);
            yield return new WaitForSeconds(1);
        }
    }
    private void SetSunAngle(float timeOfDay)
    {
        if (currentPeriod.angleStart > currentPeriod.angleEnd) currentPeriod.angleStart -= 360;
        float timeZoneTime = GetNormalizedDifference(currentPeriod.startTime, currentPeriod.endTime);
        float t = (timeOfDay - currentPeriod.startTime) < 0? (timeOfDay - currentPeriod.startTime) + 1 : (timeOfDay - currentPeriod.startTime);
        float sunAngle = Mathf.Lerp(currentPeriod.angleStart, currentPeriod.angleEnd, t / timeZoneTime);
        directionalLight.transform.localRotation = Quaternion.Euler(new Vector3(sunAngle, 0.0f, 0.0f));
    }
    /// <summary>
    /// 하루를 cycleDuration분으로 정의합니다. cycleDuration이 n이면 하루는 n분동안 진행됩니다.
    /// </summary>
    /// <returns>현재 시간을 하루의 비율(0~1)로 표현한 값을 반환합니다. 예를 들어 하루의 중간이라면 0.5를 반환합니다.</returns>
    private float GetTimeofDay(float cycleDuration)
    {
        return (((Time.time - _startTime) / 60.0f) / cycleDuration) % 1;
    }

    private void UpdateLightValue(float timeOfDay)
    {
        if (currentPeriod != null)
        {
            float timeZoneTime = GetNormalizedDifference(currentPeriod.startTime, currentPeriod.endTime);
            float t = (timeOfDay - currentPeriod.startTime) < 0 ? (timeOfDay - currentPeriod.startTime) + 1 : (timeOfDay - currentPeriod.startTime);

            currentLightValue = Mathf.Lerp(currentPeriod.lightValueStart, currentPeriod.lightValueEnd, t / timeZoneTime);
        }
        directionalLight.intensity = currentLightValue;
    }

    private float GetNormalizedDifference(float start, float end)
    {
        float diff = end - start;
        if (diff < 0)
        {
            diff += 1f;
        }
        return diff;
    }

    /// <summary>현재 시간대 TimePeriod을 찾아서 반환합니다.</summary>
    private TimePeriod GetCurrentTimePeriod()
    {
        foreach (TimePeriod period in timePeriods)
        {
            if (TimeInPeriod(period))
            {
                if (currentPeriod != null)
                {
                    if (currentPeriod.name != period.name) TimeCallBackMethod(period);
                }
                return period;
            }
        }
        return null;
    }

    //나중에 TimePeriod에 직접 함수 필드를 추가()하고, period.Method()를 통해 콜백함수 실행하도록 변경
    private void TimeCallBackMethod(TimePeriod period)
    {
        if(period.name == "Night")
        {
            NightCallbackEvent.Invoke();
        }
        else if (period.name == "morning")
        {
            MorningCallbackEvent.Invoke();
        }
    }

    private bool TimeInPeriod(TimePeriod period)
    {
        float timeOfDay = GetTimeofDay(cycleDuration);
        //걸쳐있는 period
        if (period.startTime > period.endTime)
        {
            //is in time
            return timeOfDay >= period.startTime || timeOfDay < period.endTime;
        }

        //일반 period

        return (timeOfDay >= period.startTime && timeOfDay < period.endTime);
    }
}