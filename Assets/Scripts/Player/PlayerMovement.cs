using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Movement
    [SerializeField, Range(1f, 20f)]
    float maxSpeed = 5f;
    [SerializeField]
    float jumpSpeed = 8f;
    [SerializeField]
    float acceleration = 2f;
    [SerializeField, Range(0f, 4f)]
    float changeDirectionAccelerationMultiplier = 2.75f;

    [SerializeField, Range(0f, 1f)]
    float velocityDecay = 0.9f;

    // Jump
    bool jump = false;
    bool jumpHeld = false;
    [Range(0, 5f)][SerializeField] private float fallLongMult = 0.85f;
    [Range(0, 5f)][SerializeField] private float fallShortMult = 1.55f;
    bool onGround = true;
    public bool OnGround { get { return onGround; } }

    // Input
    float inputDeadzone = 0.1f;
    float xInput = 0f;
    public float XInput { get { return xInput; } }

    // Unity components
    Transform mainTransform;
    Rigidbody2D body;
    BoxCollider2D groundCheckCollider;


    private void Awake()
    {
        mainTransform = transform.parent;
        body = mainTransform.GetComponent<Rigidbody2D>();
        groundCheckCollider = mainTransform.Find("GroundCheck").GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
        UpdateJumpInput();
    }

    private void FixedUpdate()
    {
        UpdateGroundedStatus();
        HandleJump();
        MovePlayer();
        ApplyFriction();
    }

    private void UpdateInput()
    {
        xInput = Input.GetAxis("Horizontal");
    }

    // Left/Right movement
    private void MovePlayer()
    {
        if (Mathf.Abs(xInput) <= inputDeadzone)
            return;

        // Accelerate player from current velocity to max speed
        float velocityIncrementation = xInput * acceleration;

        // Changed direction, increase acceleration
        if (Mathf.Sign(xInput) != Mathf.Sign(body.velocity.x))
            velocityIncrementation *= changeDirectionAccelerationMultiplier;
        //body.velocity = new Vector2(0, body.velocity.y); // Clear x velocity <- old, too fast change

        float newSpeed = Mathf.Clamp(body.velocity.x + velocityIncrementation, -maxSpeed, maxSpeed);
        body.velocity = new Vector2(newSpeed, body.velocity.y);

        // Rotate sprite to current direction
        UpdateSpriteDirection();
    }

    private void UpdateJumpInput()
    {
        if (onGround && Input.GetButtonDown("Jump")) jump = true;
        jumpHeld = (!onGround && Input.GetButton("Jump")) ? true : false;
    }

    private void HandleJump()
    {
        // Jumping...
        if (jump)
        {
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
            jump = false;
        }

        // Jumping High...
        if (jumpHeld && body.velocity.y > 0)
        {
            body.velocity += (fallLongMult - 1) * Physics2D.gravity.y * Time.fixedDeltaTime * Vector2.up;
        }
        // Jumping Low...
        else if (!jumpHeld && body.velocity.y > 0)
        {
            body.velocity += (fallShortMult - 1) * Physics2D.gravity.y * Time.fixedDeltaTime * Vector2.up;
        }
    }

    private void UpdateSpriteDirection()
    {
        float direction = Mathf.Sign(xInput);
        mainTransform.localScale = new Vector3(-direction, 1, 1);
    }

    private void ApplyFriction()
    {
        if (onGround && xInput == 0 && body.velocity.y <= 0)
            body.velocity *= velocityDecay;
    }


    private void UpdateGroundedStatus()
    {
        onGround = Physics2D.OverlapAreaAll(groundCheckCollider.bounds.min, groundCheckCollider.bounds.max, LayerMask.GetMask("Ground")).Length > 0;
    }
}
