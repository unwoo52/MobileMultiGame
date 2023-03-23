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
    /// ���콺�� cancel UI�� �Ÿ��� ����ؼ�, ���� �Ÿ����� ������ cancel Effect�� ����ϰ�, true�� ��ȯ
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
    ///  RectTransform�� ���콺 ��ġ ������ �Ÿ��� ����ϰ�, ���콺�� ���(rect�� �簢�� �ȿ� ������ ����)����(1~) rect�� center(~0)�� ���� �� ���� �Ÿ��� ����ȭ�Ͽ� 1���� 0�� ���� ��ȯ�Ѵ�.
    ///  effectOnDistance�� �����Ͽ� ����� �Ÿ��� �ø� �� �ִ�. 0�̸� ��谡 rect�� �𼭸�����������, ���� Ŀ������ �� �� �Ÿ��������� ��ȯ���� 1�� �Ǳ� �����Ѵ�.
    ///  ���콺�� ��躸�� �ָ� ������ 1���� ū ���� ��ȯ�Ѵ�.
    /// </summary>
    private float CalculateDistance(Vector2 MousePosition, RectTransform rectTransform, float effectOnDistance)
    {
        //���콺���� �Ÿ�
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
        //cancelUI�� ����� �Ÿ��� ���� ����
        _cancelImage.GetComponent<IModify_CancelImageRect>().Modify_CancelImageRect(distance_Cancel_and_BuildEffectOn);
        return true;
    }

}
