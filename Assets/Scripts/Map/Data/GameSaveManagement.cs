using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public interface ISaveData
{
    void SaveData(GameObject PlayerInstalledObjectsParent);
}

public class GameSaveManagement : MonoBehaviour, ISaveData
{
    private BinaryFormatter binaryFormatter = new();

    public void SaveData(GameObject PlayerInstalledObjectsParent)
    {
        string filepath = "";
        for (int i = 0; i < PlayerInstalledObjectsParent.transform.childCount; i++)
        {
            Transform _transform = PlayerInstalledObjectsParent.transform.GetChild(i);
            ItemData itemData = _transform.GetComponent<ItemData>();
            SaveBinary(filepath, itemData, _transform, _transform.rotation, _transform.name);
        }
    }


    public void SaveBinary<T>(string filePath, T data, Transform transform, Quaternion rotate, string filename)
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

    public T LoadBinary<T>(string filePath)
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

    public List<T> LoadBinaryFiles<T>(string directoryPath)
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
