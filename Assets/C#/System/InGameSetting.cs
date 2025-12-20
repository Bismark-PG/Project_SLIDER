using UnityEngine;

public class InGameSetting : MonoBehaviour
{
    [Header("Settings UI")]
    public GameObject settingsPanel;
    public KeyCode interactionKey = KeyCode.E;
    public GameObject interactionTextUI;

    private bool isPlayerInZone = false;

    private Move_Chara moveScript;
    private LaserGun gunScript;
    private FPSCamera camScript;

    void Start()
    {
        if (interactionTextUI != null) interactionTextUI.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            if (interactionTextUI != null) interactionTextUI.SetActive(true);

            moveScript = other.GetComponent<Move_Chara>();
            gunScript = other.GetComponentInChildren<LaserGun>();
            camScript = other.GetComponentInChildren<FPSCamera>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            if (interactionTextUI != null) interactionTextUI.SetActive(false);
            CloseSettings();
        }
    }

    void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(interactionKey))
        {
            if (settingsPanel != null && !settingsPanel.activeSelf)
                OpenSettings();
            else
                CloseSettings();
        }
    }

    public void OpenSettings()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX(SoundManager.Instance.MenuSelect);

        if (settingsPanel != null)
            settingsPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (moveScript != null)
            moveScript.enabled = false;

        if (gunScript != null)
            gunScript.enabled = false;

        if (camScript != null)
            camScript.enabled = false;
    }

    public void CloseSettings()
    {
        if (settingsPanel != null && settingsPanel.activeSelf && SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX(SoundManager.Instance.MenuSelect);

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (moveScript == null)
            moveScript = FindAnyObjectByType<Move_Chara>();

        if (gunScript == null)
            gunScript = FindAnyObjectByType<LaserGun>();

        if (camScript == null)
            camScript = FindAnyObjectByType<FPSCamera>();

        if (moveScript != null)
            moveScript.enabled = true;

        if (gunScript != null)
            gunScript.enabled = true;

        if (camScript != null)
            camScript.enabled = true;
    }
} 