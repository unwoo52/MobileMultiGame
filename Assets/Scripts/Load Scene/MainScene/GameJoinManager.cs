using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        Screen.SetResolution(800, 600, false);
    }

    public void Connect(string gamename, string mapname)
    {
        GameSaveManagement.Instance.mapname= mapname;
        GameSaveManagement.Instance.gamename=gamename;
        SceneManager.LoadScene("LoadScene");
    }
}
