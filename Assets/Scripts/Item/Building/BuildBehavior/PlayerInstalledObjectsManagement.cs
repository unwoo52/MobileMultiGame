using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildObjectData
{
    public Vector3 position;
    public Quaternion rotation;
    public string name;
    public float hp;
    public int itemcode;
}
[System.Serializable]
public class BuildObjcetDataWrapper
{
    public BuildObjectData[] objects;
}
public class PlayerInstalledObjectsManagement : MonoBehaviour, ILoadGameData<BuildObjcetDataWrapper>, ISaveGameData<BuildObjcetDataWrapper>
{
    [SerializeField] private GameObject _buildParent;

    #region interfaces

    public bool SaveGameData(out BuildObjcetDataWrapper data)
    {
        return SaveBuildings(out data);
    }
    public bool LoadGameData(BuildObjcetDataWrapper data)
    {
        return LoadBuildings(data);
    }
    #endregion


    private bool SaveBuildings(out BuildObjcetDataWrapper buildObjcetDataWrapper)
    {
        List<BuildObjectData> buildObjectDataList = new List<BuildObjectData>();
        BuildObjcetDataWrapper _wrapper = new();

        for (int i = 0; i < transform.childCount; i++)
        {
            BuildObjectData buildObjectData = new();
            GetBuildingData(ref buildObjectData, i);

            buildObjectDataList.Add(buildObjectData);
        }

        _wrapper.objects = buildObjectDataList.ToArray();

        buildObjcetDataWrapper = _wrapper;

        return true;
    }
    private bool GetBuildingData(ref BuildObjectData buildObjectData, int i)
    {
        GameObject gameObject = transform.GetChild(i).gameObject;

        buildObjectData.position = gameObject.transform.position;
        buildObjectData.rotation = gameObject.transform.rotation;
        if (gameObject.TryGetComponent(out IGetItemData getItemData))
        {
            BuidingItemData itemData = getItemData.GetItemData();
            buildObjectData.name = itemData.ItemName;
            buildObjectData.itemcode = itemData.ItemCode;
            buildObjectData.hp = itemData.HP;
        }

        return true;
    }


    
    private bool LoadBuildings(BuildObjcetDataWrapper buildObjcetDataWrapper)
    {            
        foreach (BuildObjectData buildObjectData in buildObjcetDataWrapper.objects)
        {
            CreateBuilding(buildObjectData);
        }
        return true;
    }

    
    private bool CreateBuilding(BuildObjectData buildingdata)
    {
        if(!CreateBuildPrefab(out GameObject createobject)) return false;

        if (!SetBuildingState(createobject, buildingdata)) return false;

        return true;
    }

    private bool CreateBuildPrefab(out GameObject buildPrefab)
    {
        buildPrefab = null;
        if (_buildParent == null) return false;

        if (PhotonNetwork.IsConnected == false)
        {
            buildPrefab = Instantiate(_buildParent, this.transform);
        }
        else if (PhotonNetwork.IsMasterClient)
        {
            buildPrefab = PhotonNetwork.Instantiate(_buildParent.name, transform.position, Quaternion.identity, 0);
        }
        else return false;

        if (buildPrefab == null) return false;

        return true;
    }

    private bool SetBuildingState(GameObject createobject, BuildObjectData buildingdata)
    {
        createobject.transform.position = buildingdata.position;
        createobject.transform.rotation = buildingdata.rotation;
        if (createobject.TryGetComponent(out Building building))
        {
            if (building.TryGetComponent(out ISetBuildingItemData setBuildingItemData))
            {
                setBuildingItemData.SetBuildingItemData(buildingdata.itemcode, buildingdata.hp);
            }
        }

        return true;
    }

}
