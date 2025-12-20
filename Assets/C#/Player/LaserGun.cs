using UnityEngine;
using System.Collections;
using System.Globalization;

public class LaserGun : MonoBehaviour
{
    [Header("Laser Settings")]
    public float damage = 10f;
    public float fireRate = 0.75f;
    public float laserRange = 100f;
    public float laserDuration = 0.1f;

    [Header("Hit Layers")]
    public LayerMask hitLayers;

    [Header("References")]
    public Transform firePoint;
    public Camera fpsCamera;
    public LineRenderer laserLine;

    [Header("Recoil Connection")]
    public RecoilSystem recoilSystem;

    [Header("Key Bindings")]
    public KeyCode fireKey = KeyCode.Mouse0;

    private float nextFireTime = 0f;
    private bool isReloadSoundPlayed = true;

    void Start()
    {
        if (laserLine != null)
            laserLine.enabled = false;

        nextFireTime = 0f;
        isReloadSoundPlayed = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(fireKey) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            StartCoroutine(ShootLaser());

            isReloadSoundPlayed = false;
        }

        if (!isReloadSoundPlayed && Time.time >= nextFireTime)
        {
            SoundManager.Instance.PlaySFX(SoundManager.Instance.Reload);
            isReloadSoundPlayed = true;
        }
    }

    IEnumerator ShootLaser()
    {
        if (recoilSystem != null)
        {
            recoilSystem.Fire();
        }

        laserLine.enabled = true;

        SoundManager.Instance.PlaySFX(SoundManager.Instance.Fire);

        laserLine.SetPosition(0, firePoint.position);

        Ray ray = fpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 endPosition;

        if (Physics.Raycast(ray, out hit, laserRange, hitLayers))
        {
            endPosition = hit.point;

            if (hit.collider.CompareTag("Breakable"))
            {
                BreakableObject breakScript = hit.collider.GetComponent<BreakableObject>();

                if (breakScript != null)
                    breakScript.Break(hit.point);
                else
                    Destroy(hit.collider.gameObject);
            }
            else if (hit.collider.CompareTag("Switch"))
            {
                Switch switchScript = hit.collider.GetComponent<Switch>();
                if (switchScript != null)
                {
                    switchScript.Activate();
                }
            }
        }
        else
        {
            endPosition = ray.GetPoint(laserRange);
        }

        laserLine.SetPosition(1, endPosition);

        yield return new WaitForSeconds(laserDuration);

        laserLine.enabled = false;
    }
}