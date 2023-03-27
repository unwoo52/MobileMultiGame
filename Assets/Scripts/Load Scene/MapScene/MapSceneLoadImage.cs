using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSceneLoadImage : MonoBehaviour
{
    public Image image;
    public float endTime;

    private void Start()
    {
        StartCoroutine(PaseOut1());
    }

    IEnumerator PaseOut1()
    {
        Color color;
        float value = 0;
        float t;
        float currentLerpTime = 0;
        while (value < 0.9f) 
        {
            currentLerpTime += Time.deltaTime;
            t = currentLerpTime / endTime;
            t = Mathf.Sin(t * Mathf.PI * 0.5f);
            value = Mathf.Lerp(0,1,t);
            color = new Color(0.1f, 0.1f, 0.1f, 1 - value);
            image.color = color;
            yield return null;
        }
        Destroy(gameObject);
    }
}
