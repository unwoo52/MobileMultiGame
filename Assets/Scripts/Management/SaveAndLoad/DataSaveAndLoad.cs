using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class DataSaveAndLoad
{
    static  BinaryFormatter binaryFormatter = new();

    #region private codes

    #region binary methods
    public static void SaveBinary<T>(string filePath, string filename, T data)
    {
        string combinedFilePath = Path.Combine(filePath, filename);

        CreateDirectoryIfNotExists(filePath);
        using (FileStream fileStream = File.Create(combinedFilePath))
        {
            binaryFormatter.Serialize(fileStream, data);
            fileStream.Close();
        }
    }


    public static T LoadBinary<T>(string filePath)
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
            //LogError(filePath + "파일이 존재하지 않습니다.");
        }
        return data;
    }

    public static List<T> LoadBinaryFiles<T>(string directoryPath)
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
                        //Debug.LogErrorFormat("Error while loading file {0}: {1}", filePath, ex.Message);
                        //이 메소드는 리스트를 반환하는데, 나중에 bool method(ref List<T> list, string dirpath)로 바꾸기.
                        //createnewgame에서 참조중이니, createnewgame 코드 리팩토링 하거나 아니면 게임 저장을 (게임이름 + 게임 데이터)통합으로 바꿀 때 리뉴얼 하기.
                        //(게임이름 + 게임 데이터)통합으로 바꾼다면, load씬에서 데이터 로드도 끝나면 씬 전환이 이루어지게 해도 괜찮을듯
                    }
                }
            }
        }
        else
        {
            //Debug.LogError(directoryPath + "디렉토리가 존재하지 않습니다.");
        }
        return dataList;
    }
    #endregion

    #region json methods

    public static bool LoadDataoneIMSIMUSTBESUJANG(out string json, string filepath, string filename)
    {
        json = null;

        string path = Path.Combine(filepath, filename);
        json = File.ReadAllText(path);

        return true;
    }

    public static bool LoadToJson<T>(ref T t ,string filepath, string filename)
    {
        string path = Path.Combine(filepath, $"{filename}.json");

        if (!File.Exists(path))
        {
            t = default(T);
            return false;
        }

        string str = File.ReadAllText(path);
        t = JsonUtility.FromJson<T>(str);
        return true;
    }
    public static bool SaveToJson<T>(T data, string filepath, string filename)
    {
        string str = JsonUtility.ToJson(data);
        try
        {
            CreateDirectoryIfNotExists(filepath);

            if (filename.EndsWith(".json"))
            {
                filename = filename.Substring(0, filename.Length - 5);
            }
            File.WriteAllText(Path.Combine(filepath, $"{filename}.json"), str);
        }
        catch
        {
            return false;
        }

        return true;
    }
    #endregion

    public static void CreateDirectoryIfNotExists(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }
    #endregion
}
