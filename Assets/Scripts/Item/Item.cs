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


        /*codes*/ //���߿� ItemBehavior��� �θ� Ŭ������ ���� ��, ���.
        private bool SetItemImage(Sprite image)
        {
            if (!this.transform.Find("Image").TryGetComponent(out UnityEngine.UI.Image imageCom)) return false;
            imageCom.sprite = image;

            return true;
        }
    }
}

