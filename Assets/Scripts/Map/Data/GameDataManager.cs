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

            //각각의 데이터 로드 실행
        //건물 데이터 로드...
        if (!LoadDataAtObject(InGameManager.Instance.PlayerInstalledObjectsParent, totalDataWrapper._buildObjcetDataWrapper)) 
            SetFlag(ref flag, "buildObject", false);
        //플레이어 데이터 로드...
        //적 데이터 로드...
        //아이템 데이터 로드...


        PrintFailedData(FlagDictionary.loadDataFlag, flag);
        return flag;
    }

    /// <summary> 오브젝트가 갖고 있는 ILoadGameData인터페이스에 data를 로드하게 합니다. </summary>
    private bool LoadDataAtObject<T>(GameObject parentobject, T data) where T : class
    {
        if (!parentobject.TryGetComponent(out ILoadGameData<T> loadGameData)) return false;

        return loadGameData.LoadGameData(data);
    }

    /// <summary>
    /// flag 비트와 대응되는 딕셔너리와 flag비트를 받고, false 비트가 활성화 되어있다면 오류 로그를 출력하고 false를 반환합니다.
    /// 그렇지 않고, 모든 flag가 true이면 로그를 출력하지 않고 true를 리턴합니다.
    /// </summary>
    private bool PrintFailedData<T>(T t, long flag) where T : IDictionary<string, int>
    {
        List<string> failedData = new List<string>();

        // 플래그를 비트 연산하여 실패한 데이터를 리스트에 추가합니다.
        foreach (var pair in t)
        {
            if ((flag & (1L << pair.Value)) != 0)
                failedData.Add(pair.Key);
        }

        // 실패한 데이터 이름을 이용하여 로그를 출력합니다.
        if (failedData.Count > 0)
        {
            string dataNames = string.Join(", ", failedData.ToArray());
            string message = $"{dataNames} 데이터의 로드에 실패했습니다.";
            Debug.LogError(message);
            return false;
        }
        else return true;
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

    /*codes*/
    private bool SetFlag(ref long flag, string Indexname, bool value)
    {
        if (!FlagDictionary.loadDataFlag.TryGetValue(name, out int bitIndex))
        {
            Debug.LogError($"{name} 데이터에 대한 플래그 비트를 찾을 수 없습니다.");
            return false;
        }

        if (value)
        {
            flag |= (1L << bitIndex);  // 비트를 1로 설정
        }
        else
        {
            flag &= ~(1L << bitIndex); // 비트를 0으로 설정
        }

        return true;
    }
}
