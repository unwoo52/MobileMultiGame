using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IIsMouseCloseToCancelUI
{
    bool IsMouseCloseToCancelUI();
}
public interface IModify_CancelImageRect
{
    void Modify_CancelImageRect(float distance);
}
public interface IGetCancelImageRectTransform
{
    RectTransform GetCancelImageRectTransform();
}
public interface ISetActive
{
    void SetObjectActive(bool param);
}
public class CancelImage : MonoBehaviour, ISetActive, IModify_CancelImageRect, IIsMouseCloseToCancelUI, IGetCancelImageRectTransform
{
    [SerializeField] private RectTransform _cancelImageBigRect;
    [SerializeField] [Range(0,10f)]private float maxScaleFactor = 1.5f;
    public void SetObjectActive(bool param)
    {
        gameObject.SetActive(param);
    }

    public void Modify_CancelImageRect(float distance)
    {
        _cancelImageBigRect.localScale = Vector3.Lerp(new Vector3(maxScaleFactor, maxScaleFactor, 0), new Vector3(1, 1, 0), distance);
    }

    public bool IsMouseCloseToCancelUI()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(_cancelImageBigRect, Input.mousePosition);
    }

    public RectTransform GetCancelImageRectTransform()
    {
        return GetComponent<RectTransform>();
    }
}
