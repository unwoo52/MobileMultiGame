using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CharacterMovement
{
    public VariableJoystick variableJoystick;


    public void FixedUpdate()
    {
        Vector3 target = new Vector3(variableJoystick.Horizontal,0, variableJoystick.Vertical);
        MoveToPosition(
            transform.position + target,
            3
            );

        if(variableJoystick.Horizontal == 0 && variableJoystick.Vertical == 0 && coMove != null) { StopMovement(); }
    }

}
