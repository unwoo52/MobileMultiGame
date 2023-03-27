using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public interface ISaveDatasTESTITEMDATASAVE
{
    void SaveDataTESTITEMDATASAVE(GameObject PlayerInstalledObjectsParent);
}
public interface ISaveData
{
    void SaveData<T>(string filePath, string filename, T data);
}
public class GameSaveManagement : MonoBehaviour, ISaveDatasTESTITEMDATASAVE, ISaveData
{
    #region singleton
    private static GameSaveManagement _instance = null;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static GameSaveManagement Instance
    {
        get
        {
            if (null == _instance)
            {
                return null;
            }
            return _instance;
        }
    }
    #endregion
    private BinaryFormatter binaryFormatter = new();
    public string gamename, mapname;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void SaveData<T>(string filePath, string filename, T data)
    {
        SaveBinary(filePath, filename, data);
    }
    public void SaveDataTESTITEMDATASAVE(GameObject PlayerInstalledObjectsParent)
    {
        string filepath = "";
        for (int i = 0; i < PlayerInstalledObjectsParent.transform.childCount; i++)
        {
            Transform _transform = PlayerInstalledObjectsParent.transform.GetChild(i);
            ItemData itemData = _transform.GetComponent<ItemData>();
            BinarySave_DataAndTransformAndRotate(filepath, _transform.name, itemData, _transform, _transform.rotation);
        }
    }
    public void SaveBinary<T>(string filePath, string filename, T data)
    {
        string directoryPath = filePath;
        string combinedFilePath = Path.Combine(directoryPath, filename);

        CreateDirectoryIfNotExists(directoryPath);
        using (FileStream fileStream = File.Create(combinedFilePath))
        {
            binaryFormatter.Serialize(fileStream, data);
            fileStream.Close();
        }
    }


    private void BinarySave_DataAndTransformAndRotate<T>(string filePath, string filename, T data, Transform transform, Quaternion rotate)
    {
        string directoryPath = filePath;
        string combinedFilePath = Path.Combine(directoryPath, filename);

        CreateDirectoryIfNotExists(directoryPath);

        using (FileStream fileStream = File.Create(combinedFilePath))
        {
            binaryFormatter.Serialize(fileStream, data);
            binaryFormatter.Serialize(fileStream, transform.position);
            binaryFormatter.Serialize(fileStream, transform.rotation);
            binaryFormatter.Serialize(fileStream, rotate);
        }
    }

    private T LoadBinary<T>(string filePath)
    {
        T data = default;
        if (File.Exists(filePath))
        {
            using (FileStream fileStream = File.Open(filePath, FileMode.Open))
            {
                data = (T)binaryFormatter.Deserialize(fileStream);
                fileStream.Close();
            }
        }
        else
        {
            Debug.LogError(filePath + "파일이 존재하지 않습니다.");
        }
        return data;
    }

    private List<T> LoadBinaryFiles<T>(string directoryPath)
    {
        List<T> dataList = new List<T>();
        if (Directory.Exists(directoryPath))
        {
            foreach (string filePath in Directory.GetFiles(directoryPath))
            {
                if (Path.GetExtension(filePath) == ".dat")
                {
                    using (FileStream fileStream = File.Open(filePath, FileMode.Open))
                    {
                        T data = (T)binaryFormatter.Deserialize(fileStream);
                        dataList.Add(data);
                    }
                }
            }
        }
        else
        {
            Debug.LogError(directoryPath + "디렉토리가 존재하지 않습니다.");
        }
        return dataList;
    }

    private void CreateDirectoryIfNotExists(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }

}
