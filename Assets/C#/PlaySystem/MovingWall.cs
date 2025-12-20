using UnityEngine;

public class MovingWall : MonoBehaviour
{
    [Header("Settings")]
    public Transform targetPosition;
    public float moveSpeed = 15.0f;

    private bool isActivated = false;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }
    public void ResetWall()
    {
        isActivated = false;
        transform.position = initialPosition;
        gameObject.SetActive(true);
    }

    public void ActivateWall()
    {
        isActivated = true;
    }

    void Update()
    {
        if (!isActivated || targetPosition == null) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition.position) < 0.1f)
        {
            gameObject.SetActive(false);
            isActivated = false;
        }
    }
}