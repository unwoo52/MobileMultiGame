using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoadManagement : MonoBehaviour
{
    #region singleton
    private static GameLoadManagement _instance = null;

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
    public static GameLoadManagement Instance
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
    public string gamename;
    public string mapName;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
