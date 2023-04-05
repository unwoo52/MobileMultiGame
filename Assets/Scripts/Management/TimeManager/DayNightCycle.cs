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
    [SerializeField] private Light _directionalLight;

    public Light DirectionalLight
    {
        get => _directionalLight;
        set => _directionalLight = value;
    }
    [SerializeField] List<TimePeriod> timePeriods = new List<TimePeriod>();

    //하루를 cycleDuration분으로 정의합니다. cycleDuration이 n이면 하루는 n분동안 진행됩니다.
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
            // 새로운 플레이어에게 현재의 _baseTime을 전달합니다.
            photonView.RPC(nameof(SetBaseTime), newPlayer, _baseTime);
        }
    }
    [SerializeField] private TMP_Text _baseTimeText;
    [PunRPC]
    private void SetBaseTime(double baseTime)
    {
        _baseTime = baseTime;
    }

    // 게임 시간을 반환합니다.
    public float GetGameTime()
    {
        // Master Client에서는 PhotonNetwork.Time에서 방 생성 시간을 빼서 게임 시간을 구합니다.
        if (PhotonNetwork.IsMasterClient)
        {
            return (float)(PhotonNetwork.Time - _baseTime);
        }
        // 다른 플레이어들은 Master Client로부터 전달받은 방 생성 시간을 사용하여 게임 시간을 구합니다.
        else
        {
            return (float)((PhotonNetwork.Time - PhotonNetwork.GetPing() / 2000f) - _baseTime);
        }
    }
    #endregion

    private void Update()
    {
        //현재 시간대 정의 (아침 낮 밤 등..)
        currentPeriod = GetCurrentTimePeriod();

        //시간 비율 timeOfDay에 반환. ex)12시정각이면 0.5 반환, 하루의 시작이면 0 반환
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

    /// <summary>현재 시간을 하루의 비율(0~1)로 표현한 값을 반환합니다. 예를 들어 하루의 중간이라면 0.5를 반환합니다.</summary>
    private float GetTimeofDay(float cycleDuration)
    {

        // Master Client에서는 PhotonNetwork.Time에서 방 생성 시간을 빼서 게임 시간을 구합니다.
        if (PhotonNetwork.IsMasterClient)
        {
            return (float)((PhotonNetwork.Time - _baseTime) / cycleDuration);
        }
        // 다른 플레이어들은 Master Client로부터 전달받은 방 생성 시간을 사용하여 게임 시간을 구합니다.
        else
        {
            return (float)(((PhotonNetwork.Time - PhotonNetwork.GetPing() / 2000f) / cycleDuration) - _baseTime);
        }

        //(분으로 나눈 값) / cycleDuration
        //cycleDuration이 1이면 분값. 1은 1분
        //cycleDuration이 10이면 1은 0.1분
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