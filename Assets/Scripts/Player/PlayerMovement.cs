using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPun
{
    [SerializeField] private VariableJoystick variableJoystick;
    [SerializeField] private AnimContoller animContoller;
    [SerializeField] private float rotateSpeed;
    private Camera _camera;
    private float HorMove;
    private float VerMove;


    private void Start()
    {
        variableJoystick = CanvasManagement.Instance.VariableJoystick.GetComponent<VariableJoystick>();
        animContoller = GetComponent<AnimContoller>();

        _camera = Camera.main;
    }

    public void FixedUpdate()
    {
        
        if (PhotonNetwork.IsConnected == true && photonView.IsMine == false)
        {
            return;
        }

        if (variableJoystick.Horizontal == 0 && variableJoystick.Vertical == 0)
        {
            animContoller.OnSetHorizontalParam(0);
            animContoller.OnSetVerticalParam(0);
        }
        else
        {
            animContoller.OnSetHorizontalParam(variableJoystick.Horizontal);
            animContoller.OnSetVerticalParam(variableJoystick.Vertical);

            if (Mathf.Abs(variableJoystick.Horizontal) > 0.3f)
            {
                transform.Rotate(Vector3.up, variableJoystick.Horizontal * rotateSpeed * Time.deltaTime);
            }
        }
    }
}
