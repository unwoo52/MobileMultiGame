using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ISetVisibile_ToggleUI
{
    void SetVisibile_ToggleUI(bool showToggleUI);
}
public interface IToggleUIWidth
{
    void ToggleUIWidth();
}
public class ToggleUI : MonoBehaviour, IToggleUIWidth, ISetVisibile_ToggleUI
{
    private Coroutine coToggle;
    private bool IsHideUI = true;
    private Color _originColor;
    public Color OriginColor => _originColor;
    [SerializeField] private Sprite ToggleButtonImageShow;
    [SerializeField] private Sprite ToggleButtonImageHide;
    [SerializeField] private float ToggleSpeedInventoryUI = 0.06f;
    [SerializeField] private RectTransform ContentRect;

    private void Start()
    {
        _originColor = GetComponent<Image>().color;
        if(ContentRect == null) ContentRect = transform.parent.GetChild(0).GetComponent<RectTransform>();
        GetComponent<Image>().sprite = IsHideUI ? ToggleButtonImageShow : ToggleButtonImageHide;
    }

    public void SetVisibile_ToggleUI(bool showToggleUI)
    {
        Image image = GetComponent<Image>();
        image.color = showToggleUI ? new Color(0, 0, 0, 0) : OriginColor;
    }
    public void ToggleUIWidth()
    {
        if (coToggle != null) StopCoroutine(coToggle);
        coToggle = StartCoroutine(ToggleWidth());
    }

    public void ToggleUIHigth()
    {
        if (coToggle != null) StopCoroutine(coToggle);
        coToggle = StartCoroutine(ToggleHigth());
    }
    IEnumerator ToggleWidth()
    {
        SwapImage();
        float endPosX = !IsHideUI ? ContentRect.rect.width : 0;

        IsHideUI = !IsHideUI;

        Vector2 position;
        RectTransform rectTransform = transform.parent.GetComponent<RectTransform>();
        float curY = rectTransform.anchoredPosition.y;
        float distance = Mathf.Abs(rectTransform.anchoredPosition.x - endPosX);

        while (Mathf.Epsilon < distance)
        {
            position = new Vector2(Mathf.Lerp(rectTransform.anchoredPosition.x, endPosX, ToggleSpeedInventoryUI), curY);
            rectTransform.anchoredPosition = position;

            distance = Mathf.Abs(rectTransform.anchoredPosition.x - endPosX);

            yield return null;
        }
    }

    IEnumerator ToggleHigth()
    {
        SwapImage();
        float endPosY = !IsHideUI ? 0 : ContentRect.rect.height;

        IsHideUI = !IsHideUI;

        Vector2 position;
        RectTransform rectTransform = transform.parent.GetComponent<RectTransform>();
        float curX = rectTransform.anchoredPosition.x;
        float distance = Mathf.Abs(rectTransform.anchoredPosition.y - endPosY);

        while (Mathf.Epsilon < distance)
        {
            position = new Vector2(curX, Mathf.Lerp(rectTransform.anchoredPosition.y, endPosY, ToggleSpeedInventoryUI));
            rectTransform.anchoredPosition = position;

            distance = Mathf.Abs(rectTransform.anchoredPosition.y - endPosY);

            yield return null;
        }
    }

    private void SwapImage()
    {
        GetComponent<Image>().sprite = IsHideUI? ToggleButtonImageHide : ToggleButtonImageShow;
    }
}
