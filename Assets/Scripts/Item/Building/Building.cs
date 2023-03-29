using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICreateBuilding
{
    void CreateBuildObject(BuidingItemData itemdata);
}
public interface ISetBuildingItemData
{
    void SetBuildingItemData(int _code, float hp);
}
public class Building : MonoBehaviour, IGetItemData, ICreateBuilding, ISetActive, ISetBuildingItemData
{
    [SerializeField]
    private ItemDatabase _itemDatabase;
    [SerializeField]
    private CreateBuilding _createBuilding;

    private GameObject buildObject;

    private BuidingItemData _itemdata;
    private float _hp;
    public BuidingItemData ItemData { get { return _itemdata; } set { _itemdata = value; } }


    public void CreateBuildObject(BuidingItemData itemData)
    {
        _itemdata = itemData;

        //doBuildProcess
        _createBuilding.BuildProcess(out buildObject, _itemdata);
    }

    public BuidingItemData GetItemData()
    {
        return _itemdata;
    }

    public void SetBuildingItemData(int _code, float hp)
    {
        _itemdata = _itemDatabase.Items[_code] as BuidingItemData;
        _hp = hp;
        _createBuilding.BuildProcess(out buildObject, _itemdata);
    }
    public void SetObjectActive(bool b)
    {
        gameObject.SetActive(b);
    }
}
