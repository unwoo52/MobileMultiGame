using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathTESTScript : MonoBehaviour
{
    public float rotationSpeed = 10.0f;

    void FixedUpdate()
    {
        XRotate();
    }

    private void XRotate()
    {// 회전 방향과 속도를 설정합니다.
        Vector3 rotationDirection = Vector3.right + Vector3.up + Vector3.forward;
        float angularVelocity = rotationSpeed;

        // Rigidbody 컴포넌트를 가져와 회전을 적용합니다.
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.angularVelocity = rotationDirection * angularVelocity;

    }
    private void YRotate()
    {// 회전 방향과 속도를 설정합니다.
        Vector3 rotationDirection = Vector3.up;
        float angularVelocity = rotationSpeed;

        // Rigidbody 컴포넌트를 가져와 회전을 적용합니다.
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.angularVelocity = rotationDirection * angularVelocity;

    }
}
