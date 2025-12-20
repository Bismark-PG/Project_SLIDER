using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIToggleEffect : MonoBehaviour
{
    [Header("Settings")]
    public bool isOn = false;
    public GameObject checkmarkObject;

    [Header("Events")]
    public UnityEvent<bool> onToggleChanged;

    private Button btn;

    void Awake()
    {
        btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(OnButtonClicked);
        }
    }

    public void SetInitialState(bool state)
    {
        isOn = state;
        UpdateVisual();
    }

    void OnButtonClicked()
    {
        isOn = !isOn;
        UpdateVisual();

        onToggleChanged?.Invoke(isOn);
    }

    void UpdateVisual()
    {
        if (checkmarkObject != null)
        {
            checkmarkObject.SetActive(isOn);
        }
    }
}