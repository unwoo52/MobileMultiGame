using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
    private MapData _mapdata = new MapData();
    [SerializeField]
    private string _savegamePath = "Assets/Saved/GameInMainScene";
    public TMP_Dropdown dropdown;

    private void Start()
    {
        dropdown.onValueChanged.AddListener(OnValueChanged);
        _mapdata._mapName = "Map A";
        if(gameSaveManager == null)
        {
            GameObject gameSaveManagerObject = GameObject.Find("GameSaveManager");
            if (gameSaveManagerObject != null)
            {
                gameSaveManager = gameSaveManagerObject;
            }
        }
        LoadSaveGame();
    }
    private void LoadSaveGame()
    {
        List<MapData> list;
        if (gameSaveManager.TryGetComponent(out ILoadDataAtDirectory loadDataAtDirectory))
        {
            list = loadDataAtDirectory.LoadDataAtDirectory<MapData>(_savegamePath);
        }
        else return;

        foreach(MapData mapdata in list)
        {
            CreateGame(mapdata._gameName, mapdata._mapName);
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
        GameObject GamePrefab = Instantiate(newGamePrefab, ContentParent);
        if (GamePrefab == null) return false;

        if (GamePrefab.TryGetComponent(out ISetMapdata isetMapdata))
        {
            isetMapdata.SetMapdata(gamename, mapname);
        }
        else return false;


        SetActiveCreateCanvas(false);
        SaveCreatedGame();

        return true;
    }

    private void SaveCreatedGame()
    {
        string filename = $"{_mapdata._gameName}.bin";
        if (gameSaveManager.TryGetComponent(out ISaveData saveData))
        {
            saveData.SaveData(_savegamePath, filename, _mapdata);
        }
    }
    public void CancelCreateGame()
    {
        _mapdata._mapName = "Map A";
        _mapdata._gameName = null;
        SetActiveCreateCanvas(false);
    }
}