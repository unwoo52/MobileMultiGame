using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPun
{
    [SerializeField] private VariableJoystick variableJoystick;
    [SerializeField] private AnimContoller animContoller;
    [SerializeField] private float RotateSpeed;


    private void Start()
    {
        variableJoystick = CanvasManagement.Instance.VariableJoystick.GetComponent<VariableJoystick>();
        animContoller = GetComponent<AnimContoller>();
    }

    public void FixedUpdate()
    {

        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        Vector3 target = new Vector3(variableJoystick.Horizontal,0, variableJoystick.Vertical);

        if(variableJoystick.Horizontal == 0 && variableJoystick.Vertical == 0) { animContoller.OnStopMove(); }
        else
        {
            animContoller.OnLookat(transform.position + target, RotateSpeed);
            animContoller.OnOnlyMove();
        }
    }

}
