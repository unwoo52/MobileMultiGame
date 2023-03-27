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
        //CreateGame();
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
        MainSceneCanvasManager.Instance.SetActive_CreateCanvas(true);
    }

    public void CreateGame_EndCreate()
    {
        CreateGame(_mapdata._gameName, _mapdata._mapName);
    }
    private void CreateGame(string gamename, string mapname)
    {
        GameObject GamePrefab = Instantiate(newGamePrefab, ContentParent);
        GamePrefab.GetComponent<ISetMapdata>().SetMapdata(gamename, mapname);
        MainSceneCanvasManager.Instance.SetActive_CreateCanvas(false);
        SaveCreatedGame();
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
        MainSceneCanvasManager.Instance.SetActive_CreateCanvas(false);
    }
}