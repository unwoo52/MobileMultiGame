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
            //cancel effect가 활성화되면, 건물 오브젝트 set active false

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
            //설치되는 오브젝트들을 모아두는 InstallObjectsParent을 호출해 parent로 지정
            Transform InstallObjectParent = InGameManager.Instance.PlayerInstalledObjectsParent.transform;

            //건물 설치를 컨트롤하는 프레펩 오브젝트(스크립트만 있는 오브젝트)를 생성
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
                Debug.LogError("아이템의 메시 정보를 가져오는데에 실패했습니다.");
                return false;
            }

            if(buildObject.TryGetComponent(out ISetBuildingMesh setBuildingMesh))
            {
                if(!setBuildingMesh.SetBuildingMesh(meshFilter2.sharedMesh, meshRenderer1.sharedMaterial))
                {
                    Debug.LogError("아이템의 메시 정보 설정에 실패했습니다.");
                    return false;
                }
            }
            else
            {
                Debug.LogError("아이템의 ISetBuildingMesh 인터페이스를 불러오는 것에 실패했습니다.");
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
