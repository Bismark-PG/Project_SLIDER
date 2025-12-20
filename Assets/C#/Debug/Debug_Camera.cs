using UnityEngine;

public class Debug_Camera : MonoBehaviour
{
  public float moveSpeed = 10f;
    public float lookSpeed = 2f;

    float rotationX = 0f;
    float rotationY = 0f;

    public KeyCode UP   = KeyCode.Q;
    public KeyCode DOWN = KeyCode.E;

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            rotationX += Input.GetAxis("Mouse X") * lookSpeed;
            rotationY += Input.GetAxis("Mouse Y") * lookSpeed;
            rotationY = Mathf.Clamp(rotationY, -90, 90);

            transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
            transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
        }

        float moveX = 0;
        float moveZ = 0;
        float moveY = 0;

        if (Input.GetKey(KeyCode.RightArrow))
            moveX = 1;

        if (Input.GetKey(KeyCode.LeftArrow))
            moveX = -1;

        if (Input.GetKey(KeyCode.UpArrow))
            moveZ = 1;

        if (Input.GetKey(KeyCode.DownArrow))
            moveZ = -1;

        if (Input.GetKey(UP))
            moveY = -1; // Q

        if (Input.GetKey(DOWN))
            moveY = 1;  // E

        Vector3 moveDir = (Vector3.right * moveX) + (Vector3.forward * moveZ);
        
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
        transform.Translate(Vector3.up * moveY * moveSpeed * Time.deltaTime, Space.World);
    }
}