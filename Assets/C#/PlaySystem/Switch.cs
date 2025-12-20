using UnityEngine;

public class Switch : MonoBehaviour
{
    [Header("Link Settings")]
    public SwitchGate linkedGate;

    [Header("Material Settings")]
    public Material inactiveMaterial;
    public Material activeMaterial;

    public bool IsActivated { get; private set; } = false;

    private Renderer myRenderer;

    void Start()
    {
        myRenderer = GetComponent<Renderer>();
    }

    public void Activate()
    {
        if (IsActivated) return;

        IsActivated = true;

        if (myRenderer != null && activeMaterial != null)
        {
            myRenderer.material = activeMaterial;
        }

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(SoundManager.Instance.SwitchOff);
        }

        if (linkedGate != null)
        {
            linkedGate.CheckSwitches();
        }
    }

    public void ResetSwitch()
    {
        IsActivated = false;

        if (myRenderer != null && inactiveMaterial != null)
        {
            myRenderer.material = inactiveMaterial;
        }
    }
}