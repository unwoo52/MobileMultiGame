using System.Collections.Generic;
using UnityEngine;

namespace MyNamespace
{
    public class TestItemMakeScript : MonoBehaviour
    {
        [SerializeField] private List<BuildingItemData> itemscriptableList;

        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private GameObject _contentParent;


        private void Start()
        {
            foreach (var item in itemscriptableList)
            {
                AddItem(item);
            }
            Destroy(gameObject);
        }

        [ContextMenu("AddItem")]
        public void AddItem(BuildingItemData scriptableObject)
        {
            GameObject InstanteObject = Instantiate(_itemPrefab);
            InstanteObject.transform.SetParent(_contentParent.transform, false);
            InstanteObject.transform.localScale = Vector3.one;

            if (InstanteObject.TryGetComponent(out Item itemScript))
            {
                itemScript.SetItemData(scriptableObject);
            }
        }
    }
}
