using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BuildingItemUsingSyetem : ItemUsingSystem
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject _prefabTempBuildObjectParent;
    private GameObject buildObject;


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
        else buildObject.GetComponent<ICompleteBuild>().CompleteBuildObject();//testcode
                                                                          //if can building place > CompleteBuild();
                                                                          //else cannot building place > Cancelbuild();

        //if dis is
        //  >set ptc
        //else
        //  >deactive ptc
        return true;
    }


    public bool CreateBuildObject()
    {

        //설치되는 오브젝트들을 모아두는 InstallObjectsParent을 호출해 parent로 지정
        Transform InstallObjectParent = inGameManager.Instance.GetInstallObjectsParent();

        //건물 설치를 컨트롤하는 프레펩 오브젝트(스크립트만 있는 오브젝트)를 생성
        buildObject = Instantiate(_prefabTempBuildObjectParent, InstallObjectParent);

        //CreateBuildObject 기능은 위에 코드에서 만든 object의 스크립트에게 이관
            //설치할 건물 데이터 불러오기
        if (!GetBuildObjectAtItemData(out BuidingItemData itemData)) return false;
            //CreateBuildObject
        buildObject.GetComponent<ICreateBuilding>().CreateBuildObject(itemData);

        return true;
    }

    private bool GetBuildObjectAtItemData(out BuidingItemData itemdata)
    {
        itemdata = transform.parent.GetComponent<IGetItemData>().GetItemData();
        return true;
    }
}
