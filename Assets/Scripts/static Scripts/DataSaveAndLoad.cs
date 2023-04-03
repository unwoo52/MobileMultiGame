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
            //LogError(filePath + "������ �������� �ʽ��ϴ�.");
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
                        Debug.LogErrorFormat("Error while loading file {0}: {1}", filePath, ex.Message);
                        //�� �޼ҵ�� ����Ʈ�� ��ȯ�ϴµ�, ���߿� bool method(ref List<T> list, string dirpath)�� �ٲٱ�.
                        //createnewgame���� �������̴�, createnewgame �ڵ� �����丵 �ϰų� �ƴϸ� ���� ������ (�����̸� + ���� ������)�������� �ٲ� �� ������ �ϱ�.
                        //(�����̸� + ���� ������)�������� �ٲ۴ٸ�, load������ ������ �ε嵵 ������ �� ��ȯ�� �̷������ �ص� ��������
                    }
                }
            }
        }
        else
        {
            //Debug.LogError(directoryPath + "���丮�� �������� �ʽ��ϴ�.");
        }
        return dataList;
    }
    #endregion

    #region json methods
    public static List<T> LoadJsonFilesInDirectory<T>(string directoryPath) where T : class
    {
        List<T> dataList = new List<T>();

        DirectoryInfo dir = new DirectoryInfo(directoryPath);
        FileInfo[] fileInfos = dir.GetFiles("*.json");

        foreach (FileInfo fileInfo in fileInfos)
        {
            try
            {
                using (StreamReader reader = new StreamReader(fileInfo.FullName))
                {
                    string json = reader.ReadToEnd();
                    T data = JsonUtility.FromJson<T>(json);
                    dataList.Add(data);
                }
            }
            catch (Exception e) 
            {
                dataList.Add(e as T);
            }
            
        }

        return dataList;
    }
    public static List<string> LoadJsonStringFilesInDirectory(string directoryPath)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
        FileInfo[] fileInfo = directoryInfo.GetFiles("*.json");
        List<string> jsonDataList = new List<string>();

        foreach (FileInfo file in fileInfo)
        {
            string jsonData = File.ReadAllText(file.FullName);
            jsonDataList.Add(jsonData);
        }

        return jsonDataList;
    }
    public static bool LoadDataoneIMSIMUSTBESUJANG(out string json, string filepath, string filename)
    {
        json = null;

        string path = Path.Combine(filepath, filename);
        json = File.ReadAllText(path);

        return true;
    }

    public static bool LoadToJson<T>(ref T data ,string filepath, string filename)
    {
        string path = Path.Combine(filepath, $"{filename}.json");

        if (!File.Exists(path))
        {
            data = default;
            return false;
        }

        string str = File.ReadAllText(path);

        data = JsonUtility.FromJson<T>(str);

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
