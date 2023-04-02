using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyNamespace
{
    public interface IAllGameDataSave
    {
        bool AllGameDataSave();
    }
    public interface IAllGameDataLoad
    {
        byte AllGameDataLoad();
    }
    public interface ILoadGameData
    {
        bool LoadGameData(string datalist);
    }
    public interface ISaveGameData
    {
        bool SaveGameData(out string data);
    }

    [Serializable]
    class TotalDataClass
    {
        [SerializeField] public string[] jsondatastring;
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

        [ContextMenu("!!!!!!Load")]
        public byte AllGameDataLoad()
        {
            //GameObjectListForDataLoading의 게임 오브젝트 순서와, 데이터 저장되는 순서는 동일해야 함
            Dictionary<string, byte> loadDataFlag = new()
            {
                {"Player Installed Objects Parent", 0}
            };
            byte failFlag = 0;
            TotalDataClass data = new();

            //data load
            try
            {
                DataSaveAndLoad.LoadToJson(ref data, Application.dataPath + "/Saved/GameData", gamename);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load data for {e}.");
            }

            List<GameObject> GameObjectListForDataLoading = InGameManager.Instance.GameObjectListForDataLoading;
            //각각의 데이터 로드 실행
            //건물 데이터 로드...
            /*
            foreach (GameObject obj in GameObjectListForDataLoading)
            {
                if (!LoadDataToGameObject(obj, data))
                {
                    Debug.LogError($"Failed to load data for {obj.name}.");

                    //set fail flag
                    if(loadDataFlag.TryGetValue(mapname, out byte temp)){
                        FlagTool.SetBit(ref failFlag, temp, true);
                    }
                }
            }*/

            int temp = 0;
            foreach(var str in data.jsondatastring)
            {
                GameObject gameObject = GameObjectListForDataLoading[temp];
                if(gameObject.TryGetComponent(out ILoadGameData loadGameData))
                {
                    loadGameData.LoadGameData(str);
                }
                else
                {
                    Debug.LogError($"Failed to load data for {gameObject.name}.");

                    //set fail flag
                    if (loadDataFlag.TryGetValue(mapname, out byte tempbyte))
                    {
                        FlagTool.SetBit(ref failFlag, temp, true);
                    }
                }
                temp++;
            }

            FlagTool.PrintFailedData(loadDataFlag, failFlag);
            return failFlag;
        }

        [ContextMenu("!!!!!!SAVE")]
        public bool AllGameDataSave()
        {
            bool temp = true;
            //TotalData 인스턴스 생성
            List<string> data = new();

            //인스턴스들에게 값 넣기
            List<GameObject> GameObjectListForDataLoading = InGameManager.Instance.GameObjectListForDataLoading;
            foreach (GameObject obj in GameObjectListForDataLoading)
            {
                if (obj.TryGetComponent(out ISaveGameData saveGameData))
                {
                    saveGameData.SaveGameData(out string objectdata);

                    data.Add(objectdata);
                }
            }

            TotalDataClass test = new();
            test.jsondatastring = data.ToArray();

            Debug.Log(JsonUtility.ToJson(test));

            //모인 데이터들을 취합이 끝난 뒤, 세이브
            DataSaveAndLoad.SaveToJson(test, Application.dataPath + "/Saved/GameData", gamename);

            return temp;
        }
    }
}
