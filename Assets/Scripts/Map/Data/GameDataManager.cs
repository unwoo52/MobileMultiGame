using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

public interface ISaveData
{
    void SaveData<T>(string filePath, string filename, T data);
}
public interface ILoadDataAtDirectory
{
    List<T> LoadDataAtDirectory<T>(string datapath);
}
public class GameDataManager : MonoBehaviour, ISaveData, ILoadDataAtDirectory
{
    #region singleton
    private static GameDataManager _instance = null;

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
    public static GameDataManager Instance
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

    #region interface
    public List<T> LoadDataAtDirectory<T>(string datapath)
    {
        return LoadBinaryFiles<T>(datapath);
    }
    public void SaveData<T>(string filePath, string filename, T data)
    {
        SaveBinary(filePath, filename, data);
    }

    #endregion

    #region private codes
    private bool ToJsonUtillity<T>(out string str, T data)
    {
        str = JsonUtility.ToJson(data);
        return true;
    }

    private void SaveBinary<T>(string filePath, string filename, T data)
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
                if (Path.GetExtension(filePath) == ".bin")
                {
                    try
                    {
                        using (FileStream fileStream = File.Open(filePath, FileMode.Open))
                        {
                            T data = (T)binaryFormatter.Deserialize(fileStream);
                            dataList.Add(data);
                            fileStream.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogErrorFormat("Error while loading file {0}: {1}", filePath, ex.Message);
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
    #endregion
}
