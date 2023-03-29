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
public class PlayerInstalledObjectsManagement : MonoBehaviour
{
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
        string json = JsonUtility.ToJson(_wrapper);

        Debug.Log(json);

        // 파일 이름 생성에 DateTime.Now.Ticks 값을 사용하여 유니크한 파일 이름을 생성합니다.
        string filename = "BUILDING_DATA_" + DateTime.Now.Ticks.ToString() + ".json";
        string filepath = Application.dataPath + "/Saved/GameData/";
        CreateDirectoryIfNotExists(filepath);
        File.WriteAllText(filepath + filename, json);
    }

    
    [ContextMenu("!!!Load Building Datas")]
    private void LoadBuildingData()
    {
        // Saved/GameData 폴더에서 .json 파일을 모두 찾아서 읽어옵니다.
        string[] filenames = Directory.GetFiles(Application.dataPath + "/Saved/GameData", "*.json");

        foreach (string filename in filenames)
        {
            string str = File.ReadAllText(filename);

            // 파일에서 읽어온 JSON 데이터를 BuildObjectData 리스트로 변환합니다.
            BuildObjcetDataWrapper childObjects = JsonUtility.FromJson<BuildObjcetDataWrapper>(str);
            
            foreach (BuildObjectData buildObjectData in childObjects.objects)
            {
                LoadData(buildObjectData);
            }
        }
    }

    private bool LoadData(out string json, string filepath, string filename)
    {
        json = null;

        string path = Path.Combine(filepath, filename);
        json = File.ReadAllText(path);

        return true;
    }
    
    private void LoadData(BuildObjectData data4)
    {
        GameObject createobject = null;
        if (PhotonNetwork.IsConnected == false)
        {
            createobject = Instantiate(_buildParent, this.transform);
        }
        else if (PhotonNetwork.IsMasterClient)
        {
            createobject = PhotonNetwork.Instantiate(_buildParent.name, transform.position, Quaternion.identity, 0);
        }

        if (createobject != null)
        {
            createobject.transform.position = data4.position;
            createobject.transform.rotation = data4.rotation;
            if (createobject.TryGetComponent(out Building building))
            {
                if (building.TryGetComponent(out ISetBuildingItemData setBuildingItemData))
                {
                    setBuildingItemData.SetBuildingItemData(data4.itemcode, data4.hp);
                }
            }

        }
    }


    private void CreateDirectoryIfNotExists(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }
}
