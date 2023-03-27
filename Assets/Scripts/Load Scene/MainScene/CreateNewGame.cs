using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateNewGame : MonoBehaviour
{
    [System.Serializable]
    class MapData
    {
        public string _gameName;
        public string _mapName;
    }
    [SerializeField]
    private GameObject newGamePrefab;
    [SerializeField]
    private Transform ContentParent;
    [SerializeField]
    private GameObject gameSaveManager;
    MapData _mapdata = new MapData();

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
        GameObject GamePrefab = Instantiate(newGamePrefab, ContentParent);
        GamePrefab.transform.GetChild(0).GetComponent<TMP_Text>().text = _mapdata._gameName;
        GamePrefab.GetComponent<ISetSaveGameAddListener>().SetSaveGameAddListener();
        GamePrefab.GetComponent<ISetGamePanelMapName>().SetGamePanelMapName(_mapdata._mapName);
        GamePrefab.GetComponent<ISetGamePanelGameName>().SetGamePanelGameName(_mapdata._gameName);
        MainSceneCanvasManager.Instance.SetActive_CreateCanvas(false);
        SaveCreatedGame();
    }

    private void SaveCreatedGame()
    {
        string filepath = "Assets/Saved/GameInMainScene";
        string filename = $"{_mapdata._gameName}.bin";
        if (gameSaveManager.TryGetComponent(out ISaveData saveData))
        {
            saveData.SaveData(filepath, filename, _mapdata);
        }
    }
    public void CancelCreateGame()
    {
        _mapdata._mapName = "Map A";
        _mapdata._gameName = null;
        MainSceneCanvasManager.Instance.SetActive_CreateCanvas(false);
    }
}