using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TestItemMakeScript : MonoBehaviour
{
    [SerializeField] private List<ItemData> itemscriptableList;

    private BinaryFormatter binaryFormatter = new();

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
    public void AddItem(ItemData scriptableObject)
    {
        GameObject InstanteObject = Instantiate(_itemPrefab);
        InstanteObject.transform.SetParent(_contentParent.transform, false);
        InstanteObject.transform.localScale = Vector3.one;

        if(InstanteObject.TryGetComponent(out Item itemScript))
        {
            itemScript.Initialize();
            itemScript.SetItemData(scriptableObject.ItemName, scriptableObject.ItemPrefab, scriptableObject.ItemImage);
        }
    }


    /**/
    //-------
    public void SaveBinary<T>(string filePath, T data)
    {
        using (FileStream fileStream = File.Create(filePath))
        {
            binaryFormatter.Serialize(fileStream, data);
            fileStream.Close();
        }
    }

    public T LoadBinary<T>(string filePath)
    {
        T data = default;
        if (File.Exists(filePath))
        {
            using (FileStream fileStream = File.Open(filePath, FileMode.Open))
            {
                data = (T)binaryFormatter.Deserialize(fileStream);
                fileStream.Close();
            }
        }
        else
        {
            Debug.LogError(filePath + "파일이 존재하지 않습니다.");
        }
        return data;
    }
}
