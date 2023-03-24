using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPun
{
    [SerializeField] private VariableJoystick variableJoystick;
    [SerializeField] private AnimContoller animContoller;
    [SerializeField] private float rotateSpeed;
    private Camera _camera;


    private void Start()
    {
        variableJoystick = CanvasManagement.Instance.VariableJoystick.GetComponent<VariableJoystick>();
        animContoller = GetComponent<AnimContoller>();

        _camera = Camera.main;
    }

    public void FixedUpdate()
    {
        
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        /*
        Vector3 target = new Vector3(variableJoystick.Horizontal, 0, variableJoystick.Vertical);

        Quaternion cameraRotation = _camera.transform.rotation;
        float angle = Quaternion.Angle(Quaternion.identity, cameraRotation);

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
        Vector3 rotatedVector = rotation * target;
        */


        if (variableJoystick.Horizontal == 0 && variableJoystick.Vertical == 0)
        {
            animContoller.OnSetHorizontalParam(0);
            animContoller.OnSetVerticalParam(0);
        }
        else
        {
            //animContoller.OnLookat(transform.position + rotatedVector, RotateSpeed);
            //animContoller.OnOnlyMove();
            animContoller.OnSetHorizontalParam(variableJoystick.Horizontal);
            animContoller.OnSetVerticalParam(variableJoystick.Vertical);
            if (Mathf.Abs(variableJoystick.Horizontal) > 0.3f)
            {
                transform.Rotate(Vector3.up, variableJoystick.Horizontal * rotateSpeed * Time.deltaTime);
            }
        }
    }
}
