using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TestItemMakeScript : MonoBehaviour
{
    [SerializeField] private List<BuidingItemData> itemscriptableList;

    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private GameObject _contentParent;


    private void Start()
    {
        foreach(var item in itemscriptableList)
        {
            AddItem(item);
        }
    }

    [ContextMenu("AddItem")]
    public void AddItem(BuidingItemData scriptableObject)
    {
        GameObject InstanteObject = Instantiate(_itemPrefab);
        InstanteObject.transform.SetParent(_contentParent.transform, false);
        InstanteObject.transform.localScale = Vector3.one;



        if(InstanteObject.TryGetComponent(out Item itemScript))
        {
            itemScript.SetItemData(scriptableObject);
        }
    }
}