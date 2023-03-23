using System.Collections.Generic;
using UnityEngine;

public class MainSceneCanvasManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> scenes;

    private void Awake()
    {
        GoToDefaultScene();
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