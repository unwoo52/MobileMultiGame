using System;
using UnityEngine;

public interface IAllGameDataSave
{
    bool AllGameDataSave();
}
public interface IAllGameDataLoad
{
    bool AllGameDataLoad();
}
public interface ILoadGameData<T> where T : class
{
    bool LoadGameData(T data);
}
public interface ISaveGameData<T> where T : class
{
    bool SaveGameData(out T data);
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
    public bool AllGameDataLoad()
    {
        bool temp = true;

        //load Total totalWrapper
        TotalDataWrapper totalDataWrapper = new();
        DataSaveAndLoad.LoadToJson(out totalDataWrapper, Application.dataPath + "/Saved/GameData", gamename); // ���⼭ false�� �߻��Ǹ�, ������ ������ '������ ó�� �����ؼ�' �ϳ� ��
        //�߰���, ������ �������ڸ��� ���丮�� ���̺������� �ٷ� �ѹ��� �����ϰ� �����.

        //get parents from InGameManaer...
        //�������̽��� �� parents���Լ� bool LoadMethod(gamename) ����
        LoadDataAtObject(InGameManager.Instance.PlayerInstalledObjectsParent, totalDataWrapper._buildObjcetDataWrapper);
        //�÷��̾� ������ �ε�...
        //�� ������ �ε�...
        //������ ������ �ε�...

        //������ false�� �߻��Ǹ� �����͸� load�ϴ� �� ������ �߻��� ��
        //���߿� HRESULTó�� 1��Ʈ¥�� ����ü�� �����ϱ�


        return temp;
    }

    /// <summary> ������Ʈ�� ���� �ִ� ILoadGameData�������̽��� data�� �ε��ϰ� �մϴ�. </summary>
    private bool LoadDataAtObject<T>(GameObject parentobject, T data) where T : class
    {
        bool temp = true;

        if (!parentobject.TryGetComponent(out ILoadGameData<T> loadGameData)) temp = false;
        loadGameData.LoadGameData(data);

        return temp;
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
}
