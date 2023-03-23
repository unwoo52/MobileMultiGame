using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroToPlayer : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayerMask;
    List<GameObject> EnemyList= new List<GameObject>();
    WaitForSeconds waitTime = new WaitForSeconds(1);

    private void Start()
    {
        StartCoroutine(AggroEnemy());
    }

    IEnumerator AggroEnemy()
    {
        while (true)
        {
            foreach (GameObject enemy in EnemyList)
            {
                //if (!TryGetComponent(out SearchingFSM_LookAround script)) break;
                Ray ray = new(transform.position, enemy.transform.position - transform.position);
                //Debug.DrawRay(transform.position + Vector3.up, enemy.transform.position - transform.position + Vector3.up, Color.red, 0.5f);

                //script.GetRay(ray, this.gameObject);
            }
            yield return waitTime;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (1 << other.gameObject.layer == enemyLayerMask)
        {
            EnemyList.Add(other.gameObject);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        /*
        //범위 밖으로 나간 적에게 em.Reset실행
        if (1 << other.gameObject.layer == enemyLayerMask)
        {
            EnemyList.Remove(other.gameObject);
            TryGetComponent(out SearchingFSM_LookAround script);
            script.LostInSight();
        }
        */
    }
}
