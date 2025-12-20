using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public void OnNormalClick()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.MenuSelect);
    }

    public void OnToggleClick()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.SettingButtonClick);
    }
}