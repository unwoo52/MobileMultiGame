using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyNamespace
{
    public class BuildingItemUsingSyetem : ItemUsingSystem
    {
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private GameObject _buildParent;
        private GameObject buildObject;
        BuildingItemData _itemdata;


        protected override bool StartDrag(PointerEventData eventData)
        {
            if (!CreateBuildObject()) { Debug.LogError("fail log"); return false; }
            return true;
        }
        protected override bool BeingDrag(PointerEventData eventData)
        {
            //cancel effect�� Ȱ��ȭ�Ǹ�, �ǹ� ������Ʈ set active false

            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
            {
                if (distanceFlag)
                {
                    buildObject.GetComponent<ISetActive>().SetObjectActive(false);
                }
                else
                {
                    buildObject.GetComponent<ISetActive>().SetObjectActive(true);
                    buildObject.GetComponent<ISetBuildObjectPos>().SetBuildObjectPos(hit.point);
                }
            }
            return true;
        }
        protected override bool EndDrag(PointerEventData eventData)
        {
            //if on exit build image > Cancelbuild();
            if (IsMouseClosetoCancelUI())
            {
                buildObject.GetComponent<ICancelbuild>().CancelBuildObject();
            }
            //else if on ground >
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(eventData.position);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
                {
                    buildObject.GetComponent<ICompleteBuild>().CompleteBuildObject(_itemdata, hit.point);
                }
            }
                

            //if dis is
            //  >set ptc
            //else
            //  >deactive ptc
            return true;
        }


        public bool CreateBuildObject()
        {
            //��ġ�Ǵ� ������Ʈ���� ��Ƶδ� InstallObjectsParent�� ȣ���� parent�� ����
            Transform InstallObjectParent = InGameManager.Instance.PlayerInstalledObjectsParent.transform;

            //�ǹ� ��ġ�� ��Ʈ���ϴ� ������ ������Ʈ(��ũ��Ʈ�� �ִ� ������Ʈ)�� ����
            /*
            if (PhotonNetwork.IsConnected == false)
            {
                buildObject = Instantiate(_buildParent, InstallObjectParent);
            }
            else
            {
                buildObject = PhotonNetwork.Instantiate(_buildParent.name, Vector3.zero, Quaternion.identity, 0);
                buildObject.transform.SetParent(InstallObjectParent, false);
            }
            */
            buildObject = Instantiate(_buildParent, InstallObjectParent);

            BuildingItemData itemdata = null;
            MeshRenderer meshRenderer1 = null;
            MeshFilter meshFilter2 = null;
            if(GetBuildObjectAtItemData(out itemdata))
            {
                _itemdata = itemdata;
                itemdata.ItemPrefab.TryGetComponent(out meshRenderer1);
                itemdata.ItemPrefab.TryGetComponent(out meshFilter2);
            }
            else
            {
                Debug.LogError("�������� �޽� ������ �������µ��� �����߽��ϴ�.");
                return false;
            }

            if(buildObject.TryGetComponent(out ISetBuildingMesh setBuildingMesh))
            {
                if(!setBuildingMesh.SetBuildingMesh(meshFilter2.sharedMesh, meshRenderer1.sharedMaterial))
                {
                    Debug.LogError("�������� �޽� ���� ������ �����߽��ϴ�.");
                    return false;
                }
            }
            else
            {
                Debug.LogError("�������� ISetBuildingMesh �������̽��� �ҷ����� �Ϳ� �����߽��ϴ�.");
                return false;
            }


            return true;
        }
        private bool GetBuildObjectAtItemData(out BuildingItemData itemdata)
        {
            itemdata = transform.parent.GetComponent<IGetItemData>().GetItemData();
            return true;
        }
    }
}
