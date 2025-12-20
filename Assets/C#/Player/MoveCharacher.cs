using UnityEngine;

public class Move_Chara : MonoBehaviour
{
    Rigidbody RB;
    CapsuleCollider playerCollider;

    [Header("Movement Settings")]
    public float walkSpeed = 8.0f;
    public float runSpeed = 16.0f;
    public float jumpPower = 6.5f;

    [Header("Treadmill Settings")]
    public bool isTreadmillMode = false;
    public float worldSpeed = 10.0f;

    [Header("Key Bindings")]
    public KeyCode moveForwardKey = KeyCode.W;
    public KeyCode moveBackwardKey = KeyCode.S;
    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode moveRightKey = KeyCode.D;

    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Visual Trick Settings")]
    public GameObject slidingDummy;   
    public Camera playerCamera;
    public Transform deathVisualPoint;

    [Header("Camera & FOV Effects")]
    public Vector3 standCamPos;       
    public Vector3 slideCamPos;       
    public float normalFOV = 60f;     
    public float slideFOV = 80f;      
    public float transitionSpeed = 10.0f;

    [Header("Audio Sources")]
    public AudioSource slideAudioSource;
    public float walkStepInterval = 0.5f;
    private float stepTimer = 0f;
    private bool wasGrounded = true;
    
    [Header("Collider Settings")]
    public float slideHeightRatio = 0.5f;
    private float originalHeight;
    private Vector3 originalCenter;

    [Header("Death Settings")]
    public bool isDead = false;

    private bool isGrounded;
    private bool useSprintToggle = false;
    private bool isSprinting = false;
    private bool isRunningInput = false;
    private bool isSlidingAction = false;
    void Start()
    {
        RB = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();

        if (playerCollider != null)
        {
            originalHeight = playerCollider.height; 
            originalCenter = playerCollider.center; 
        }

        gameObject.layer = LayerMask.NameToLayer("Player");

        UpdateSettings();

        isDead = false;

        if (slideAudioSource != null && SoundManager.Instance != null)
        {
            slideAudioSource.clip = SoundManager.Instance.Slide.clip;
            slideAudioSource.volume = SoundManager.Instance.Slide.volume;
            slideAudioSource.loop = true;
            slideAudioSource.Stop();
        }

        if (playerCamera != null)
        {
            if (standCamPos == Vector3.zero) standCamPos = playerCamera.transform.localPosition;
            normalFOV = playerCamera.fieldOfView;
        }

        if (slidingDummy != null) slidingDummy.SetActive(false);
    }

