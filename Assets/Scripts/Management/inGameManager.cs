using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyNamespace
{
    public class InGameManager : MonoBehaviourPunCallbacks
    {
        #region singleton
        private static InGameManager _instance = null;
        [SerializeField]
        private List<GameObject> _gameObjectListForDataLoading;
        public List<GameObject> GameObjectListForDataLoading => _gameObjectListForDataLoading;
        [SerializeField] private Vector3 _playerSpawnPostion;
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
        [SerializeField] private GameObject _enemyInstalledParent;
        public GameObject EnemyInstalledParent => _enemyInstalledParent;

        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject _gameDataManager;

        private void Start()
        {
            _gameDataManager = GameDataManager.Instance.gameObject;
            CreatePlayer();
            MapDataLoad();
        }

        private bool MapDataLoad()
        {
            if (!_gameDataManager.TryGetComponent(out IAllGameDataLoad loadGameData)) return false;
            long flag = loadGameData.AllGameDataLoad();

            //0x1 비트면, 새로 생성된 게임이므로 로드할 데이터가 없기 때문에 true를 리턴
            if (flag == 0x1)
            {
                Debug.Log("No data to load as this is the first time the game is being created.");
                return true;
            }
            if (flag == 0) return true;

            else return false;
        }

        private void CreatePlayer()
        {
            if (!PhotonNetwork.IsConnected)
            {
                Instantiate(playerPrefab, _playerSpawnPostion, Quaternion.identity);
            }
            else
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
                        PhotonNetwork.Instantiate(this.playerPrefab.name, _playerSpawnPostion, Quaternion.identity, 0);
                    }
                    else
                    {
                        Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                    }
                }
            }            
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
}
