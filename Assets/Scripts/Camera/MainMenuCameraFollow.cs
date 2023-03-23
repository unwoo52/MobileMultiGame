using UnityEngine;
using System.Collections;

public class MainMenuCameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject CameraPositionDefult;
    [SerializeField] private GameObject CameraPositionA;
    [SerializeField] private GameObject CameraPositionB;
    [SerializeField] private GameObject CameraPositionC;
    [SerializeField] private GameObject CameraPositionD;
    [SerializeField] private float ZoomTime;

    private IEnumerator currentCoroutine;

    public void PressButtonDefult()
    {
        StopCurrentCoroutine();
        currentCoroutine = MoveCameraTo(CameraPositionDefult);
        StartCoroutine(currentCoroutine);
    }
    public void PressButtonA()
    {
        StopCurrentCoroutine();
        currentCoroutine = MoveCameraTo(CameraPositionA);
        StartCoroutine(currentCoroutine);
    }

    public void PressButtonB()
    {
        StopCurrentCoroutine();
        currentCoroutine = MoveCameraTo(CameraPositionB);
        StartCoroutine(currentCoroutine);
    }

    public void PressButtonC()
    {
        StopCurrentCoroutine();
        currentCoroutine = MoveCameraTo(CameraPositionC);
        StartCoroutine(currentCoroutine);
    }

    public void PressButtonD()
    {
        StopCurrentCoroutine();
        currentCoroutine = MoveCameraTo(CameraPositionD);
        StartCoroutine(currentCoroutine);
    }

    private IEnumerator MoveCameraTo(GameObject target)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        float currentLerpTime = 0;
        float t;


        while (currentLerpTime < ZoomTime)
        {

            t = currentLerpTime / ZoomTime;
            t = t * t * t * (t * (6f * t - 15f) + 10f);
            transform.position = Vector3.Lerp(startPos, target.transform.position, t);
            transform.rotation = Quaternion.Lerp(startRot, target.transform.rotation, t);
            currentLerpTime += Time.deltaTime;
            yield return null;
        }

        transform.position = target.transform.position;
        transform.rotation = target.transform.rotation;
    }

    private void StopCurrentCoroutine()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
    }
}