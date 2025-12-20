using UnityEngine;

public class ShortcutDoor : MonoBehaviour
{
    [Header("Target Object")]
    public GameObject shortcutObject;

    void Update()
    {
        if (shortcutObject == null) return;

        if (GameManager.Instance != null)
        {
            bool isUnlocked = GameManager.Instance.isStage3CheckpointReached;

            if (shortcutObject.activeSelf != isUnlocked)
            {
                shortcutObject.SetActive(isUnlocked);
            }
        }
    }
}