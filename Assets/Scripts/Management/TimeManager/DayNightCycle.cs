using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICycleCallback
{
    void OnCycle(float timeOfDay);
}
public interface IMorningCallback
{
    void MorningCallback();
}
public interface INightCallback
{
    void NightCallback();
}

public class DayNightCycle : MonoBehaviour
{
    [System.Serializable]
    public class TimePeriod
    {
        [Tooltip("�ð����� �̸��Դϴ�.")]
        public string name;

        [Tooltip("�ð����� ���� �ð��Դϴ�. (0~1)")]
        public float startTime;

        [Tooltip("�ð����� ���� �ð��Դϴ�. (0~1)")]
        public float endTime;

        [Tooltip("�ð����� ���� �����Դϴ�. (��)")]
        public float angleStart;

        [Tooltip("�ð����� ���� �����Դϴ�. (��)")]
        public float angleEnd;

        [Tooltip("�ð��� ���� ���� ��� ������ ���۰��Դϴ�.")]
        public float lightValueStart;

        [Tooltip("�ð��� ���� ���� ��� ������ �����Դϴ�.")]
        public float lightValueEnd;

        [Tooltip("�ð��뿡�� ������ �ݹ� �Լ��Դϴ�.")]
        public ICycleCallback callback;
    }

    [SerializeField] List<Component> CallBackBehaviorScripts;

    [SerializeField] Light directionalLight;
    [SerializeField] List<TimePeriod> timePeriods = new List<TimePeriod>();
    [SerializeField] float cycleDuration = 1.0f;

    private float startTime;
    private float currentLightValue;
    private TimePeriod currentPeriod;

    private void Start()
    {
        startTime = Time.time;
        currentPeriod = GetCurrentTimePeriod();

        StartCoroutine(TESTCORUTINE());
    }

    private void Update()
    {
        float timeOfDay = GetTimeofDay(cycleDuration);
        currentPeriod = GetCurrentTimePeriod();
        
        SetSunAngle(timeOfDay);


        UpdateLightValue(timeOfDay);
        ExecuteCallback(currentPeriod, timeOfDay);
    }
    IEnumerator TESTCORUTINE()
    {
        while (true)
        {
            float timeOfDay = GetTimeofDay(cycleDuration);
            Debug.Log(GetCurrentTimePeriod().name + timeOfDay);
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
    /// �Ϸ縦 cycleDuration������ �����մϴ�. cycleDuration�� n�̸� �Ϸ�� n�е��� ����˴ϴ�.
    /// </summary>
    /// <returns>���� �ð��� �Ϸ��� ����(0~1)�� ǥ���� ���� ��ȯ�մϴ�. ���� ��� �Ϸ��� �߰��̶�� 0.5�� ��ȯ�մϴ�.</returns>
    private float GetTimeofDay(float cycleDuration)
    {
        return (((Time.time - startTime) / 60.0f) / cycleDuration) % 1;
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

    private void ExecuteCallback(TimePeriod period, float timeOfDay)
    {
        if (period == null) return;
        if (period.callback != null) period.callback.OnCycle(timeOfDay);
        if (currentPeriod != period)
        {
            if (currentPeriod == null || currentPeriod.name != period.name)
            {
                ExecuteCallback(period.name + "Callback");
            }
        }
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

    private void ExecuteCallback(string name)
    {
        SendMessage(name, SendMessageOptions.DontRequireReceiver);
    }
    /// <summary>���� �ð��� TimePeriod�� ã�Ƽ� ��ȯ�մϴ�.</summary>
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

    //���߿� TimePeriod�� ���� �Լ� �ʵ带 �߰�()�ϰ�, period.Method()�� ���� �ݹ��Լ� �����ϵ��� ����
    private void TimeCallBackMethod(TimePeriod period)
    {
        if(period.name == "Night")
        {
            NightCallback();
        }
        else if (period.name == "morning")
        {
            MorningCallback();
        }
    }

    private bool TimeInPeriod(TimePeriod period)
    {
        float timeOfDay = GetTimeofDay(cycleDuration);
        //�����ִ� period
        if (period.startTime > period.endTime)
        {
            //is in time
            return timeOfDay >= period.startTime || timeOfDay < period.endTime;
        }

        //�Ϲ� period

        return (timeOfDay >= period.startTime && timeOfDay < period.endTime);
    }


    private void MorningCallback()
    {
        if (CallBackBehaviorScripts.Count == 0) return;
        foreach (Component com in CallBackBehaviorScripts)
        {
            if (com == null) continue;
            if (com.TryGetComponent(out IMorningCallback morningCallback)) morningCallback.MorningCallback();
        }
    }
    private void NightCallback()
    {
        if (CallBackBehaviorScripts.Count == 0) return;
        foreach (Component com in CallBackBehaviorScripts)
        {
            if (com == null) continue;
            if (com.TryGetComponent(out INightCallback nightCallback)) nightCallback.NightCallback();
        }
    }
}