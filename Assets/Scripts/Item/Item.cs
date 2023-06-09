using UnityEngine;

namespace MyNamespace
{
    public interface IGetItemData
    {
        BuildingItemData GetItemData();
    }
    public class Item : MonoBehaviour, IGetItemData
    {
        private BuildingItemData _itemData;

        public BuildingItemData GetItemData()
        {
            return _itemData;
        }

        public bool SetItemData(BuildingItemData itemData)
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
}

