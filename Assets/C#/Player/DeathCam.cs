using UnityEngine;

public class DeathCam : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 3.0f, -3.0f);

    void OnEnable()
    {
        if (target == null) return;

        transform.position = target.position + offset;

        transform.LookAt(target.position);

    }
}