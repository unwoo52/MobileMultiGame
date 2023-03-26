using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Scriptable Object/Item Data", order = int.MaxValue)]
public class ItemData : ScriptableObject
{
    [SerializeField]
    private string _itemName;
    public string ItemName { get { return _itemName; } }


    [SerializeField]
    private int _itemCount;
    public int ItemCount { get { return _itemCount; } }

    [SerializeField]
    private GameObject _itemPrefab;
    public GameObject ItemPrefab { get { return _itemPrefab; } }

    [SerializeField]
    private Sprite _itemImage;
    public Sprite ItemImage { get { return _itemImage; } }
}