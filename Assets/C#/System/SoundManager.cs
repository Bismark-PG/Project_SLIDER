using UnityEngine;
using System.Collections;

[System.Serializable]
public class SoundData
{
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1.0f;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Source")]
    public AudioSource SFXSource;
    public AudioSource BGMSource;

    [Header("Player")]
    public SoundData Jump; 
    public SoundData Land;
    public SoundData Walk;
    public SoundData Slide;
    public SoundData Death;

    [Header("Weapon")]
    public SoundData Fire;
    public SoundData Reload;

    [Header("UI & System")]
    public SoundData MenuSelect;
    public SoundData SettingButtonClick;
    public SoundData Title;
    public SoundData StageStart;
    public SoundData Denied;

    [Header("Game Play")]
    public SoundData TreadmillStart;
    public SoundData TreadmillDone;
    public SoundData SwitchOff;
    public SoundData SwitchDoorOpen;
    public SoundData WallDestroy;
    public SoundData BigWallDestroy;

    [Header("BGM")]
    public SoundData StageBGM;
    public SoundData BossBGM;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayBGM(Title);
    }

    public void PlaySFX(SoundData data)
    {
        if (data != null && data.clip != null && SFXSource != null)
        {
            SFXSource.PlayOneShot(data.clip, data.volume);
        }
    }
    public void PlayBGM(SoundData data)
    {
        if (data != null && data.clip != null && BGMSource != null)
        {
            StopAllCoroutines();
            BGMSource.clip = data.clip;
            BGMSource.volume = data.volume;
            BGMSource.loop = true;
            BGMSource.Play();
        }
    }
    public void PlayBGMWithFadeIn(SoundData data, float fadeTime = 2.0f)
    {
        if (data != null && data.clip != null && BGMSource != null)
        {
            StartCoroutine(FadeInRoutine(data, fadeTime));
        }
    }

    IEnumerator FadeInRoutine(SoundData data, float duration)
    {
        BGMSource.Stop();
        BGMSource.clip = data.clip;
        BGMSource.loop = true;
        BGMSource.volume = 0f;
        BGMSource.Play();

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            BGMSource.volume = Mathf.Lerp(0f, data.volume, timer / duration);
            yield return null;
        }
        BGMSource.volume = data.volume;
    }

    public void SetBGMVolume(float ratio)
    {
        if (BGMSource != null && StageBGM != null)
        {
            BGMSource.volume = Mathf.Lerp(0f, StageBGM.volume, ratio);
        }
    }

    public void StopAllSounds()
    {
        if (SFXSource != null)
            SFXSource.Stop();

        if (BGMSource != null)
            BGMSource.Stop();
    }

    public void FadeOutBGM(float duration)
    {
        if (BGMSource != null && BGMSource.isPlaying)
        {
            StartCoroutine(FadeOutRoutine(duration));
        }
    }

    IEnumerator FadeOutRoutine(float duration)
    {
        float startVolume = BGMSource.volume;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            BGMSource.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
            yield return null;
        }

        BGMSource.Stop();
        BGMSource.volume = startVolume;
    }
}