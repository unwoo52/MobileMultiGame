using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSceneZoomCameraController : MonoBehaviour
{
    [SerializeField] private Camera startCamera;
    [SerializeField] private Transform startZoomStartPos;
    [SerializeField] private Transform startZoomEndPos;
    [SerializeField] private float zoomTime = 1f;

    private void Start()
    {

        // ���� ī�޶� Ȱ��ȭ
        startCamera.gameObject.SetActive(true);
        startCamera.transform.position = startZoomStartPos.position;
        startCamera.transform.rotation = startZoomStartPos.rotation;

        // �ε巯�� �̵� ����
        StartCoroutine(StartCameraZoom());
    }

    private IEnumerator StartCameraZoom()
    {
        float elapsedTime = 0f;

        while (elapsedTime < zoomTime)
        {
            float t = elapsedTime / zoomTime;

            t = t = Mathf.Sin(t * Mathf.PI * 0.5f);

            startCamera.transform.position = Vector3.Lerp(startZoomStartPos.position, startZoomEndPos.position, t);
            startCamera.transform.rotation = Quaternion.Lerp(startZoomStartPos.rotation, startZoomEndPos.rotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        startCamera.gameObject.SetActive(false);
        Destroy(gameObject);
    }
}