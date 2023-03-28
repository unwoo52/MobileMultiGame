using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class BuildObjectData
{
    public Transform transform;
    public string name;
    public float hp;
    public int itemcode;
}

public class PlayerInstalledObjectsManagement : MonoBehaviour
{
    string filename = "TEST.json";
    [ContextMenu("First Child Building Save at Json")]
    private void TESTSAVE()
    {
        List<BuildObjectData> childObjects = new List<BuildObjectData>();
        BuildObjectData buildObjectData = new BuildObjectData();

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject gameObject = transform.GetChild(i).gameObject; // i번째 자식 오브젝트의 Transform을 가져옴


            buildObjectData.transform = gameObject.transform;
            if (gameObject.TryGetComponent(out IGetBuildingName getBuildingName)) getBuildingName.GetBuildingName(out string name);
            buildObjectData.name = name;

            childObjects.Add(buildObjectData);
        }
        string str = JsonUtility.ToJson(childObjects[0]);

        File.WriteAllText(Application.dataPath + "/Saved/GameData" + filename, JsonUtility.ToJson(childObjects[0]));
    }




    [ContextMenu("Load Building Data")]
    private void TESTLOAD()
    {
        string str2 = File.ReadAllText(Application.dataPath + "/Saved/GameData" + filename);

        BuildObjectData data4 = JsonUtility.FromJson<BuildObjectData>(str2);
    }
}
