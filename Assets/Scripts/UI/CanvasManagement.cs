using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManagement : MonoBehaviour, IGetCanvas
{
    #region singleton
    private static CanvasManagement _instance = null;

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
    public static CanvasManagement Instance
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
    [SerializeField] private GameObject _inventory;
    [SerializeField] private GameObject _quickSlot;
    [SerializeField] private GameObject _variableJoystick;
    [SerializeField] private GameObject _canvas;
    public GameObject Inventory { get { return _inventory; } }
    public bool GetCanvasGameObject(ref GameObject gameObject)
    {
        gameObject = _canvas;
        if (gameObject == null) gameObject = transform.parent.gameObject;
        return gameObject != null;
    }
}