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
    {// ȸ�� ����� �ӵ��� �����մϴ�.
        Vector3 rotationDirection = Vector3.right + Vector3.up + Vector3.forward;
        float angularVelocity = rotationSpeed;

        // Rigidbody ������Ʈ�� ������ ȸ���� �����մϴ�.
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.angularVelocity = rotationDirection * angularVelocity;

    }
    private void YRotate()
    {// ȸ�� ����� �ӵ��� �����մϴ�.
        Vector3 rotationDirection = Vector3.up;
        float angularVelocity = rotationSpeed;

        // Rigidbody ������Ʈ�� ������ ȸ���� �����մϴ�.
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.angularVelocity = rotationDirection * angularVelocity;

    }
}
