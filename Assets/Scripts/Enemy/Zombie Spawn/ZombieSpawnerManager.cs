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
        Debug.Log("Morning!");
    }

    public void NightCallback()
    {
        Debug.Log("Night!");
        foreach (GameObject spawner in spawnerList)
        {
            spawner.TryGetComponent(out IGetZombieSpawner zombieSpawner);
            zombieSpawner.GetZombieSpawner();
        }
    }
}
