using UnityEngine;

public class StartTrigger : MonoBehaviour
{
    [Header("Target Scripts")]
    public MovingWall wallScript;
    public Treadmill floorScript;
    public DeadWall wallSwitcher;

    [Header("Settings")]
    public KeyCode interactionKey = KeyCode.E;
    public GameObject interactionText;

    private bool isPlayerInZone = false;
    private Move_Chara playerScript;
    void OnEnable()
    {
        isPlayerInZone = false;
        playerScript = null;
        if (interactionText != null) interactionText.SetActive(false);
    }

    void Start()
    {
        if (interactionText != null) interactionText.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(interactionKey))
        {
            StartStage();
        }
    }

    void StartStage()
    {
        if (wallScript != null)
            wallScript.ActivateWall();

        if (floorScript != null)
            floorScript.ActivateTreadmill();

        if (wallSwitcher != null)
            wallSwitcher.ActivateDeadZone();

        if (playerScript != null)
        {
            playerScript.isTreadmillMode = true;
            Debug.Log("Treadmill Start");
        }

        if (interactionText != null)
            interactionText.SetActive(false);

        gameObject.SetActive(false);

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.StopAllSounds();
            SoundManager.Instance.PlaySFX(SoundManager.Instance.StageStart);

            SoundManager.Instance.PlaySFX(SoundManager.Instance.TreadmillStart);

            SoundManager.Instance.PlayBGMWithFadeIn(SoundManager.Instance.StageBGM, 2.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            playerScript = other.GetComponent<Move_Chara>();
            if (interactionText != null) interactionText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            playerScript = null;
            if (interactionText != null) interactionText.SetActive(false);
        }
    }
}