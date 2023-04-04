using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyNamespace
{
    public class GameJoinManager : MonoBehaviour
    {
        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
            DontDestroyOnLoad(gameObject);
        }
        private void Start()
        {
            Screen.fullScreen = false;
            int width = Screen.width;
            int height = Screen.height;
            if (Application.isEditor)
            {
                Screen.SetResolution(width, height, false); // 에디터에서 실행 중일 때 해상도 설정
            }
            else
            {
                Screen.SetResolution(3040/3, 1440/3, false);
            }
        }

        public void Connect(string gamename, string mapname)
        {
            GameDataManager.Instance.mapname = mapname;
            GameDataManager.Instance.gamename = gamename;
            SceneManager.LoadScene("LoadScene");
        }
    }
}

