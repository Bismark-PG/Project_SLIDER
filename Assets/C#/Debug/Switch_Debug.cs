using UnityEngine;

public class Switch_Debug : MonoBehaviour
{
    public GameObject mainCamera; 
    public GameObject debugCamera;

    public KeyCode switchKey = KeyCode.F1;

    void Start()
    {
        if (mainCamera != null) mainCamera.SetActive(true);
        if (debugCamera != null) debugCamera.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            ToggleCamera();
        }
    }

    void ToggleCamera()
    {
        bool isMainActive = mainCamera.activeSelf;

        mainCamera.SetActive(!isMainActive);
        debugCamera.SetActive(isMainActive);

        if (isMainActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Debug.Log("Camera Switched! Debug Mode: " + isMainActive);
    }
}