using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingScript : MonoBehaviour
{
    [SerializeField] private LayerMask TargetMask;
    [SerializeField] private List<GameObject> gameObjects= new List<GameObject>();
    public List<GameObject> GameOnjects => gameObjects;
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & TargetMask) != 0)
        {
            gameObjects.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & TargetMask) != 0)
        {
            ILostTarget target;
            target = transform.parent.GetComponent<ILostTarget>();
            if (target != null) { target = GetComponent<ILostTarget>(); }
            target.LostTarget(other.gameObject);
            gameObjects.Remove(other.gameObject);
        }
    }
}
