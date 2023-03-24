using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private GameObject FollowTarget;
    Vector3 FollowPos;
    [SerializeField] private float HeightVector;
    [SerializeField] private float ForwardVector;
    [SerializeField] private float SideVector;
    [SerializeField] private float Dist;
    [SerializeField] private float LerpSpeed;
    private void Update()
    {
        FollowPos = Vector3.Lerp(FollowPos, FollowTarget.transform.position, Time.deltaTime * LerpSpeed);
        transform.position = 
            FollowPos
            + Vector3.up* HeightVector
            + Vector3.right*SideVector
            + Vector3.forward*ForwardVector
            + -Camera.main.transform.forward*Dist;
    }
}


