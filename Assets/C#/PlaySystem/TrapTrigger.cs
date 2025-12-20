using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    [Header("Trigger Switch")]
    public Switch triggerSwitch;

    [Header("Fake Floor Settings")]
    public GameObject wallPrefab;
    public Transform wallSpawnPoint;

    [Header("Chasing Wall")]
    public ChasingWall chasingWallScript;

    private GameObject currentWallInstance;
    private bool isTrapActivated = false;

    void Start()
    {
        SpawnFakeWall();
    }

    void Update()
    {
        if (triggerSwitch != null && triggerSwitch.IsActivated && !isTrapActivated)
        {
            ActivateTrap();
        }
    }

    void ActivateTrap()
    {
        isTrapActivated = true;
        Debug.Log("TRAP ACTIVATED!");

        if (GameManager.Instance != null && !GameManager.Instance.isStage3CheckpointReached)
        {
            GameManager.Instance.isStage3CheckpointReached = true;
            Debug.Log("Stage 3 Shortcut Unlocked");
        }

        if (currentWallInstance != null)
        {
            BreakableObject breakScript = currentWallInstance.GetComponent<BreakableObject>();
            if (breakScript != null)
            {
                breakScript.Break(currentWallInstance.transform.position);
            }
            else
            {
                Destroy(currentWallInstance);
            }

            if (SoundManager.Instance != null)
                SoundManager.Instance.PlaySFX(SoundManager.Instance.BigWallDestroy);
        }

        if (chasingWallScript != null)
        {
            chasingWallScript.StartChasing();
        }

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBGMWithFadeIn(SoundManager.Instance.BossBGM, 0.5f);
        }
    }

    public void ResetTrap()
    {
        isTrapActivated = false;
        if (triggerSwitch != null) triggerSwitch.ResetSwitch();
        SpawnFakeWall();
        if (chasingWallScript != null) chasingWallScript.ResetWall();
    }

    void SpawnFakeWall()
    {
        if (currentWallInstance != null) Destroy(currentWallInstance);
        if (wallPrefab != null && wallSpawnPoint != null)
        {
            currentWallInstance = Instantiate(wallPrefab, wallSpawnPoint.position, wallSpawnPoint.rotation);
        }
    }
}