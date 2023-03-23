using UnityEngine;
using UnityEngine.UI;

public interface ISetActive_ExitImage_And_ToggleInventoryUI
{
    void SetActive_ExitImage_And_ToggleInventoryUI(bool isActiveUI);
}
public interface IGetCancelUI
{
    GameObject CancelUI { get; }
}
public interface IGetToggleUI
{
    GameObject ToggleUI { get; }
}

public class Inventory : MonoBehaviour, ISetActive_ExitImage_And_ToggleInventoryUI, IGetCancelUI, IGetToggleUI
{
    [SerializeField] private GameObject _cancelUI;
    [SerializeField] private GameObject _toggleUI;

    public GameObject CancelUI { get { return _cancelUI; } }
    public GameObject ToggleUI { get { return _toggleUI; } }


    public void SetActive_ExitImage_And_ToggleInventoryUI(bool isActiveUI)
    {
        Toggle_Inventory();

        _cancelUI.GetComponent<ISetActive>().SetObjectActive(isActiveUI);

        _toggleUI.GetComponent<ISetVisibile_ToggleUI>().SetVisibile_ToggleUI(isActiveUI);

    }
    /*codes*/
    #region .
    private void Toggle_Inventory()
    {
        _toggleUI.GetComponent<IToggleUIWidth>().ToggleUIWidth();
    }
    #endregion
}
