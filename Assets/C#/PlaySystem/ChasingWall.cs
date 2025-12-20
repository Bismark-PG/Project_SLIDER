using UnityEngine;

public class ChasingWall : MonoBehaviour
{
    [Header("Chasing Settings")]
    public float moveSpeed = 10.0f;
    public float maxDistance = 20.0f;

    [Header("Target")]
    public Transform playerTransform;

    private bool isChasing = false;

    private Vector3 startPosition;
    private float fixedX;
    private float fixedY;

    void Awake()
    {
        startPosition = transform.position;
        fixedX = transform.position.x;
        fixedY = transform.position.y;
    }

    void OnEnable()
    {
        if (playerTransform == null)
        {
            Move_Chara player = FindAnyObjectByType<Move_Chara>();
            if (player != null) playerTransform = player.transform;
        }
    }

    public void StartChasing()
    {
        fixedX = transform.position.x;
        fixedY = transform.position.y;

        gameObject.SetActive(true);
        isChasing = true;
    }

    public void ResetWall()
    {
        isChasing = false;
        transform.position = startPosition;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isChasing || playerTransform == null)
            return;

        float currentZ = transform.position.z;
        float playerZ = playerTransform.position.z;

        float distance = playerZ - currentZ;

        if (distance > maxDistance)
        {
            currentZ = playerZ - maxDistance;
        }
        else
        {
            currentZ += moveSpeed * Time.deltaTime;
        }

        transform.position = new Vector3(fixedX, fixedY, currentZ);
    }
}