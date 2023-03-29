using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviourPunCallbacks
{
    #region singleton
    private static InGameManager _instance = null;

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
    public static InGameManager Instance
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
    [SerializeField] private GameObject _playerInstalledObjectsParent;
    public GameObject PlayerInstalledObjectsParent => _playerInstalledObjectsParent;
    [SerializeField] private GameObject EnemyInstalledParent;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerSpawner;

    [SerializeField] private GameObject _gameDataManager;

    private void Start()
    {
        _gameDataManager = GameDataManager.Instance.gameObject;
        CreatePlayer();
        //MapDataLoad();
    }

    private bool MapDataLoad()
    {
        if (!_gameDataManager.TryGetComponent(out IAllGameDataSave loadGameData)) return false;
        if (!loadGameData.AllGameDataSave()) return false;        

        return true;
    }
    private void CreatePlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            if (PlayerManager.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                PhotonNetwork.Instantiate(this.playerPrefab.name, playerSpawner.transform.position, Quaternion.identity, 0);
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }
    }

    [ContextMenu("Do Save Buildings")]
    public void SaveBuilding()
    {
        //enemy data save
        //instante building save
    }

    public Transform GetEnemyInstalledParent()
    {
        return EnemyInstalledParent.transform;
    }


    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
