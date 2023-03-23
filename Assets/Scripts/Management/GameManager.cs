using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGetInstallObjectsParent
{
    Transform GetInstallObjectsParent();
}
public class GameManager : MonoBehaviour, IGetInstallObjectsParent
{
    #region singleton
    private static GameManager _instance = null;

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
    public static GameManager Instance
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
    [SerializeField] private GameObject InstalledObjectsParent;
    private void Start()
    {
        if (!Initialize())
        {
            Debug.LogError("Init() 실패! 컴포넌트를 찾지 못했습니다.");
        }
    }
    private bool Initialize()
    {
        if(!FindOnject(ref InstalledObjectsParent, "Installed Objects Parent")) return false;

        return true;
    }

    private bool FindOnject(ref GameObject gameobject, string findName)
    {
        if (gameobject != null) return true;
        if (transform.Find(findName) == null) return false;

        gameobject = transform.Find(findName).gameObject;

        return true;
    }
    public Transform GetInstallObjectsParent()
    {
        return InstalledObjectsParent.transform;
    }
}
