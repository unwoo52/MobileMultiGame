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
    /// ������ �����͸� �ε��մϴ�. ��ȯ�ϴ� flag�� 0�̸� ��� �������� ���� �ε�, 1�̸� ���� ������ �����̿��� �ε��� �����Ͱ� ���ٴ� ��,
    /// �� ���� ��Ʈ�� ���� ������ �����͸� �ǹ��մϴ�. 
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

            //������ ������ �ε� ����
        //�ǹ� ������ �ε�...
        foreach(GameObject obj in GameObjectListForDataLoading)
        {
            if (!LoadDataAtObject(obj, data))
                Debug.LogError($"Failed to load data for {obj.name}.");
        }

        //�÷��̾� ������ �ε�...
        //�� ������ �ε�...
        //������ ������ �ε�...


        FlagTool.PrintFailedData(loadDataFlag, failFlag);
        return failFlag;
    }

    public bool AllGameDataSave()
    {
        bool temp = true;/*
        //TotalData �ν��Ͻ� ����
        TotalDataWrapper totalDataWrapper = new();
        totalDataWrapper._buildObjcetDataWrapper = new();

        //�ν��Ͻ��鿡�� �� �ֱ�
        //get parents from InGameManaer...
        if (InGameManager.Instance.PlayerInstalledObjectsParent.TryGetComponent(out ISaveGameData<BuildObjcetDataWrapper> saveGameData))
        {
            if(!saveGameData.SaveGameData(out totalDataWrapper._buildObjcetDataWrapper)) temp = false;
        }
        else temp = false;

        //���� �����͵��� ������ ���� ��, ���̺�
        DataSaveAndLoad.SaveToJson(totalDataWrapper, Application.dataPath + "/Saved/GameData", gamename);
        */
        return temp;
    }

    /*codes*/

    /// <summary>
    /// ������Ʈ�� ILoadGameData �������̽��� data�� �����մϴ�.
    /// ������Ʈ�� �������̽��� ���ų�, ����� �������̽� �Լ��� fail�� �����ϸ� fail�� �����մϴ�.
    /// </summary>
    private bool LoadDataAtObject(GameObject parentobject, List<object> data)
    {
        if (!parentobject.TryGetComponent(out ILoadGameData loadGameData)) return false;

        return loadGameData.LoadGameData(data);
    }
}
