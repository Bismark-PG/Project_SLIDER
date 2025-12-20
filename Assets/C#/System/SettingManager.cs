using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    [Header("UI References")]
    public Slider volumeSlider;
    public Slider sensitivitySlider;
    public UIToggleEffect invertYButton;
    public UIToggleEffect sprintButton;

    private FPSCamera playerCamera;
    private Move_Chara playerMove;

    void OnEnable()
    {
        if (playerCamera == null) playerCamera = FindAnyObjectByType<FPSCamera>();
        if (playerMove == null) playerMove = FindAnyObjectByType<Move_Chara>();

        RefreshUI();
    }

    void RefreshUI()
    {
        float savedVol = PlayerPrefs.GetFloat("Volume", 0.75f);
        float savedSens = PlayerPrefs.GetFloat("Sensitivity", 200.0f);
        bool savedInvertY = PlayerPrefs.GetInt("InvertY", 0) == 1;
        bool savedSprintToggle = PlayerPrefs.GetInt("SprintToggle", 0) == 1;

        if (volumeSlider != null) volumeSlider.value = savedVol;
        if (sensitivitySlider != null) sensitivitySlider.value = savedSens;
        if (invertYButton != null) invertYButton.SetInitialState(savedInvertY);
        if (sprintButton != null) sprintButton.SetInitialState(savedSprintToggle);

        AudioListener.volume = savedVol;

        Debug.Log($"[UI Init] Sensitivity : {savedSens} / InvertY : {savedInvertY} / Toggle : {savedSprintToggle}");
    }
    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }

    public void SetSensitivity(float value)
    {
        Debug.Log($"Sensitivity Shange! Value : {value}");

        PlayerPrefs.SetFloat("Sensitivity", value);
        PlayerPrefs.Save();

        if (playerCamera != null)
        {
            playerCamera.UpdateSettings();
            Debug.Log(" >> Send To Camera");
        }
    }

    public void SetInvertY(bool isOn)
    {
        Debug.Log($"InvertY : {isOn}");

        PlayerPrefs.SetInt("InvertY", isOn ? 1 : 0);
        PlayerPrefs.Save();
        if (playerCamera != null) playerCamera.UpdateSettings();
    }

    public void SetSprintToggle(bool isOn)
    {
        Debug.Log($"Toggle Able: {isOn}");

        PlayerPrefs.SetInt("SprintToggle", isOn ? 1 : 0);
        PlayerPrefs.Save();
        if (playerMove != null) playerMove.UpdateSettings();
    }
}