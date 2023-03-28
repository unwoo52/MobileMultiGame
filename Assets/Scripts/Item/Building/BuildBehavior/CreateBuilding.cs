using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface ICancelbuild
{
    bool CancelBuildObject();
}
public interface ICreateBuilding
{
    void CreateBuildObject(BuidingItemData itemdata);
}
public interface ISetBuildObjectPos
{
    void SetBuildObjectPos(Vector3 vector3);
}
public interface ICompleteBuild
{
    void CompleteBuildObject();
}

public class CreateBuilding : MonoBehaviour, ICancelbuild, ICreateBuilding, ISetActive, ISetBuildObjectPos, ICompleteBuild
{
    [SerializeField] GameObject buildObject;
    [SerializeField] Building building;
    BuidingItemData _itemData;
    public bool CancelBuildObject()
    {
        Destroy(this);
        return true;
    }

    public void CompleteBuildObject()
    {
        building._itemdata = _itemData;
        Destroy(this);
    }

    public void CreateBuildObject(BuidingItemData itemData)
    {
        _itemData = itemData;
        InstantiateBuilding();
        
    }

    private void InstantiateBuilding()
    {
        buildObject = Instantiate(_itemData.ItemPrefab, transform);
    }

    

    public void SetBuildObjectPos(Vector3 vector3)
    {
        Debug.Log(vector3);
        Renderer renderer = buildObject.GetComponent<Renderer>();
        float height = renderer.bounds.size.y;
        transform.position = new Vector3(vector3.x, vector3.y + height / 2f, vector3.z);
    }

    public void SetObjectActive(bool b)
    {
        buildObject.SetActive(b);
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
