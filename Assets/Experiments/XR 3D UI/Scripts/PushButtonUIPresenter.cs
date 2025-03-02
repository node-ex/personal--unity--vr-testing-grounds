using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Content.Interaction;

public class PushButtonUIPresenter : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _buttonValueText;

    [SerializeField]
    private XRPushButton _pushButton;

    private void Awake()
    {
        _pushButton.onValueChange.AddListener(OnButtonValueChange);
    }

    private void OnDestroy()
    {
        _pushButton.onValueChange.RemoveListener(OnButtonValueChange);
    }

    private void OnButtonValueChange(float value)
    {
        Debug.Log($"Button value changed to {value}");
        /* F2 means fixed-point notation with 2 decimal places */
        _buttonValueText.text = value.ToString("F2");
    }
}
