using Photon.Pun;
using System;
using System.Collections.Generic;
using System.IO;
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
public class PlayerInstalledObjectsManagement : MonoBehaviour, ILoadGameData<BuildObjcetDataWrapper>, ISaveGameData
{
    #region interfaces
    public bool SaveGameData(string gamename)
    {
        throw new NotImplementedException();
    }

    public bool LoadGameData(BuildObjcetDataWrapper data)
    {
        return LoadDataAndCreateBuilding(data);
    }
    #endregion

    [SerializeField] private GameObject _buildParent;

    [ContextMenu("!!!Save Building Objects")]
    private void SaveBuildingObjects()
    {
        List<BuildObjectData> childObjects = new List<BuildObjectData>();
        BuildObjcetDataWrapper _wrapper = new();

        for (int i = 0; i < transform.childCount; i++)
        {
            BuildObjectData buildObjectData = new BuildObjectData();
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
            childObjects.Add(buildObjectData);
        }

        _wrapper.objects = childObjects.ToArray();

        SaveData(_wrapper);
    }

    private void SaveData<T>(T _wrapper)
    {
        /*
        string json = JsonUtility.ToJson(_wrapper);

        Debug.Log(json);

        // 파일 이름 생성에 DateTime.Now.Ticks 값을 사용하여 유니크한 파일 이름을 생성합니다.
        string filename = "BUILDING_DATA_" + DateTime.Now.Ticks.ToString() + ".json";
        string filepath = Application.dataPath + "/Saved/GameData/";
        CreateDirectoryIfNotExists(filepath);
        File.WriteAllText(filepath + filename, json);*/
    }

    
    private bool LoadDataAndCreateBuilding(BuildObjcetDataWrapper buildObjcetDataWrapper)
    {            
        foreach (BuildObjectData buildObjectData in buildObjcetDataWrapper.objects)
        {
            LoadBuilding(buildObjectData);
        }
        return true;
    }

    
    private bool LoadBuilding(BuildObjectData buildingdata)
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
