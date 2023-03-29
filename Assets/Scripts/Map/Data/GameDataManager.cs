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
        DataSaveAndLoad.LoadToJson(out totalDataWrapper, Application.dataPath + "/Saved/GameData", gamename); // 여기서 false가 발생되면, 원인은 무조건 '게임을 처음 시작해서' 하나 뿐
        //추가로, 게임이 생성되자마자 디렉토리와 세이브파일을 바로 한번은 생성하게 만들기.

        //get parents from InGameManaer...
        //인터페이스로 각 parents에게서 bool LoadMethod(gamename) 실행
        LoadDataAtObject(InGameManager.Instance.PlayerInstalledObjectsParent, totalDataWrapper._buildObjcetDataWrapper);
        //플레이어 데이터 로드...
        //적 데이터 로드...
        //아이템 데이터 로드...

        //위에서 false가 발생되면 데이터를 load하던 중 문제가 발생한 것
        //나중에 HRESULT처럼 1비트짜리 구조체로 구현하기


        return temp;
    }

    /// <summary> 오브젝트가 갖고 있는 ILoadGameData인터페이스에 data를 로드하게 합니다. </summary>
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

        return temp;
    }
}
