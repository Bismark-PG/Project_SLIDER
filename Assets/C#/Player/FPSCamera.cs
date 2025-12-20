using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    [Header("Sensitivity")]
    public float mouseSensitivity = 200f;

    [Header("References")]
    public Transform playerBody;  
    public Move_Chara moveScript; 

    [Header("Sliding Look Settings")]
    public float maxLookAngle = 80f; 
    public float shoulderTilt = 0.5f;
    public float slideFixedAngle = -25f;

    float xRotation = 0f;
    float yRotation = 0f;

    bool isInvertY = false;

    bool wasSliding = false;

    void Start()
    {
        UpdateSettings();

        Time.timeScale = 1.0f;

        if (playerBody == null) playerBody = transform.parent;
        if (moveScript == null) moveScript = FindAnyObjectByType<Move_Chara>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UpdateSettings()
    {
        float loadedSens = PlayerPrefs.GetFloat("Sensitivity", 200f);

        if (loadedSens < 1f)
        {
            loadedSens = 200f;
            Debug.LogWarning("Sensitivity Pick");
        }

        mouseSensitivity = loadedSens;
        isInvertY = PlayerPrefs.GetInt("InvertY", 0) == 1;

        Debug.Log($"Final Sensitivity: {mouseSensitivity}");
    }

    void Update()
    {
        if (mouseSensitivity <= 0 || Time.timeScale == 0) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        bool isSliding = (moveScript != null && moveScript.IsSliding());

        if (isSliding != wasSliding)
        {
            if (isSliding)
            {
                yRotation = 0f;
            }
            else
            {
                xRotation = 0f;
            }
            wasSliding = isSliding;
        }

        if (isSliding)
        {
            xRotation = slideFixedAngle;

            yRotation += mouseX;
            yRotation = Mathf.Clamp(yRotation, -maxLookAngle, maxLookAngle);

            float tiltZ = -yRotation * shoulderTilt;

            transform.localRotation = Quaternion.Euler(xRotation, yRotation, tiltZ);
        }
        else
        {
            if (isInvertY)
                xRotation += mouseY;
            else
                xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, -45f, 45f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            if (playerBody != null)
            {
                playerBody.Rotate(Vector3.up * mouseX);
            }
        }
    }
}