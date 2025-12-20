using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [Header("Settings")]
    public GameObject brokenPrefab;
    public float explosionForce = 500f;
    public float explosionRadius = 3f;

    [Header("Sound Settings")]
    public bool isBigWall = false;

    [Header("Live Time")]
    public float minDestroyTime = 0.5f;
    public float maxDestroyTime = 2.0f;

    public void Break(Vector3 hitPoint)
    {
        if (SoundManager.Instance != null)
        {
            if (isBigWall)
                SoundManager.Instance.PlaySFX(SoundManager.Instance.BigWallDestroy);
            else
                SoundManager.Instance.PlaySFX(SoundManager.Instance.WallDestroy);
        }

        if (brokenPrefab != null)
        {
            Material originalMat = null;
            Renderer myRenderer = GetComponent<Renderer>();
            if (myRenderer != null) originalMat = myRenderer.material;

            GameObject brokenObj = Instantiate(brokenPrefab, transform.position, transform.rotation);

            float targetSize = transform.localScale.x;
            brokenObj.transform.localScale = new Vector3(targetSize, transform.localScale.y, targetSize);

            Renderer[] childRenderers = brokenObj.GetComponentsInChildren<Renderer>();
            Rigidbody[] rbs = brokenObj.GetComponentsInChildren<Rigidbody>();

            if (originalMat != null)
            {
                foreach (Renderer childRenderer in childRenderers) childRenderer.material = originalMat;
            }

            foreach (Rigidbody rb in rbs)
            {
                rb.AddExplosionForce(explosionForce, hitPoint, explosionRadius);
                float randomLifeTime = Random.Range(minDestroyTime, maxDestroyTime);
                Destroy(rb.gameObject, randomLifeTime);
            }

            Destroy(brokenObj, maxDestroyTime + 0.5f);
        }

        gameObject.SetActive(false);
    }

    public void ResetWall()
    {
        gameObject.SetActive(true);
    }
}