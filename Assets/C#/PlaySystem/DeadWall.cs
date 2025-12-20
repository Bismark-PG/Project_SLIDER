using UnityEngine;

public class DeadWall : MonoBehaviour
{
    [Header("Walls")]
    public GameObject safeWall;
    public GameObject deadWall;

    void Start()
    {
        ResetWall();
    }

    public void ActivateDeadZone()
    {
        if (safeWall != null) safeWall.SetActive(false);
        if (deadWall != null) deadWall.SetActive(true);
    }

    public void ResetWall()
    {
        if (safeWall != null) safeWall.SetActive(true);
        if (deadWall != null) deadWall.SetActive(false);
    }
}