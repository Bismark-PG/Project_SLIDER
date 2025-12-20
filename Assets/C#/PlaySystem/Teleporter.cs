using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform teleportTarget;
    public KeyCode interactionKey = KeyCode.E;

    [Header("Lock Settings")]
    public int requiredStage = 0;
    public GameObject lockUI;
    public float uiDisplayTime = 3.0f;

    [Header("UI")]
    public GameObject interactionTextUI;

    private bool isPlayerInZone = false;
    private GameObject playerObject = null;
    private bool isUiActive = false;

    void Start()
    {
        if (interactionTextUI != null) interactionTextUI.SetActive(false);
        if (lockUI != null) lockUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            playerObject = other.gameObject;
            if (interactionTextUI != null) interactionTextUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            playerObject = null;
            if (interactionTextUI != null) interactionTextUI.SetActive(false);
            if (lockUI != null) lockUI.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(interactionKey))
        {
            TryTeleport();
        }
    }

    void TryTeleport()
    {
        if (!CheckPermission())
        {
            ShowWarning();
            return;
        }
        Teleport();
    }

    bool CheckPermission()
    {
        if (GameManager.Instance == null)
            return true;

        if (requiredStage == 0)
            return true;

        if (requiredStage == 1)
            return GameManager.Instance.isStage1Clear;

        if (requiredStage == 2)
            return GameManager.Instance.isStage2Clear;

        return true;
    }

    void ShowWarning()
    {
        if (isUiActive)
            return;

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(SoundManager.Instance.Denied);
        }

        StartCoroutine(ShowUIRoutine());
    }

    IEnumerator ShowUIRoutine()
    {
        isUiActive = true;
        if (lockUI != null)
            lockUI.SetActive(true);

        yield return new WaitForSeconds(uiDisplayTime);
     
        if (lockUI != null)
            lockUI.SetActive(false);

        isUiActive = false;
    }

    void Teleport()
    {
        if (playerObject == null || teleportTarget == null)
            return;

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.StopAllSounds();
            SoundManager.Instance.PlaySFX(SoundManager.Instance.MenuSelect);
        }

        playerObject.transform.position = teleportTarget.position;

        Rigidbody rb = playerObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log("Teleport done");
    }
}