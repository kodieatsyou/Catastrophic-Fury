using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;

    private DefaultControls controls;

    [Header("Sounds")]
    [SerializeField] private AudioClip[] meowSounds;
    [SerializeField] private AudioClip footStepSound;
    [SerializeField] private AudioClip landSound;
    [SerializeField] private AudioClip jumpSound;

    [Header("Sound Settings")]
    [SerializeField] private float landNoiseRadius;
    [SerializeField] private float walkingNoiseRadius;
    [SerializeField] private float meowNoiseRadius;

    [Header("Push Settings")]
    [SerializeField] private Transform pushDetectOrigin;
    [SerializeField] private float pushDetectDistance;
    [SerializeField] private float pushDetectRadius;
    [SerializeField] private float hitCooldown;
    public float hitPower;

    [Header("Movement Settings")]
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float speed;
    [SerializeField] private float sneakSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxJumpHeight;
    [SerializeField] private float maxJumpHoldTime;
    [SerializeField] private float playerGravity;
    [SerializeField] private float groundFriction;
    [SerializeField] private float sneakingColliderYSize;
    private float normalColliderYSize;
    private float sneakYPos;
    private float normalYPos;

    [SerializeField] private CapsuleCollider topCollider1;
    [SerializeField] private CapsuleCollider topCollider2;
    [SerializeField] private CapsuleCollider bodyCollider;

    [SerializeField] LayerMask sneakBlockedLayers;

    [Header("Objects")]
    [SerializeField] private GameObject mesh;

    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera mainCam;
    [SerializeField] private CinemachineVirtualCamera sneakCam;

    //COMPONENTS
    private Rigidbody rb;
    private Animator animator;

    //VECTORS
    private Vector2 movementInput = Vector2.zero;

    //FLOATS
    private float totalWalkingNoise;
    private float currentJumpHoldTime;

    //BOOLS
    [SerializeField] private bool isSneaking;
    [SerializeField] private bool canUnSneak = true;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool canHit;
    [SerializeField] private bool isGrounded;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance);
            instance = this;
        } else
        {
            instance = this;
        }
        controls = new DefaultControls();
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Sneak.performed += _ => isSneaking = true;
        controls.Player.Sneak.canceled += _ => isSneaking = false;

        controls.Player.Jump.started+= OnJumpStarted;
        controls.Player.Jump.canceled += OnJumpCancelled;

        controls.Player.Meow.performed += Meow;
        controls.Player.BreakLeft.performed += PushObjectLeft;
        controls.Player.BreakRight.performed += PushObjectRight;

        CameraSwitcher.RegisterVirtualCamera(mainCam);
        CameraSwitcher.RegisterVirtualCamera(sneakCam);
    }

    private void OnDisable()
    {
        controls.Disable();
        controls.Player.Sneak.performed -= _ => isSneaking = true;
        controls.Player.Sneak.canceled -= _ => isSneaking = false;

        controls.Player.Jump.started -= OnJumpStarted;
        controls.Player.Jump.canceled -= OnJumpCancelled;

        controls.Player.Meow.performed -= Meow;
        controls.Player.BreakLeft.performed -= PushObjectLeft;
        controls.Player.BreakRight.performed -= PushObjectRight;

        CameraSwitcher.UnRegisterVirtualCamera(mainCam);
        CameraSwitcher.UnRegisterVirtualCamera(sneakCam);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = mesh.GetComponent<Animator>();
        canHit = true;

        //CameraSwitcher.SwitchToVirtualCamera(mainCam);
        sneakYPos = mesh.transform.localPosition.y + ((normalColliderYSize - sneakingColliderYSize) / 2.0f);
        normalYPos = mesh.transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        ReadMovement();
        animator.SetBool("isGrounded", isGrounded);
        DoSneak();
    }

    private void DoSneak()
    {
        topCollider1.enabled = false;
        topCollider2.enabled = false;
        RaycastHit hit;
        if (Physics.Raycast(
            new Vector3(transform.position.x, transform.position.y + bodyCollider.center.y, transform.position.z),
            transform.up,
            out hit,
            0.4f * 2,
            sneakBlockedLayers
            ))
        {
            canUnSneak = false;
        }
        else
        {
            canUnSneak = true;
        }

        if (controls.Player.Sneak.inProgress)
        {
            isSneaking = true;
        } else
        {
            if(canUnSneak)
            {
                isSneaking = false;
            } else
            {
                isSneaking = true;
            }
        }


        if(isSneaking)
        {
            animator.SetBool("isSneaking", true);

        } else
        {
            animator.SetBool("isSneaking", false);
            topCollider1.enabled = true;
            topCollider2.enabled = true;
        }

    }

    private void FixedUpdate()
    {
        MovePlayer();
        JumpHold();
    }

    private void ReadMovement()
    {
        movementInput = controls.Player.Move.ReadValue<Vector2>();
        animator.SetFloat("RunSpeed", movementInput.y);
        animator.SetFloat("TurnSpeed", movementInput.x);
        animator.SetFloat("MoveMagnitude", movementInput.magnitude);
    }

    private void MovePlayer()
    {
        if(isGrounded && movementInput.magnitude == 0)
        {
            Vector3 frictionVelocity = -rb.velocity * groundFriction;
            rb.AddForce(frictionVelocity, ForceMode.Acceleration);
        }
        if(!isJumping)
        {
            rb.AddForce(-transform.up * playerGravity, ForceMode.Force);
        }
        //Rotate Player
        if (movementInput.x != 0)
        {
            float yRot = transform.rotation.eulerAngles.y + (movementInput.x * rotateSpeed);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, yRot, transform.rotation.eulerAngles.z);
        }
        Vector3 movementDirection = (transform.forward * movementInput.y + transform.right * 0).normalized;
        
        if (isSneaking)
        {
            rb.velocity = new Vector3(movementDirection.x * sneakSpeed, rb.velocity.y, movementDirection.z * sneakSpeed);

        }
        else
        {
            rb.velocity = new Vector3(movementDirection.x * speed, rb.velocity.y, movementDirection.z * speed);
        }
    }

    private void JumpHold()
    {

        // Check if jump button is held
        if (isJumping && currentJumpHoldTime < maxJumpHoldTime)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Force);
            currentJumpHoldTime += Time.fixedDeltaTime;
        }
        if(isJumping && currentJumpHoldTime >= maxJumpHoldTime)
        {
            isJumping = false;
        }
        // Limit max jump height
        if (isJumping && transform.position.y > maxJumpHeight)
        {
            transform.position = new Vector3(transform.position.x, maxJumpHeight, transform.position.z);
            isJumping = false;
        }
    }

    private void OnJumpCancelled(InputAction.CallbackContext ctx)
    {
        if (isJumping)
        {
            isJumping = false;
        }
        
    }

    private void OnJumpStarted(InputAction.CallbackContext ctx)
    {
        AudioManager.instance.PlayOneShot(jumpSound);
        if (isGrounded)
        {
            currentJumpHoldTime = 0f;
            isJumping = true;
            animator.SetTrigger("Jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    private void Meow(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
        {
            animator.SetTrigger("Meow");
            //Mow
        }
    }

    private void PushObjectLeft(InputAction.CallbackContext ctx)
    {
        if(ctx.performed && canHit)
        {
            canHit = false;
            animator.SetTrigger("PushL");
            StartCoroutine(CheckForObjectToPush((hits) =>
            {
                if (hits != null)
                {
                    foreach (Collider hit in hits)
                    {
                        Debug.Log(hit.GetComponent<Collider>().name);
                        hit.transform.GetComponent<Interactable>().InteractLeft();
                    }
                }
            }));
            StartCoroutine(HitCoolDown());
        }
    }

    private void PushObjectRight(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && canHit)
        {
            canHit = false;
            animator.SetTrigger("PushR");
            StartCoroutine(CheckForObjectToPush((hits) =>
            {
                if(hits != null)
                {
                    foreach(Collider hit in hits)
                    {
                        Debug.Log(hit.GetComponent<Collider>().name);
                        hit.transform.GetComponent<Interactable>().InteractRight();
                    }
                }
            }));
            StartCoroutine(HitCoolDown());
        }
    }

    private IEnumerator CheckForObjectToPush(System.Action<Collider[]> onHits)
    {
        yield return new WaitForSeconds(0.45f);
        int interactMask = 1 << 12;
        Collider[] hitObjects = Physics.OverlapSphere(pushDetectOrigin.position, pushDetectRadius, interactMask);

        if(hitObjects.Length > 0)
        {
            onHits(hitObjects);
            yield break;
        } else
        {
            onHits(null);
            yield break;
        }
    }

    private IEnumerator HitCoolDown()
    {
        // Wait for the cooldown duration
        yield return new WaitForSeconds(hitCooldown);
        canHit = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Walkable" || other.transform.tag == "Floor")
        {
            isGrounded = true;
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Walkable" || other.transform.tag == "Floor")
        {
            isGrounded = false;
        }
    }

    public void MakeFootstepSoundQueue()
    {
        GameObject sq = Instantiate(GameAssets.i.soundQueue);
        sq.transform.position = transform.position;
        sq.layer = 15;
        sq.GetComponent<SoundQueue>().PlaySoundQueue(footStepSound, walkingNoiseRadius);
    }

    public void MakeMeowSoundQueue()
    {
        GameObject sq = Instantiate(GameAssets.i.soundQueue);
        sq.transform.position = transform.position;
        sq.layer = 15;
        int randomIndex = Random.Range(0, meowSounds.Length);
        sq.GetComponent<SoundQueue>().PlaySoundQueue(meowSounds[randomIndex], meowNoiseRadius);
    }

    public void MakeLandSoundQueue()
    {
        GameObject sq = Instantiate(GameAssets.i.soundQueue);
        sq.transform.position = transform.position;
        sq.layer = 15;
        sq.GetComponent<SoundQueue>().PlaySoundQueue(landSound, landNoiseRadius);
    }

    public void SwitchToMainCam()
    {
        CameraSwitcher.SwitchToVirtualCamera(mainCam);
    }

    private void OnDrawGizmos()
    {
        //Physics.SphereCast(transform.position + pushDetectOrigin.localPosition, pushDetectRadius, transform.forward, out hit, pushDetectDistance, interactMask))
        Gizmos.DrawWireSphere(pushDetectOrigin.position, pushDetectRadius);
    }
}