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
        [SerializeField] private List<GameObject> _gameObjectListForDataLoading;
        public List<GameObject> GameObjectListForDataLoading
        {
            get => _gameObjectListForDataLoading;
            set => _gameObjectListForDataLoading = value;
        }

        [SerializeField] private GameObject _playerInstalledObjectsParent;
        public GameObject PlayerInstalledObjectsParent
        {
            get => _playerInstalledObjectsParent;
            set => _playerInstalledObjectsParent = value;
        }

        [SerializeField] private GameObject _enemyInstalledParent;
        public GameObject EnemyInstalledParent
        {
            get => _enemyInstalledParent;
            set => _enemyInstalledParent = value;
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