    public void UpdateSettings()
    {
        int toggleValue = PlayerPrefs.GetInt("SprintToggle", 0);

        useSprintToggle = (toggleValue == 1);

        Debug.Log($"[Move_Chara] Sprint Toggle Mode: {useSprintToggle}");

        if (!useSprintToggle) isSprinting = false;
    }

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.3f, LayerMask.GetMask("Ground"));

        if (!wasGrounded && isGrounded)
        {
            SoundManager.Instance.PlaySFX(SoundManager.Instance.Land);
        }

        wasGrounded = isGrounded;

        if (useSprintToggle)
        {
            if (Input.GetKeyDown(sprintKey)) isSprinting = !isSprinting;
            isRunningInput = isSprinting;
        }
        else
        {
            isRunningInput = Input.GetKey(sprintKey);
        }

        isSlidingAction = isRunningInput && isGrounded;

        if (isGrounded && Input.GetKeyDown(jumpKey))
        {
            RB.linearVelocity = new Vector3(RB.linearVelocity.x, 0, RB.linearVelocity.z);
            RB.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            SoundManager.Instance.PlaySFX(SoundManager.Instance.Jump);
        }

        HandleFootsteps();

        HandleVisualTrick();

        HandleColliderSize();
    }

    void HandleFootsteps()
    {
        bool isMovingInput = Input.GetKey(moveForwardKey)
            || Input.GetKey(moveBackwardKey)
            ||Input.GetKey(moveLeftKey) 
            || Input.GetKey(moveRightKey);

        //if (isGrounded && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && !isSlidingAction)
        if (isGrounded && isMovingInput && !isSlidingAction)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0)
            {
                SoundManager.Instance.PlaySFX(SoundManager.Instance.Walk);

                stepTimer = isRunningInput ? (walkStepInterval * 0.6f) : walkStepInterval;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    void HandleVisualTrick()
    {
        if (playerCamera == null) return;

        if (slidingDummy != null)
            slidingDummy.SetActive(isSlidingAction);

        Vector3 targetPos = isSlidingAction ? slideCamPos : standCamPos;
        float targetFOV = isSlidingAction ? slideFOV : normalFOV;

        playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, targetPos, Time.deltaTime * transitionSpeed);
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * transitionSpeed);

        if (slideAudioSource != null)
        {
            if (isSlidingAction)
            {
                if (!slideAudioSource.isPlaying)
                {
                    slideAudioSource.Play();
                }
            }
            else
            {
                if (slideAudioSource.isPlaying)
                {
                    slideAudioSource.Stop();
                }
            }
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        // float x = Input.GetAxisRaw("Horizontal");
        // float z = Input.GetAxisRaw("Vertical");

        float x = 0f;
        float z = 0f;

        if (Input.GetKey(moveForwardKey))
            z += 1f;
        if (Input.GetKey(moveBackwardKey))
            z -= 1f;
        if (Input.GetKey(moveRightKey))
            x += 1f;
        if (Input.GetKey(moveLeftKey))
            x -= 1f;

        Vector3 moveDir = (transform.right * x + transform.forward * z).normalized;
        float currentSpeed = walkSpeed;

        if (useSprintToggle)
        {
            currentSpeed = isSprinting ? runSpeed : walkSpeed;
        }
        else
        {
            currentSpeed = Input.GetKey(sprintKey) ? runSpeed : walkSpeed;
        }

        Vector3 targetVelocity = moveDir * currentSpeed;

        if (isTreadmillMode)
        {
            targetVelocity += Vector3.back * worldSpeed;
        }

        RB.linearVelocity = new Vector3(targetVelocity.x, RB.linearVelocity.y, targetVelocity.z);
    }

    void HandleColliderSize()
    {
        if (playerCollider == null) return;

        if (isSlidingAction)
        {
            playerCollider.height = originalHeight * slideHeightRatio;
            playerCollider.center = originalCenter * slideHeightRatio;
        }
        else
        {
            playerCollider.height = originalHeight;
            playerCollider.center = originalCenter;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag("DeadZone"))
        {
            Debug.Log("Player Dead");
            isDead = true;

            Treadmill treadmill = other.GetComponentInParent<Treadmill>();

            if (treadmill != null)
            {
                other.isTrigger = false;

                other.gameObject.layer = LayerMask.NameToLayer("Default");

                Debug.Log("Wall Dead");
            }
            else
            {
                Debug.Log("Fall Dead");
            }

            GameManager.Instance.OnPlayerDied();
        }
    }

    public bool IsSliding()
    {
        return isSlidingAction;
    }

    public void ResetVisualsOnDeath()
    {
        if (slidingDummy != null) slidingDummy.SetActive(false);

        if (playerCamera != null)
        {
            if (deathVisualPoint != null)
            {
                playerCamera.transform.localPosition = deathVisualPoint.localPosition;
                playerCamera.transform.localRotation = Quaternion.identity;
            }
            else
            {
                playerCamera.transform.localPosition = standCamPos;
            }
            playerCamera.fieldOfView = normalFOV;
        }

        if (slideAudioSource != null && slideAudioSource.isPlaying)
        {
            slideAudioSource.Stop();
        }

        if (playerCollider != null)
        {
            playerCollider.height = originalHeight;
            playerCollider.center = originalCenter;
        }

        isSlidingAction = false;
        isSprinting = false;
    }

    public void Revive()
    {
        isDead = false;
        Debug.Log("Player Revived");
    }
}