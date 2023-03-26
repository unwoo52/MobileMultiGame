using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ISetGamePanelGameName
{
    void SetGamePanelGameName(string name);
}
public interface ISetGamePanelMapName
{
    void SetGamePanelMapName(string mapName);
}
public interface ISetSaveGameAddListener
{
    void SetSaveGameAddListener();
}

public class SavedGame : MonoBehaviour, ISetGamePanelGameName, ISetGamePanelMapName, ISetSaveGameAddListener
{
    private string _gameName;
    private string _mapName;
    public void SetGamePanelMapName(string mapName)
    {
        _mapName = mapName;
    }

    public void SetGamePanelGameName(string gameName)
    {
        _gameName = gameName;
    }

    public void SetSaveGameAddListener()
    {
        Button button = GetComponent<Button>();

        if (button != null)
        {
            // On Click 이벤트에 대한 설정을 합니다.
            button.onClick.AddListener(Connet);
        }
    }
    public void Connet()
    {
        MainSceneCanvasManager.Instance.Connect(_gameName, _mapName);
    }
}
