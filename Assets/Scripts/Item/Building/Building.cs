using UnityEngine;


namespace MyNamespace
{
    public interface ISetBuildingItemData
    {
        void SetBuildingItemData(int _code, float hp);
    }
    public class Building : MonoBehaviour, IGetItemData, ISetActive, ISetBuildingItemData
    {
        [SerializeField]
        private ItemDatabase _itemDatabase;
        [SerializeField]
        private CreateBuilding _createBuilding;

        private GameObject buildObject;

        private BuildingItemData _itemdata;
        private float _hp;
        public BuildingItemData ItemData { get { return _itemdata; } set { _itemdata = value; } }


        public BuildingItemData GetItemData()
        {
            return _itemdata;
        }

        public void SetBuildingItemData(int _code, float hp)
        {
            _itemdata = _itemDatabase.Items[_code] as BuildingItemData;
            _hp = hp;
        }
        public void SetObjectActive(bool b)
        {
            gameObject.SetActive(b);
        }
    }

}
