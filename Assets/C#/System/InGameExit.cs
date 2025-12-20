using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameExit : MonoBehaviour
{
    [Header("Exit Settings")]
    public string mainMenuSceneName = "MainMenu";
    public KeyCode interactionKey = KeyCode.E;
    public GameObject interactionTextUI;

    private bool isPlayerInZone = false;

    void Start()
    {
        if (interactionTextUI != null) interactionTextUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            if (interactionTextUI != null) interactionTextUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            if (interactionTextUI != null) interactionTextUI.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(interactionKey))
        {
            GoToMainMenu();
        }
    }

    void GoToMainMenu()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.StopAllSounds();
            SoundManager.Instance.PlaySFX(SoundManager.Instance.MenuSelect);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(mainMenuSceneName);
    }
}