using UnityEngine;

public class SwitchGate : MonoBehaviour
{
    [Header("Linked Switches")]
    public Switch[] requiredSwitches;

    [Header("Gate Object")]
    public GameObject gateObject;

    private bool isOpened = false;

    void Start()
    {
        if (gateObject == null) gateObject = gameObject;

        if (requiredSwitches == null || requiredSwitches.Length == 0)
        {
            requiredSwitches = GetComponentsInChildren<Switch>();
        }

        foreach (Switch sw in requiredSwitches)
        {
            if (sw != null)
            {
                sw.linkedGate = this;
            }
        }
    }

    public void CheckSwitches()
    {
        if (isOpened) return;

        foreach (Switch sw in requiredSwitches)
        {
            if (sw != null && !sw.IsActivated)
            {
                return;
            }
        }

        OpenGate();
    }

    void OpenGate()
    {
        isOpened = true;
        Debug.Log("All Switches Activated! Gate Open.");

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(SoundManager.Instance.MenuSelect);
        }

        if (gateObject != null)
        {
            gateObject.SetActive(false);
        }
    }

    public void ResetGate()
    {
        isOpened = false;
        if (gateObject != null)
        {
            gateObject.SetActive(true);
        }
    }
}