using System;
using System.Text;
using UnityEngine;
[Serializable]
public struct ItemDataStruct
{
    public int itemCount;
    public StringBuilder itemName;
    public GameObject itemPrefab;
    public Sprite itemImage;
}
public interface IGetBuildObject
{
    GameObject GetbuildObject();
}
public class Item : MonoBehaviour, IGetBuildObject
{
    private ItemDataStruct _itemData;
    public ItemDataStruct ItemData => _itemData;
    public void Initialize()
    {
        _itemData.itemName = new StringBuilder();
    }
    public bool SetItemData(string itemName, GameObject itemPrefab, Sprite itemImage)
    {
        _itemData.itemName.Append(itemName);
        _itemData.itemPrefab = itemPrefab;
        SetItemImage(itemImage);
        return true;
    }


    /*codes*/ //나중에 ItemBehavior라는 부모 클래스로 만든 후, 상속.
    private bool SetItemImage(Sprite image)
    {
        _itemData.itemImage = image;

        if (!GetImageObject(out Transform ItemImageObj)) return false;
        if (!ItemImageObj.TryGetComponent(out UnityEngine.UI.Image imageCom)) return false;
        imageCom.sprite = image;

        return true;
    }
    protected bool GetImageObject(out Transform transform)
    {
        transform = this.transform.Find("Image");
        return true;
    }
    protected bool GetNameTextMashObject(out Transform transform)
    {
        transform = this.transform.Find("Item Name");
        return true;
    }
    protected bool GetCountTextMahObject(out Transform transform)
    {
        transform = this.transform.Find("Item Count");
        return true;
    }
    private void UseItem()
    {
        if (IsExistItem())
        {

        }
    }

    private bool IsExistItem()
    {
        return _itemData.itemCount > 0;
    }

    public GameObject GetbuildObject()
    {
        return ItemData.itemPrefab;
    }
}
