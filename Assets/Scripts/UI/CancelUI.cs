using UnityEngine;

public interface ICancelusingItemEffect
{
    bool IsCancelusingItemEffectOn(Vector2 vector2, Camera camera);
}
public interface ICancelItemCondition
{
    bool IsCancelusingItemConditionOn();
}
public interface ISetParticleObject
{
    void SetParticleObject(bool isActiveUI);
}
public interface ISetCancelImageObject
{
    void SetCancelImageObject(bool isActiveUI);
}
public interface IGetCancelImage
{
    GameObject CancelImage { get; }
}
public class CancelUI : MonoBehaviour, ICancelusingItemEffect, IGetCancelImage, ISetActive, ICancelItemCondition
{
    [SerializeField] private GameObject _cancelImage;
    [SerializeField] private float distanceMouseAndCancelRect;

    [SerializeField][Range(0, 10)] private float _effectOnDistance = 1;

    public GameObject CancelImage => _cancelImage;
    public void SetObjectActive(bool showCancelUI)
    {
        gameObject.SetActive(showCancelUI);
    }

    /// <summary>
    /// 마우스와 cancel UI의 거리를 계산해서, 일정 거리보다 가까우면 cancel Effect를 출력하고, true를 반환
    /// </summary>
    public bool IsCancelusingItemEffectOn(Vector2 vector2, Camera camera)
    {
        RectTransform rectTransform = _cancelImage.GetComponent<IGetCancelImageRectTransform>().GetCancelImageRectTransform();

        Vector2 MousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            vector2,
            camera,
            out MousePosition);

        distanceMouseAndCancelRect = CalculateDistance(MousePosition, rectTransform, _effectOnDistance);

        //effect on
        SetCancelImageSize(distanceMouseAndCancelRect);
        return distanceMouseAndCancelRect < 1;
    }

    public bool IsCancelusingItemConditionOn()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    ///  RectTransform와 마우스 위치 사이의 거리를 계산하고, 마우스가 경계(rect의 사각형 안에 들어오는 순간)부터(1~) rect의 center(~0)에 닿을 때 까지 거리를 정규화하여 1에서 0의 값을 반환한다.
    ///  effectOnDistance을 조절하여 경계의 거리를 늘릴 수 있다. 0이면 경계가 rect의 모서리부터이지만, 값이 커질수록 더 먼 거리에서부터 반환값이 1이 되기 시작한다.
    ///  마우스가 경계보다 멀리 있으면 1보다 큰 값을 반환한다.
    /// </summary>
    private float CalculateDistance(Vector2 MousePosition, RectTransform rectTransform, float effectOnDistance)
    {
        //마우스와의 거리
        float arrivePointX = rectTransform.rect.center.x;
        float distancMouseX = Mathf.Abs(MousePosition.x - arrivePointX);
        float normalizedDistanceX = distancMouseX / ((rectTransform.rect.width / 2f) + (rectTransform.rect.width * effectOnDistance));


        float arrivePointY = rectTransform.rect.center.y;
        float distancMouseY = Mathf.Abs(MousePosition.y - arrivePointY);
        float normalizedDistanceY = distancMouseY / ((rectTransform.rect.height / 2f) + (rectTransform.rect.width * effectOnDistance));

        float distancMouse = normalizedDistanceX > normalizedDistanceY ? normalizedDistanceX : normalizedDistanceY;

        return distancMouse;
    }

    private bool SetCancelImageSize(float distance_Cancel_and_BuildEffectOn)
    {
        //cancelUI의 사이즈를 거리에 따라 수정
        _cancelImage.GetComponent<IModify_CancelImageRect>().Modify_CancelImageRect(distance_Cancel_and_BuildEffectOn);
        return true;
    }

}
