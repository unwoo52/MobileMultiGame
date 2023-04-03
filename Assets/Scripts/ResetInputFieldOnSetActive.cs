using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResetInputFieldOnSetActive : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    private void OnEnable()
    {
        inputField.text = string.Empty;
    }

    private void OnDisable()
    {
        inputField.text = string.Empty;
    }
}