using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> gameObjects;

    public void ToggleInGameOptionSettingCanvas()
    {
        GameObject targetObject = gameObjects.Find(obj => obj.name == "In Game Option Setting Canvas");

        if (targetObject != null)
        {
            targetObject.SetActive(!targetObject.activeSelf);
        }
    }
}
