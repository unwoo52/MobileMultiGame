using Photon.Pun;
using UnityEngine;

namespace MyNamespace
{
    public class MapInitialize : MonoBehaviour
    {

        #region singleton
        private static MapInitialize _instance = null;
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
        public static MapInitialize Instance
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

        [SerializeField] private GameObject _InitManagement;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Vector3 _playerSpawnPostion;

        private void Start()
        {
            GameInit();
            //if (!GameInit()) LeaveRoom(); << 테스트중엔 사용하지 않을 코드. LeaveRoom의 역할은 IsPhotonConnect? LeaveRoom : LoadScene(0);
            MapDataLoad();

            CreatePlayer();

            if (PhotonNetwork.IsMasterClient)
            {
                Destroy(gameObject);

            }
        }
        private bool GameInit()
        {
            if (!PhotonNetwork.IsConnected)
            {
                if (!GameInit_Single())
                {
                    Debug.LogError("GameInit_Single method fail.");
                    return false;
                }
            }
            else
            {
                if (!GameInit_Multi())
                {
                    Debug.LogError("GameInit_Multi method fail.");
                    return false;
                }
            }

            return true;
        }
        private bool GameInit_Multi()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return true;
            }

            try
            {
                GameObject _initobject = PhotonNetwork.Instantiate(_InitManagement.name, Vector3.zero, Quaternion.identity, 0);
                _initobject.transform.SetParent(transform.parent, false);
            }
            catch
            {
                Debug.LogError("GameInit_Multi method fail.");
                return false;
            }
            
            return true;
        }

        private bool GameInit_Single()
        {
            GameObject _initobject = Instantiate(_InitManagement);
            
            return true;
        }


        private bool MapDataLoad()
        {
            if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient) { return false; }

            GameObject _gameDataManager = GameDataManager.Instance.gameObject;

            if (!_gameDataManager.TryGetComponent(out IAllGameDataLoad loadGameData)) return false;
            long flag = loadGameData.AllGameDataLoad();

            //0x1 비트면, 새로 생성된 게임이므로 로드할 데이터가 없기 때문에 true를 리턴
            if (flag == 0x1)
            {
                Debug.Log("No data to load as this is the first time the game is being created.");
                return true;
            }
            if (flag == 0) return true;
            else
            {
                Debug.LogError("MapDataLoad method fail.");
                return false;
            }
        }

        private bool CreatePlayer()
        {

            if (!PhotonNetwork.IsConnected)
            {
                if (!CreatePlater_Single())
                {
                    Debug.LogError("CreatePlater_Single method fail.");
                    return false;
                }
            }
            else
            {
                if (!CreatePlater_Multi())
                {
                    Debug.LogError("CreatePlater_Multi method fail.");
                    return false;
                }
            }
            
            return true;
        }
        private bool CreatePlater_Single()
        {
            Instantiate(playerPrefab, _playerSpawnPostion, Quaternion.identity);
            return true;
        }
        private bool CreatePlater_Multi()
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
            return true;
        }
    }
}

