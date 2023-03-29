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
public interface ISaveGameData
{
    bool SaveGameData(string gamename);
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


    public bool AllGameDataLoad()
    {
        //load Total AlldataWrapper
        //AlldataWrapper.BuildWrapper


        //get parents from InGameManaer...



        //GameObject _playerInstalledObjectsManagement = inGameManager.Instance.PlayerInstalledObjectsManagement;
        //_playerInstalledObjectsManagement.getcomponent(interface LoadData);

        string data;

        DataSaveAndLoad.LoadFileData(out data, Application.dataPath + "/Saved/GameData/", gamename);

        if (!DataSaveAndLoad.LoadJson(out TotalDataWrapper json, data)) return false;
        //PlayerInstalledObjectsManagement.loadData(json._buildObjcetDataWrapper);


        //�������̽��� �� parents���Լ� bool LoadMethod(gamename) ����
        return true;
    }


    public bool AllGameDataSave()
    {
        //get parents from InGameManaer...

        //�������̽��� �� parents���Լ� Save ����

        //�� parent���� ������ ������ ���°� return

        //���� �����͵��� ����
        TotalDataWrapper totalDataWrapper;

        //save totalDataWrapper�� json����

        return true;
    }
}
