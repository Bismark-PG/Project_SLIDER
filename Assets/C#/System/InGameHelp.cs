using UnityEngine;

public class InGameHelp : MonoBehaviour
{
    [Header("UI Objects")]
    public GameObject helpPanel;       
    public KeyCode interactionKey = KeyCode.E;
    public GameObject interactionTextUI; 

    private bool isPlayerInZone = false;

    private Move_Chara moveScript;
    private LaserGun gunScript;
    private FPSCamera camScript;

    void Start()
    {
        if (interactionTextUI != null)
            interactionTextUI.SetActive(false);

        if (helpPanel != null)
            helpPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            if (interactionTextUI != null) 
                interactionTextUI.SetActive(true);

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
            if (interactionTextUI != null)
                interactionTextUI.SetActive(false);

            CloseHelp();
        }
    }

    void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(interactionKey))
        {
            if (helpPanel != null && !helpPanel.activeSelf)
                OpenHelp();
            else
                CloseHelp();
        }
    }

    public void OpenHelp()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX(SoundManager.Instance.MenuSelect);

        if (helpPanel != null)
            helpPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (moveScript != null)
            moveScript.enabled = false;

        if (gunScript != null)
            gunScript.enabled = false;

        if (camScript != null)
            camScript.enabled = false;
    }

    public void CloseHelp()
    {
        if (helpPanel != null && helpPanel.activeSelf && SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX(SoundManager.Instance.MenuSelect);

        if (helpPanel != null)
            helpPanel.SetActive(false);

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