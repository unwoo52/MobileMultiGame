using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IGetBuildingName
{
    void GetBuildingName(out string name);
}
public class Building : MonoBehaviour, IGetBuildingName
{
    public GameObject buildingObject;
    public void GetBuildingName(out string name)
    {
        name = buildingObject.name;
    }
}
