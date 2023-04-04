using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInitialize : MonoBehaviour
{
    [SerializeField] private GameObject InGameManager;
    [SerializeField] private GameObject PlayerIntalledObjectsParent;
    [SerializeField] private GameObject EnemyInstalledParent;
    [SerializeField] private GameObject TestSpawner;
    [SerializeField] private GameObject PlayerPrefab;

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
            SingleGameInit();
        else 
            MultiGameInitialize();
        
    }
    private bool MultiGameInitialize()
    {
        if (!PhotonNetwork.IsMasterClient)
            return true;

        PhotonNetwork.Instantiate(InGameManager.name,               Vector3.zero, Quaternion.identity, 0);
        PhotonNetwork.Instantiate(PlayerIntalledObjectsParent.name, Vector3.zero, Quaternion.identity, 0);
        PhotonNetwork.Instantiate(EnemyInstalledParent.name,        Vector3.zero, Quaternion.identity, 0);
        PhotonNetwork.Instantiate(TestSpawner.name,                 Vector3.zero, Quaternion.identity, 0);


        return true;
    }
    private bool SingleGameInit()
    {
        return true;
    }
}
