using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TestItemMakeScript : MonoBehaviour
{
    private List<ItemData> itemList = new();
    private BinaryFormatter binaryFormatter = new();
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private GameObject _contentParent;
    public GameObject ItemPrefab { get { return _itemPrefab; } }
    public GameObject ContentParent { get { return _contentParent; } }

    public string TestString;
    public GameObject TestPrefabs;
    public Sprite TestSprite;

    private void Start()
    {
        AddItem();
        Initialize();
    }
    private void Initialize()
    {
        FindOnject(ref _contentParent, "Content Parent");
    }

    private bool FindOnject(ref GameObject gameobject, string findName)
    {
        if (gameobject != null) return true;
        if (transform.Find(findName) == null) return false;

        gameobject = transform.Find(findName).gameObject;

        return true;
    }

    [ContextMenu("AddItem")]
    public void AddItem()
    {

        GameObject InstanteObject = Instantiate(_itemPrefab);
        InstanteObject.transform.SetParent(_contentParent.transform, false);
        InstanteObject.transform.localScale = Vector3.one;

        Item itemScript;
        InstanteObject.TryGetComponent(out itemScript);
        itemScript.Initialize();
        itemScript.SetItemData(TestString, TestPrefabs, TestSprite);
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
