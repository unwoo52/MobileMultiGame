using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "Buiding Item Data", menuName = "Scriptable Object/Buiding Item Data", order = int.MaxValue)]
public class BuidingItemData : ItemData
{
    [SerializeField]
    private float _hp;
    public float HP { get { return _hp; } set { _hp = value; } }

    [SerializeField]
    private GameObject _itemPrefab;
    public GameObject ItemPrefab { get { return _itemPrefab; } }
}