using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateNewGame : MonoBehaviour
{
    [SerializeField]
    private GameObject newGamePrefab;
    [SerializeField]
    private Transform ContentParent;
    private string _gameName;
    private string _mapName = "Map A";

    public TMP_Dropdown dropdown;

    private void Start()
    {
        dropdown.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(int value)
    {
        SetMapName(dropdown.options[value].text);
    }


    public void SetGameName(string name)
    {
        _gameName = name;
    }
    public void SetMapName(string mapName)
    {
        _mapName = mapName;
    }
    public void CreateGame_StartCreate()
    {
        MainSceneCanvasManager.Instance.SetActive_CreateCanvas(true);
    }

    public void CreateGame_EndCreate()
    {
        GameObject GamePrefab = Instantiate(newGamePrefab, ContentParent);
        GamePrefab.transform.GetChild(0).GetComponent<TMP_Text>().text = _gameName;
        GamePrefab.GetComponent<ISetSaveGameAddListener>().SetSaveGameAddListener();
        GamePrefab.GetComponent<ISetGamePanelMapName>().SetGamePanelMapName(_mapName);
        GamePrefab.GetComponent<ISetGamePanelGameName>().SetGamePanelGameName(_gameName);
        MainSceneCanvasManager.Instance.SetActive_CreateCanvas(false);
    }

    public void CancelCreateGame()
    {
        _mapName = "Map A";
        _gameName = null;
        MainSceneCanvasManager.Instance.SetActive_CreateCanvas(false);
    }
}