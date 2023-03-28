using System;
using System.Text;
using UnityEngine;

public interface IGetItemData
{
    BuidingItemData GetItemData();
}
public class Item : MonoBehaviour, IGetItemData
{
    private BuidingItemData _itemData;

    public BuidingItemData GetItemData()
    {
        return _itemData;
    }

    public bool SetItemData(BuidingItemData itemData)
    {
        _itemData = itemData;
        SetItemImage(itemData.ItemImage);
        return true;
    }


    /*codes*/ //나중에 ItemBehavior라는 부모 클래스로 만든 후, 상속.
    private bool SetItemImage(Sprite image)
    {
        if (!this.transform.Find("Image").TryGetComponent(out UnityEngine.UI.Image imageCom)) return false;
        imageCom.sprite = image;

        return true;
    }
}
