using UnityEngine;

public class GoalDetector : MonoBehaviour
{
    [Header("Settings")]
    public string goalTag = "Finish";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(goalTag))
        {
            Move_Chara player = FindAnyObjectByType<Move_Chara>();
            if (player != null)
            {
                player.isTreadmillMode = false;
                Debug.Log("Find Goal Detector");
            }
        }
    }
}