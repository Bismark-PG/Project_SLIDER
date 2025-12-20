using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Components")]
    public GameObject player;
    public Move_Chara playerMovement;
    public LaserGun playerGun;
    public CanvasGroup bloodScreen;

    [Header("Camera Settings")]
    public GameObject mainCamera; 
    public GameObject deathCamera;

    [Header("Settings")]
    public Transform hubSpawnPoint;
    public float respawnDelay = 3.0f;

    [Header("Stage Objects for Reset")]
    public GameObject[] StageTrigger; 
    public MovingWall[] StageWall;
    public Treadmill[] StageFloor;
    public DeadWall[] wallSwitchers;
    public Switch[] stageSwitches;
    public SwitchGate[] stageGates;
    public BreakableObject[] stageBreakables;

    [Header("Stage Progress")]
    public bool isStage1Clear = false;
    public bool isStage2Clear = false;
    public bool isStage3Clear = false;
    public bool isStage3CheckpointReached = false;

    [Header("Stage 3 Traps")]
    public TrapTrigger[] stageTraps;

    public void CompleteStage(int stageIndex)
    {
        switch (stageIndex)
        {
            case 1:
                isStage1Clear = true;
                break;

            case 2:
                isStage2Clear = true;
                break;

            case 3:
                isStage3Clear = true; 
                break;
        }

        Debug.Log($"Stage {stageIndex} Clear!");
    }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OnPlayerDied()
    {
        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.StopAllSounds();
            SoundManager.Instance.PlaySFX(SoundManager.Instance.Death);
        }

        if (playerMovement != null)
        {
            playerMovement.ResetVisualsOnDeath();
        }

        playerMovement.enabled = false;
        playerGun.enabled = false;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        mainCamera.SetActive(false);
        deathCamera.SetActive(true);

        float t = 0;
        while (t < 1.0f)
        {
            t += Time.deltaTime * 2.0f;
            bloodScreen.alpha = t * 0.5f;
            yield return null;
        }

        yield return new WaitForSeconds(respawnDelay);

        rb.isKinematic = false;

        ResetStage();
        RespawnPlayer();

        mainCamera.SetActive(true);
        deathCamera.SetActive(false);

        while (t > 0.0f)
        {
            t -= Time.deltaTime;
            bloodScreen.alpha = t * 0.5f;
            yield return null;
        }
    }

    public void ResetStage()
    {
        Debug.Log("Stage Reset");

        if (playerMovement != null)
            playerMovement.isTreadmillMode = false;

        foreach (var trigger in StageTrigger)
        {
            if (trigger != null) trigger.SetActive(true);
        }

        foreach (var wall in StageWall)
        {
            if (wall != null) wall.ResetWall();
        }

        foreach (var floor in StageFloor)
        {
            if (floor != null) floor.ResetPlatform();
        }

        foreach (var switcher in wallSwitchers)
        {
            if (switcher != null) switcher.ResetWall();
        }

        foreach (var sw in stageSwitches)
        {
            if (sw != null) sw.ResetSwitch();
        }

        foreach (var gate in stageGates)
        {
            if (gate != null) gate.ResetGate();
        }

        foreach (var trap in stageTraps)
        {
            if (trap != null) trap.ResetTrap();
        }

        foreach (var wall in stageBreakables)
        {
            if (wall != null) wall.ResetWall();
        }
    }

    void RespawnPlayer()
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (hubSpawnPoint != null)
        {
            player.transform.position = hubSpawnPoint.position;
            player.transform.rotation = Quaternion.Euler(0, hubSpawnPoint.eulerAngles.y, 0);
        }

        if (playerMovement != null)
        {
            playerMovement.enabled = true;
            playerMovement.Revive();
        }

        if (playerGun != null) playerGun.enabled = true;

        if (mainCamera != null)
        {
            mainCamera.SetActive(true);
            FPSCamera camScript = mainCamera.GetComponent<FPSCamera>();

            if (camScript != null)
                camScript.enabled = true;
        }
    }
}