using MyNamespace;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class DayNightCycle : MonoBehaviourPunCallbacks
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
    }

    [SerializeField] List<Component> CallBackBehaviorScripts;
    [SerializeField] private Light _directionalLight;

    public Light DirectionalLight
    {
        get => _directionalLight;
        set => _directionalLight = value;
    }
    [SerializeField] List<TimePeriod> timePeriods = new List<TimePeriod>();

    //�Ϸ縦 cycleDuration������ �����մϴ�. cycleDuration�� n�̸� �Ϸ�� n�е��� ����˴ϴ�.
    [SerializeField] float cycleDuration = 1.0f;

    private float currentLightValue;
    private TimePeriod currentPeriod;

    public double _baseTime;
    private void Start()
    {
        PhotonView photonView = GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient)
        {
            _baseTime = PhotonNetwork.Time;
            photonView.RPC("SetBaseTime", RpcTarget.Others, _baseTime);
        }

        currentPeriod = GetCurrentTimePeriod();

        _baseTimeText = GameObject.Find("BaseTimeText").GetComponent<TMP_Text>();
    }
    #region Photon
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // ���ο� �÷��̾�� ������ _baseTime�� �����մϴ�.
            photonView.RPC(nameof(SetBaseTime), newPlayer, _baseTime);
        }
    }
    [SerializeField] private TMP_Text _baseTimeText;
    [PunRPC]
    private void SetBaseTime(double baseTime)
    {
        _baseTime = baseTime;
    }

    // ���� �ð��� ��ȯ�մϴ�.
    public float GetGameTime()
    {
        // Master Client������ PhotonNetwork.Time���� �� ���� �ð��� ���� ���� �ð��� ���մϴ�.
        if (PhotonNetwork.IsMasterClient)
        {
            return (float)(PhotonNetwork.Time - _baseTime);
        }
        // �ٸ� �÷��̾���� Master Client�κ��� ���޹��� �� ���� �ð��� ����Ͽ� ���� �ð��� ���մϴ�.
        else
        {
            return (float)((PhotonNetwork.Time - PhotonNetwork.GetPing() / 2000f) - _baseTime);
        }
    }
    #endregion

    private void Update()
    {
        //���� �ð��� ���� (��ħ �� �� ��..)
        currentPeriod = GetCurrentTimePeriod();

        //�ð� ���� timeOfDay�� ��ȯ. ex)12�������̸� 0.5 ��ȯ, �Ϸ��� �����̸� 0 ��ȯ
        float timeOfDay = GetTimeofDay(cycleDuration);

        SetSunAngle(timeOfDay);

        UpdateLightValue(timeOfDay);

        //photon test UI
        if (_baseTimeText != null)
        {
            _baseTimeText.text = "Base Time : " + _baseTime.ToString() + "\n timeOfDay :" + timeOfDay.ToString();
        }
    }
    private void SetSunAngle(float timeOfDay)
    {
        if (currentPeriod.angleStart > currentPeriod.angleEnd) currentPeriod.angleStart -= 360;
        float timeZoneTime = GetNormalizedDifference(currentPeriod.startTime, currentPeriod.endTime);
        float t = (timeOfDay - currentPeriod.startTime) < 0? (timeOfDay - currentPeriod.startTime) + 1 : (timeOfDay - currentPeriod.startTime);
        float sunAngle = Mathf.Lerp(currentPeriod.angleStart, currentPeriod.angleEnd, t / timeZoneTime);
        _directionalLight.transform.localRotation = Quaternion.Euler(new Vector3(sunAngle, 0.0f, 0.0f));
    }

    /// <summary>���� �ð��� �Ϸ��� ����(0~1)�� ǥ���� ���� ��ȯ�մϴ�. ���� ��� �Ϸ��� �߰��̶�� 0.5�� ��ȯ�մϴ�.</summary>
    private float GetTimeofDay(float cycleDuration)
    {

        // Master Client������ PhotonNetwork.Time���� �� ���� �ð��� ���� ���� �ð��� ���մϴ�.
        if (PhotonNetwork.IsMasterClient)
        {
            return (float)((PhotonNetwork.Time - _baseTime) / cycleDuration);
        }
        // �ٸ� �÷��̾���� Master Client�κ��� ���޹��� �� ���� �ð��� ����Ͽ� ���� �ð��� ���մϴ�.
        else
        {
            return (float)(((PhotonNetwork.Time - PhotonNetwork.GetPing() / 2000f) / cycleDuration) - _baseTime);
        }

        //(������ ���� ��) / cycleDuration
        //cycleDuration�� 1�̸� �а�. 1�� 1��
        //cycleDuration�� 10�̸� 1�� 0.1��
        //return (((Time.time - (float)_baseTime) / 60.0f) / cycleDuration) % 1;
    }

    private void UpdateLightValue(float timeOfDay)
    {
        if (currentPeriod != null)
        {
            float timeZoneTime = GetNormalizedDifference(currentPeriod.startTime, currentPeriod.endTime);
            float t = (timeOfDay - currentPeriod.startTime) < 0 ? (timeOfDay - currentPeriod.startTime) + 1 : (timeOfDay - currentPeriod.startTime);

            currentLightValue = Mathf.Lerp(currentPeriod.lightValueStart, currentPeriod.lightValueEnd, t / timeZoneTime);
        }
        _directionalLight.intensity = currentLightValue;
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
        //�����ִ� period
        if (period.startTime > period.endTime)
        {
            //is in time
            return timeOfDay >= period.startTime || timeOfDay < period.endTime;
        }

        //�Ϲ� period

        return (timeOfDay >= period.startTime && timeOfDay < period.endTime);
    }
}