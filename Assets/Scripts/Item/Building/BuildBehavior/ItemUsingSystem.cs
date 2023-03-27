using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUsingSystem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    protected GameObject _inventory;
    protected GameObject _toggleUI;
    protected GameObject _cancelUI;
    protected GameObject _cancelUIImage;
    protected bool distanceFlag = false;
    protected bool CancelFlag = false;
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!ItemOnBeginDrag()) CancleDragEvent(eventData);
        if (!StartDrag(eventData)) CancleDragEvent(eventData);
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!ItemOnDrag(eventData)) CancleDragEvent(eventData);
        if (!BeingDrag(eventData)) CancleDragEvent(eventData);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!ItemOnDragEnd()) CancleDragEvent(eventData);
        if (!EndDrag(eventData)) CancleDragEvent(eventData);
    }

    protected bool IsMouseClosetoCancelUI()
    {
        return _cancelUIImage.GetComponent<IIsMouseCloseToCancelUI>().IsMouseCloseToCancelUI();
      
    }

    protected virtual bool StartDrag(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    protected virtual bool BeingDrag(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    protected virtual bool EndDrag(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }
    #region .

    private bool ItemOnBeginDrag()
    {
        if (!GetInventory()) { Debug.LogError("fail log"); return false; }
        if (!GetCancelUI()) { Debug.LogError("fail log"); return false; }

        _inventory.GetComponent<ISetActive_ExitImage_And_ToggleInventoryUI>().SetActive_ExitImage_And_ToggleInventoryUI(true);

        return true;
    }
    private bool ItemOnDrag(PointerEventData eventData)
    {
        //아이템 사용 캔슬 이펙트 출력
        if(_cancelUI.GetComponent<ICancelusingItemEffect>().IsCancelusingItemEffectOn(eventData.position, eventData.enterEventCamera)) 
            distanceFlag = true;
        else 
            distanceFlag = false;

        //item cancel 조건 설정
        //if()
        
        return true;
    }
    private bool ItemOnDragEnd()
    {
        _inventory.GetComponent<ISetActive_ExitImage_And_ToggleInventoryUI>().SetActive_ExitImage_And_ToggleInventoryUI(false);

        return true;
    }

    private bool GetInventory()
    {
        _inventory = CanvasManagement.Instance.Inventory;
        return _inventory != null;
    }
    private bool GetCancelUI()
    {
        if (_inventory == null) GetInventory();
        _cancelUI = _inventory.GetComponent<IGetCancelUI>().CancelUI;
        _toggleUI = _inventory.GetComponent<IGetToggleUI>().ToggleUI;
        _cancelUIImage = _cancelUI.GetComponent<IGetCancelImage>().CancelImage;
        return _cancelUI != null;
    }
    private void CancleDragEvent(PointerEventData eventData)
    {
        Debug.LogError("\"CancleDragEvent()\" METHOD EXECUTED ");
        eventData.pointerDrag = null;
        eventData.dragging = false;
    }
    #endregion
}
