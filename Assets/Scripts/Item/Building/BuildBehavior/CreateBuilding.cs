using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface ICancelbuild
{
    bool CancelBuildObject();
}
public interface ISetBuildObjectPos
{
    void SetBuildObjectPos(Vector3 vector3);
}
public interface ICompleteBuild
{
    void CompleteBuildObject();
}

public class CreateBuilding : MonoBehaviour, ICancelbuild,  ISetBuildObjectPos, ICompleteBuild
{
    GameObject buildObject;
    [SerializeField] Building building;
    public bool CancelBuildObject()
    {
        Destroy(this);
        return true;
    }
    public bool StartBuildProcess(out GameObject buildobject, BuidingItemData itemData)
    {        
        if(!InstantiateBuilding(out buildobject, itemData))
        {
            buildobject = null;
            return false;
        }
        this.buildObject = buildobject;
        return true;
    }
    public void CompleteBuildObject()
    {
        Destroy(this);
    }

    public void SetBuildObjectPos(Vector3 vector3)
    {
        //get half of height
        Renderer renderer = buildObject.GetComponent<Renderer>();
        float height = renderer.bounds.size.y;

        //set position
        transform.position = new Vector3(vector3.x, vector3.y + height / 2f, vector3.z);
    }

    private bool InstantiateBuilding(out GameObject buildingObject, BuidingItemData buidingItemData)
    {
        buildingObject = null;
        if (PhotonNetwork.IsConnected == false)
        {
            buildingObject = Instantiate(buidingItemData.ItemPrefab, this.transform);
        }
        else
        {
            buildingObject = PhotonNetwork.Instantiate(buidingItemData.ItemPrefab.name, transform.position, Quaternion.identity, 0);
            buildingObject.transform.parent = this.transform;
        }

        return true;
    }


    /*/*
    */
    private void FindNearestWalkablePointWithSpace(Vector3 position, float radiusX, float radiusZ)
    {
        // Define the search area using a capsule shape with the specified radii
        radiusX = this.GetComponentInChildren<MeshFilter>().mesh.bounds.size.x;
        radiusZ = this.GetComponentInChildren<MeshFilter>().mesh.bounds.size.z;
        Vector3 searchPosition = new Vector3(position.x, position.y + radiusX, position.z);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(searchPosition, out hit, radiusX + radiusZ, NavMesh.AllAreas))
        {
            Debug.DrawRay(transform.position, transform.position - hit.position, Color.red, 0.5f);
            // Check if the nearest walkable point has enough space for the object
            bool isWalkable = true;
            Collider[] colliders = Physics.OverlapBox(hit.position, new Vector3(radiusX, 0.1f, radiusZ));
            foreach (Collider collider in colliders)
            {
                if (!collider.isTrigger)
                {
                    isWalkable = false;
                    break;
                }
            }
            if (isWalkable)
            {
                // hit.position now contains the nearest walkable point with enough space for the object
                Debug.Log("Nearest walkable point with space: " + hit.position);
            }
            else
            {
                Debug.Log("No walkable point with enough space found!");
            }
        }
        else
        {
            // No walkable point found
            Debug.Log("No walkable point found!");
        }
    }
    /*
     */
}
