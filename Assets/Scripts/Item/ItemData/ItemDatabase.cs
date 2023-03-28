using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Database", menuName = "Scriptable Object/Item Database", order = int.MaxValue)]
public class ItemDatabase : ScriptableObject, ISerializationCallbackReceiver // ISerializationCallbackReceiver - ����ȭ ������ȭ ����ó�� �ݹ��
{

    public ItemData[] Items;
    public Dictionary<int, ItemData> GetItem = new Dictionary<int, ItemData>();
    public void OnAfterDeserialize()
    {
        int i = 0;
        foreach (ItemData item in Items)
        {
            //item.ItemCode = i;
            GetItem.Add(i, item);
            i++;
        }
    }
    public void OnBeforeSerialize()
    {
        GetItem = new Dictionary<int, ItemData>();
    }
}