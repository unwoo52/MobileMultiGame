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
public interface ILoadGameData<T> where T : class
{
    bool LoadGameData(T data);
}
public interface ISaveGameData<T> where T : class
{
    bool SaveGameData(out T data);
}
public class FlagDictionary
{
    public static Dictionary<string, int> loadDataFlag = new Dictionary<string, int>()
    {
        {"buildObject", 1},
        {"enemy", 2},
        {"player", 3},
        {"time", 4}
    };
}

[System.Serializable]
public class TotalDataWrapper
{
    public BuildObjcetDataWrapper _buildObjcetDataWrapper;
    //playerData...
    //EnemyDataWrapper...
    //timeDataWrapper...
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

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }


    [ContextMenu("!!!Load")]
    public long AllGameDataLoad()
    {
        long flag = 0L;

        //load Total totalWrapper
        TotalDataWrapper totalDataWrapper = new();

        if (!DataSaveAndLoad.LoadToJson(ref totalDataWrapper, Application.dataPath + "/Saved/GameData", gamename))
        {
            flag = 0x1;
            return flag;
        }

            //������ ������ �ε� ����
        //�ǹ� ������ �ε�...
        if (!LoadDataAtObject(InGameManager.Instance.PlayerInstalledObjectsParent, totalDataWrapper._buildObjcetDataWrapper)) 
            SetFlag(ref flag, "buildObject", false);
        //�÷��̾� ������ �ε�...
        //�� ������ �ε�...
        //������ ������ �ε�...


        PrintFailedData(FlagDictionary.loadDataFlag, flag);
        return flag;
    }

    /// <summary> ������Ʈ�� ���� �ִ� ILoadGameData�������̽��� data�� �ε��ϰ� �մϴ�. </summary>
    private bool LoadDataAtObject<T>(GameObject parentobject, T data) where T : class
    {
        if (!parentobject.TryGetComponent(out ILoadGameData<T> loadGameData)) return false;

        return loadGameData.LoadGameData(data);
    }

    /// <summary>
    /// flag ��Ʈ�� �����Ǵ� ��ųʸ��� flag��Ʈ�� �ް�, false ��Ʈ�� Ȱ��ȭ �Ǿ��ִٸ� ���� �α׸� ����ϰ� false�� ��ȯ�մϴ�.
    /// �׷��� �ʰ�, ��� flag�� true�̸� �α׸� ������� �ʰ� true�� �����մϴ�.
    /// </summary>
    private bool PrintFailedData<T>(T t, long flag) where T : IDictionary<string, int>
    {
        List<string> failedData = new List<string>();

        // �÷��׸� ��Ʈ �����Ͽ� ������ �����͸� ����Ʈ�� �߰��մϴ�.
        foreach (var pair in t)
        {
            if ((flag & (1L << pair.Value)) != 0)
                failedData.Add(pair.Key);
        }

        // ������ ������ �̸��� �̿��Ͽ� �α׸� ����մϴ�.
        if (failedData.Count > 0)
        {
            string dataNames = string.Join(", ", failedData.ToArray());
            string message = $"{dataNames} �������� �ε忡 �����߽��ϴ�.";
            Debug.LogError(message);
            return false;
        }
        else return true;
    }


    [ContextMenu("!!!Save")]
    public bool AllGameDataSave()
    {
        bool temp = true;
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

        return temp;
    }

    /*codes*/
    private bool SetFlag(ref long flag, string Indexname, bool value)
    {
        if (!FlagDictionary.loadDataFlag.TryGetValue(name, out int bitIndex))
        {
            Debug.LogError($"{name} �����Ϳ� ���� �÷��� ��Ʈ�� ã�� �� �����ϴ�.");
            return false;
        }

        if (value)
        {
            flag |= (1L << bitIndex);  // ��Ʈ�� 1�� ����
        }
        else
        {
            flag &= ~(1L << bitIndex); // ��Ʈ�� 0���� ����
        }

        return true;
    }
}
