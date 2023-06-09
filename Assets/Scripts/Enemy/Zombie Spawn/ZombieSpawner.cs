using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyNamespace
{
    public class ZombieSpawner : MonoBehaviourPunCallbacks, IGetZombieSpawner
    {

        public void GetZombieSpawner()
        {
            GameObject prefab = Resources.Load<GameObject>("Warzombie F Pedroso");
            if (PhotonNetwork.IsConnected == false)
            {
                Instantiate(prefab, transform.position, Quaternion.identity);
                return;
            }
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate(prefab.name, transform.position, Quaternion.identity, 0);
            }
        }
    }
}

