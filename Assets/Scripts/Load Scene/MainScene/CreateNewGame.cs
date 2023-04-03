using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace MyNamespace
{
    [System.Serializable]
    public class MapData
    {
        public string _gameName;
        public string _mapName;
    }
    public class CreateNewGame : MonoBehaviour
    {

        [SerializeField]
        private GameObject _CreateGameCanvas;
        [SerializeField]
        private GameObject newGamePrefab;
        [SerializeField]
        private Transform ContentParent;
        [SerializeField]
        private GameObject gameSaveManager;
        private MapData _mapdata;
        [SerializeField]
        private string _savegamePath;
        public TMP_Dropdown dropdown;

        private void Start()
        {
            _savegamePath = Application.dataPath + "/Saved/GameData";
            dropdown.onValueChanged.AddListener(OnValueChanged);
            InitMapdata();
            CheckGameSaveManagerIsEmpty();
            LoadSaveGames();
        }
        private void InitMapdata()
        {
            _mapdata = new MapData();
            _mapdata._mapName = "Map A";
        }
        private void CheckGameSaveManagerIsEmpty()
        {
            if (gameSaveManager == null)
            {
                GameObject gameSaveManagerObject = GameObject.Find("GameSaveManager");
                if (gameSaveManagerObject != null)
                {
                    gameSaveManager = gameSaveManagerObject;
                }
            }
        }
        private void LoadSaveGames()
        {
            List<TotalDataClass> list = DataSaveAndLoad.LoadJsonFilesInDirectory<TotalDataClass>(_savegamePath);

            foreach (var mapdata in list)
            {
                if(mapdata == null)
                {
                    Debug.LogError($"The map data is null : {mapdata}");
                    continue;
                }
                if (mapdata._gameName == null)
                {
                    Debug.LogError($"The map data does not have a variable named mapdata._gameName : {mapdata}");
                    continue;
                }

                try
                {
                    CreateGame(mapdata._gameName, mapdata._mapName);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"fail load : {e}");
                }
            }
        }

        private void OnValueChanged(int value)
        {
            SetMapName(dropdown.options[value].text);
        }


        public void SetGameName(string name)
        {
            _mapdata._gameName = name;
        }
        public void SetMapName(string mapName)
        {
            _mapdata._mapName = mapName;
        }
        public void CreateGame_StartCreate()
        {
            SetActiveCreateCanvas(true);
        }
        private bool SetActiveCreateCanvas(bool isActive)
        {
            if (_CreateGameCanvas != null && _CreateGameCanvas.TryGetComponent(out ISetActive_CreateCanvas setActive_CreateCanvas))
            {
                setActive_CreateCanvas.SetActive_CreateCanvas(isActive);
            }
            else if (_CreateGameCanvas == null)
            {
                MainSceneCanvasManager.Instance.SetActive_CreateCanvas(isActive);
            }
            else return false;

            return true;
        }
        public void CreateGame_EndCreate()
        {
            CreateGame(_mapdata._gameName, _mapdata._mapName);
        }
        private bool CreateGame(string gamename, string mapname)
        {
            if(!CreateGamePrefab(gamename, mapname)) return false;
            SaveCreatedGame();
            SetActiveCreateCanvas(false);

            return true;
        }
        private bool CreateGamePrefab(string gamename, string mapname)
        {
            GameObject GamePrefab = Instantiate(newGamePrefab, ContentParent);
            if (GamePrefab == null) return false;

            if (GamePrefab.TryGetComponent(out ISetMapdata isetMapdata))
            {
                isetMapdata.SetMapdata(gamename, mapname);
            }
            else return false;

            return true;
        }
        private void SaveCreatedGame()
        {
            TotalDataClass test = new();
            test._gameName = _mapdata._gameName;
            test._mapName = _mapdata._mapName;
            DataSaveAndLoad.SaveToJson(test, Application.dataPath + "/Saved/GameData", _mapdata._gameName);
        }
        public void CancelCreateGame()
        {
            _mapdata._mapName = "Map A";
            _mapdata._gameName = null;
            SetActiveCreateCanvas(false);
        }
    }
}