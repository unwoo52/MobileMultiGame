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

        //��ġ�Ǵ� ������Ʈ���� ��Ƶδ� InstallObjectsParent�� ȣ���� parent�� ����
        Transform InstallObjectParent = inGameManager.Instance.GetInstallObjectsParent();

        //�ǹ� ��ġ�� ��Ʈ���ϴ� ������ ������Ʈ(��ũ��Ʈ�� �ִ� ������Ʈ)�� ����
        buildObject = Instantiate(_prefabTempBuildObjectParent, InstallObjectParent);

        //CreateBuildObject ����� ���� �ڵ忡�� ���� object�� ��ũ��Ʈ���� �̰�
            //��ġ�� �ǹ� ������ �ҷ�����
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
