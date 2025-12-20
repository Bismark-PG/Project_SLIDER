using UnityEngine;

public class Treadmill : MonoBehaviour
{
    [Header("Speed Settings")]
    public float maxSpeed = 10.0f;
    public float accelerationTime = 3.0f;

    [Header("Stopping Logic")]
    public Transform endPoint;
    public Transform stopTarget;
    public float slowDownDistance = 10.0f;

    [Header("Clear Event")]
    public GameObject clearOrb;
    public GameObject boundaryWall;
    public DeadWall wallSwitcher;

    private float currentSpeed = 0f;
    private float accelTimer = 0f;
    private bool isRunning = false;

    private Vector3 initialPosition;
    private Move_Chara playerScript;

    void Start()
    {
        playerScript = FindAnyObjectByType<Move_Chara>();
        initialPosition = transform.position;

        if (clearOrb != null)
            clearOrb.SetActive(false);

        if (boundaryWall != null)
            boundaryWall.SetActive(true);
    }

    public void ResetPlatform()
    {
        isRunning = false;
        currentSpeed = 0f;
        accelTimer = 0f;
        transform.position = initialPosition;

        if (clearOrb != null) clearOrb.SetActive(false);

        if (boundaryWall != null)
            boundaryWall.SetActive(true);

        if (playerScript != null)
            playerScript.isTreadmillMode = false;
    }

    public void ActivateTreadmill()
    {
        isRunning = true;
        accelTimer = 0f;

        if (clearOrb != null)
            clearOrb.SetActive(false);

        if (boundaryWall != null)
            boundaryWall.SetActive(true);
    }

    void Update()
    {
        if (!isRunning) return;

        Vector3 endPosFlat = new Vector3(endPoint.position.x, 0, endPoint.position.z);
        Vector3 stopPosFlat = new Vector3(stopTarget.position.x, 0, stopTarget.position.z);
        float distance = Vector3.Distance(endPosFlat, stopPosFlat);

        if (distance < 0.5f)
        {
            StopTreadmill();
            return;
        }

        if (distance <= slowDownDistance)
        {
            float ratio = distance / slowDownDistance;

            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.SetBGMVolume(ratio);
            }

            if (boundaryWall != null && boundaryWall.activeSelf)
            {
                boundaryWall.SetActive(false);
                Debug.Log("Stage End");
            }

            if (wallSwitcher != null)
                wallSwitcher.ResetWall();

            currentSpeed = Mathf.Lerp(0, maxSpeed, ratio);

            if (currentSpeed < 0.5f)
                currentSpeed = 0.5f;
        }
        else
        {
            if (accelTimer < accelerationTime)
            {
                accelTimer += Time.deltaTime;
                currentSpeed = Mathf.Lerp(0, maxSpeed, accelTimer / accelerationTime);
            }
            else
            {
                currentSpeed = maxSpeed;
            }
        }

        transform.Translate(Vector3.back * currentSpeed * Time.deltaTime, Space.World);
    }

    void StopTreadmill()
    {
        currentSpeed = 0;
        isRunning = false;

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetBGMVolume(0f);
            SoundManager.Instance.PlaySFX(SoundManager.Instance.TreadmillDone);
        }

        if (clearOrb != null)
            clearOrb.SetActive(true);

        if (boundaryWall != null)
            boundaryWall.SetActive(false);

        if (wallSwitcher != null)
            wallSwitcher.ResetWall();

        if (playerScript != null)
        {
            playerScript.isTreadmillMode = false;
            Debug.Log("Treadmill End.");
        }
    }
}