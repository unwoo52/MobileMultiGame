using System;
using System.Collections.Generic;
using UnityEngine;

public interface IAllGameDataSave
{
    bool AllGameDataSave();
}
public interface IAllGameDataLoad
{
    long AllGameDataLoad();
}
public interface ILoadGameData
{
    bool LoadGameData(List<object> datalist);
}
public interface ISaveGameData<T> where T : class
{
    bool SaveGameData(out T data);
}

public class GameDataManager : MonoBehaviour, IAllGameDataSave, IAllGameDataLoad
{
    #region singleton
    private static GameDataManager _instance = null;

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
    public static GameDataManager Instance
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

    public string gamename, mapname;
    [SerializeField] private List<GameObject> GameObjectListForDataLoading;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    /// <summary>
    /// 게임의 데이터를 로드합니다. 반환하는 flag가 0이면 모든 데이터의 정상 로드, 1이면 새로 생성한 게임이여서 로드할 데이터가 없다는 뜻,
    /// 이 외의 비트는 각각 실패한 데이터를 의미합니다. 
    /// </summary>
    public long AllGameDataLoad()
    {
        Dictionary<string, byte> loadDataFlag = new Dictionary<string, byte>()
        {
            {"Player Installed Objects Parent", 2},
            {"enemy Data", 3},
            {"player Data", 4},
            {"time Data", 5},
            {"none1", 6},
            {"none2", 7}
        };
        byte failFlag = 0;
        List<object> data = new();

        //data load
        if (!DataSaveAndLoad.LoadToJson(ref data, Application.dataPath + "/Saved/GameData", gamename))
        {
            Debug.Log("No data to load as this is the first time the game is being created.");
            failFlag = 0x1;
            return failFlag;
        }

            //각각의 데이터 로드 실행
        //건물 데이터 로드...
        foreach(GameObject obj in GameObjectListForDataLoading)
        {
            if (!LoadDataAtObject(obj, data))
                Debug.LogError($"Failed to load data for {obj.name}.");
        }

        //플레이어 데이터 로드...
        //적 데이터 로드...
        //아이템 데이터 로드...


        FlagTool.PrintFailedData(loadDataFlag, failFlag);
        return failFlag;
    }

    public bool AllGameDataSave()
    {
        bool temp = true;/*
        //TotalData 인스턴스 생성
        TotalDataWrapper totalDataWrapper = new();
        totalDataWrapper._buildObjcetDataWrapper = new();

        //인스턴스들에게 값 넣기
        //get parents from InGameManaer...
        if (InGameManager.Instance.PlayerInstalledObjectsParent.TryGetComponent(out ISaveGameData<BuildObjcetDataWrapper> saveGameData))
        {
            if(!saveGameData.SaveGameData(out totalDataWrapper._buildObjcetDataWrapper)) temp = false;
        }
        else temp = false;

        //모인 데이터들을 취합이 끝난 뒤, 세이브
        DataSaveAndLoad.SaveToJson(totalDataWrapper, Application.dataPath + "/Saved/GameData", gamename);
        */
        return temp;
    }

    /*codes*/

    /// <summary>
    /// 오브젝트의 ILoadGameData 인터페이스에 data를 전달합니다.
    /// 오브젝트에 인터페이스가 없거나, 실행된 인터페이스 함수가 fail를 리턴하면 fail을 리턴합니다.
    /// </summary>
    private bool LoadDataAtObject(GameObject parentobject, List<object> data)
    {
        if (!parentobject.TryGetComponent(out ILoadGameData loadGameData)) return false;

        return loadGameData.LoadGameData(data);
    }
}
