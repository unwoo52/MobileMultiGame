using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Database", menuName = "Scriptable Object/Item Database", order = int.MaxValue)]
public class ItemDatabase : ScriptableObject, ISerializationCallbackReceiver // ISerializationCallbackReceiver - 직렬화 역직렬화 수동처리 콜백용
{

    public BuidingItemData[] Items;
    public Dictionary<int, BuidingItemData> GetItem = new Dictionary<int, BuidingItemData>();
    public void OnAfterDeserialize()
    {
        int i = 0;
        foreach (BuidingItemData item in Items)
        {
            //item.ItemCode = i;
            GetItem.Add(i, item);
            i++;
        }
    }
    public void OnBeforeSerialize()
    {
        GetItem = new Dictionary<int, BuidingItemData>();
    }
}