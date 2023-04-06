using MyNamespace;
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
    void CompleteBuildObject(BuildingItemData _itemdata, Vector3 point);
}
public interface ISetBuildingMesh
{
    bool SetBuildingMesh(Mesh mesh, Material material);
}

public class CreateBuilding : MonoBehaviourPun, ICancelbuild,  ISetBuildObjectPos, ICompleteBuild, ISetBuildingMesh
{
    [SerializeField] Building building;
    public bool CancelBuildObject()
    {
        Destroy(this);
        return true;
    }
    public void CompleteBuildObject(BuildingItemData _itemdata, Vector3 point)
    {
        Renderer renderer = GetComponent<Renderer>();
        float height = renderer.bounds.size.y / 2;
        InstantiateBuilding(_itemdata, point + new Vector3(0, height, 0));
        Destroy(gameObject);
    }

    public void SetBuildObjectPos(Vector3 vector3)
    {
        //get half of height
        Renderer renderer = GetComponent<Renderer>();
        float height = renderer.bounds.size.y;

        //set position
        transform.position = new Vector3(vector3.x, vector3.y + height / 2f, vector3.z);
    }
    /*
    private bool InstantiateBuilding(out GameObject buildingObject, BuildingItemData buidingItemData)
    {
        buildingObject = null;
        if (PhotonNetwork.IsConnected == false)
        {
            buildingObject = Instantiate(buidingItemData.ItemPrefab, InGameManager.Instance.PlayerInstalledObjectsParent.transform);
        }
        else
        {
            buildingObject = PhotonNetwork.Instantiate(buidingItemData.ItemPrefab.name, transform.position, Quaternion.identity, 0);
            buildingObject.transform.parent = null;
        }

        return true;
    }*/
    private bool InstantiateBuilding(BuildingItemData buidingItemData, Vector3 position)
    {
        if (PhotonNetwork.IsConnected == false)
        {
            GameObject gameobject = Instantiate(buidingItemData.ItemPrefab, position, Quaternion.identity, InGameManager.Instance.PlayerInstalledObjectsParent.transform);
            gameobject.transform.SetParent(StaticManager.Instance.transform);
        }
        else
        {
            GameObject gameobject = PhotonNetwork.Instantiate(buidingItemData.ItemPrefab.name, position, Quaternion.identity, 0);
            gameobject.transform.SetParent(StaticManager.Instance.transform);
            photonView.RPC(nameof(ChangerTranform), RpcTarget.All);
        }

        return true;
    }
    [PunRPC]
    public void ChangerTranform()
    {
        transform.SetParent(StaticManager.Instance.transform);
    }
    public bool SetBuildingMesh(Mesh mesh, Material material)
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        if (meshFilter != null && meshRenderer != null)
        {
            meshFilter.mesh = mesh;
            meshRenderer.material = material;
            return true;
        }
        else
        {
            Debug.LogError("MeshFilter or MeshRenderer not found on game object.");
            return false;
        }
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
