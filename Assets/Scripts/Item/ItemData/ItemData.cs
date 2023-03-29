using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : ScriptableObject
{
    [SerializeField]
    private string _itemName;
    public string ItemName { get { return _itemName; } }

    [SerializeField]
    private Sprite _itemImage;
    public Sprite ItemImage { get { return _itemImage; } }

    [SerializeField]
    private int _itemCode;
    public int ItemCode { get { return _itemCode; } }
}
