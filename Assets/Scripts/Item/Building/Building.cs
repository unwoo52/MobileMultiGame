using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IGetItemData
{
    public BuidingItemData _itemdata;

    public BuidingItemData GetItemData()
    {
        return _itemdata;
    }
}
