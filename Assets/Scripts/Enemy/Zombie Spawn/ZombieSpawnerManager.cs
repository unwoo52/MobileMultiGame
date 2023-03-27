using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGetZombieSpawner
{
    void GetZombieSpawner();
}
public class ZombieSpawnerManager : MonoBehaviour, IMorningCallback, INightCallback
{
    [SerializeField] List<GameObject> spawnerList;
    public void MorningCallback()
    {
        StartCoroutine(DestroyZombie());
    }

    public void NightCallback()
    {
        Debug.Log("Night!");
        foreach (GameObject spawner in spawnerList)
        {
            for(int i = 0; i < 10; i++)
            {
                Vector3 tempPos = new Vector3(Random.Range(0, 10), 0, Random.Range(0, 10));
                GameObject prefab = Resources.Load<GameObject>("Warzombie F Pedroso");
                if (PhotonNetwork.IsConnected == false)
                {
                    Instantiate(prefab, spawner.transform.position + tempPos, Quaternion.identity);
                    continue;
                }
                if (PhotonNetwork.IsMasterClient)
                {
                    GameObject obj = PhotonNetwork.Instantiate(prefab.name, spawner.transform.position + tempPos, Quaternion.identity, 0);                    
                    obj.transform.SetParent(inGameManager.Instance.GetEnemyInstalledParent());
                }
            }
            
        }
    }

    IEnumerator DestroyZombie()
    {
        Transform parent = inGameManager.Instance.GetEnemyInstalledParent();
        while (parent.childCount > 0)
        {
            Destroy(parent.GetChild(0).gameObject);            
            yield return null;
        }
    }
}
