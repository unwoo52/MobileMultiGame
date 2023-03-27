using System.Collections.Generic;
using UnityEngine;
public interface ISetActive_CreateCanvas
{
    void SetActive_CreateCanvas(bool param);
}
public class MainSceneCanvasManager : MonoBehaviour, ISetActive_CreateCanvas
{
    #region singleton
    private static MainSceneCanvasManager _instance = null;

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
        GoToDefaultScene();
    }
    public static MainSceneCanvasManager Instance
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
    [SerializeField] private List<GameObject> scenes;
    [SerializeField] private GameObject _createCanvas;
    public void SetActive_CreateCanvas(bool isActive)
    {
        _createCanvas.SetActive(isActive);
    }

    public void Connect(string gamename, string mapname)
    {
        GetComponent<GameJoinManager>().Connect(gamename, mapname);
    }


    public void GoToGameStartScene()
    {
        ChangeScene("Game Start Menu Canvs");
    }

    public void GoToStoreScene()
    {
        ChangeScene("Store Canvas");
    }

    public void GoToOptionScene()
    {
        ChangeScene("Option Canvas");
    }

    public void GoToQuitGameScene()
    {
        ChangeScene("Quit Canvas");
    }

    public void GoToDefaultScene()
    {
        ChangeScene("Main Menu Canvas");
    }
    public void BackButton()
    {
        GoToDefaultScene();
    }

    private void ChangeScene(string sceneName)
    {
        foreach (GameObject sceneObject in scenes)
        {
            bool isActive = sceneObject.name == sceneName;
            sceneObject.SetActive(isActive);
        }
    }

}