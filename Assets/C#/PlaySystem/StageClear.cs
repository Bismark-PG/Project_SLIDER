using UnityEngine;

public class StageClear : MonoBehaviour
{
    [Header("Stage Info")]
    public int stageIndex = 1;

    [Header("Teleport Settings")]
    public Transform hubSpawnPoint;
    public KeyCode interactionKey = KeyCode.E;

    [Header("Clear Effect")]
    public GameObject clearTextUI;

    [Header("Interaction UI")]
    public GameObject interactionTextUI;

    private bool isPlayerInZone = false;
    private GameObject playerObject = null;

    void Start()
    {
        if (interactionTextUI != null) interactionTextUI.SetActive(false);
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
        }
    }

    void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(interactionKey))
        {
            ReturnToHub();
        }
    }

    void ReturnToHub()
    {
        if (playerObject == null || hubSpawnPoint == null)
            return;

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.FadeOutBGM(2.0f);

            SoundManager.Instance.PlaySFX(SoundManager.Instance.MenuSelect);
        }

        playerObject.transform.position = hubSpawnPoint.position;
        playerObject.transform.rotation = hubSpawnPoint.rotation;

        Rigidbody rb = playerObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.CompleteStage(stageIndex);

            GameManager.Instance.ResetStage();
        }

        if (clearTextUI != null)
        {
            clearTextUI.SetActive(true);
        }

        Debug.Log($"Stage {stageIndex} Clear & Return to Hub");
    }
}