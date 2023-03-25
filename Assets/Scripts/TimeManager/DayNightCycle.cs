using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] Light directionalLight;
    [SerializeField] float time = 1.0f;

    [SerializeField] float nightLightValue = 0.05f;
    [SerializeField] float noonLightValue = 2.0f;

    [SerializeField] List<Component> CallBackBehaviorScripts;

    private float currentLightValue;
    private float startTime; // ���� �ð�

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        float timeOfDay = (((Time.time - startTime) / 60.0f) / time) % 1;
        float sunAngle = Mathf.Lerp(0.0f, 360.0f, timeOfDay); // -90������ 270�� ������ ���� ���

        // �𷺼� ����Ʈ�� �����̼� ����
        directionalLight.transform.localRotation = Quaternion.Euler(new Vector3(sunAngle, 0.0f, 0.0f));

        // Ȳȥ ����
        if (timeOfDay >= 0.4f && timeOfDay < 0.6f)
        {
            float t = Mathf.InverseLerp(0.4f, 0.6f, timeOfDay);
            currentLightValue = Mathf.Lerp(noonLightValue, nightLightValue, t);
        }
        else if(0.6f  <= timeOfDay && timeOfDay < 0.8f)
        {
            currentLightValue = nightLightValue;
        }
        // ������ ��ħ ����
        else if (timeOfDay >= 0.8f)
        {
            float t = Mathf.InverseLerp(0.8f, 1.0f, timeOfDay);
            currentLightValue = Mathf.Lerp(nightLightValue, noonLightValue, t);
        }
        // �� ����
        else
        {
            currentLightValue = noonLightValue;
        }

        // �𷺼� ����Ʈ�� ��� ����
        directionalLight.intensity = currentLightValue;
    }

    private void TSESTCALLBACK()
    {
        if (CallBackBehaviorScripts.Count == 0) return;
        foreach(Component com in CallBackBehaviorScripts)
        {
            if (com == null) continue;
            if (com.TryGetComponent(out IMorningCallback morningCallback)) morningCallback.MorningCallback();
            if (com.TryGetComponent(out INightCallback nightCallback)) nightCallback.NightCallback();
        }        
    }
}