using UnityEngine;

public class RecoilSystem : MonoBehaviour
{
    [Header("Recoil Settings")]
    public Vector3 recoilPosition = new Vector3(0, 0.1f, -0.2f);
    public Vector3 recoilRotation = new Vector3(-10f, 0, 0);

    [Header("Speed Settings")]
    public float snappiness = 6f;
    public float returnSpeed = 2f;

    private Vector3 currentRecoilPos;
    private Vector3 targetRecoilPos;
    private Vector3 currentRecoilRot;
    private Vector3 targetRecoilRot;

    void Update()
    {
        targetRecoilPos = Vector3.Lerp(targetRecoilPos, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRecoilPos = Vector3.Lerp(currentRecoilPos, targetRecoilPos, snappiness * Time.deltaTime);

        targetRecoilRot = Vector3.Lerp(targetRecoilRot, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRecoilRot = Vector3.Lerp(currentRecoilRot, targetRecoilRot, snappiness * Time.deltaTime);

        transform.localPosition = currentRecoilPos;
        transform.localRotation = Quaternion.Euler(currentRecoilRot);
    }

    public void Fire()
    {
        targetRecoilPos += recoilPosition;
        targetRecoilRot += recoilRotation;
    }
}