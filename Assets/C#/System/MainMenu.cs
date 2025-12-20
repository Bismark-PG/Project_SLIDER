using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene Name")]
    public string gameSceneName = "GameScene";

    [Header("UI Panels")]
    public GameObject settingsPanel;
    void Start()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayBGM(SoundManager.Instance.Title);
    }

    public void ClickStart()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.StopAllSounds();

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX(SoundManager.Instance.MenuSelect);

        SceneManager.LoadScene(gameSceneName);
    }

    public void ClickSettings()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX(SoundManager.Instance.MenuSelect);

        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public void ClickCloseSettings()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX(SoundManager.Instance.MenuSelect);

        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void ClickExit()
    {
        Debug.Log("Game Done");
        Application.Quit();
    }
}