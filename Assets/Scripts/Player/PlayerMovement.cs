using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Colliders")]
    [SerializeField]
    BoxCollider2D groundCheckCollider;

    [SerializeField]
    BoxCollider2D stepCollider;

    [SerializeField]
    BoxCollider2D stepBlockCollider;

    [Header("Movement")]
    [SerializeField, Range(1f, 20f)]
    float maxSpeed = 5f;
    [SerializeField]
    float acceleration = 2f;
    [SerializeField, Range(0f, 4f)]
    float directionSpeedChange = 2.75f;
    [SerializeField, Range(0f, 1f)]
    float velocityDecay = 0.9f;

    bool canWalkUp = true;

    // Jump
    [Header("Jumping")]
    [SerializeField]
    float jumpSpeed = 8f;
    [SerializeField, Range(0, 5f)]
    private float fallLongMult = 0.85f;
    [SerializeField, Range(0, 5f)]
    private float fallShortMult = 1.55f;

    bool jump = false;
    bool jumpHeld = false;
    bool onGround = true;
    public bool OnGround { get { return onGround; } }

    // Input
    float inputDeadzone = 0.1f;
    float xInput = 0f;
    public float XInput { get { return xInput; } }

    // Unity components
    Transform mainTransform;
    Rigidbody2D body;

    float walkUpCooldown = 0f;
    float walkUpTimer = 0.09f;

    private void Awake()
    {
        mainTransform = transform.parent;
        body = mainTransform.GetComponent<Rigidbody2D>();
        if (groundCheckCollider == null)
            groundCheckCollider = mainTransform.Find("GroundCheck").GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGroundedStatus();
        UpdateWalkUpStatus();
        CheckOutOfMapFall();
        UpdateInput();
    }

    private void FixedUpdate()
    {
        walkUpCooldown += Time.deltaTime;
        HandleJump();
        MovePlayer();
        ApplyFriction();
    }

    #region Ground Movement

    // Left/Right movement
    private void MovePlayer()
    {
        if (Mathf.Abs(xInput) <= inputDeadzone)
            return;

        // Accelerate player from current velocity to max speed
        float velocityIncrementation = xInput * acceleration;

        // Changed direction, increase acceleration
        if (Mathf.Sign(xInput) != Mathf.Sign(body.velocity.x))
            velocityIncrementation *= directionSpeedChange;
        //body.velocity = new Vector2(0, body.velocity.y); // Clear x velocity <- old, too fast change

        float newSpeed = Mathf.Clamp(body.velocity.x + velocityIncrementation, -maxSpeed, maxSpeed);
        body.velocity = new Vector2(newSpeed, body.velocity.y);

        if (canWalkUp && onGround && (Math.Sign(xInput) == Math.Sign(-mainTransform.localScale.x)) && walkUpCooldown > walkUpTimer)
        {
            WalkUp();
            walkUpCooldown = 0;
        }

        // Rotate sprite to current direction
        UpdateSpriteDirection();
    }

    private void ApplyFriction()
    {
        if (onGround && xInput == 0 && body.velocity.y <= 0)
            body.velocity *= velocityDecay;
    }

    private void WalkUp()
    {
        float newX = mainTransform.position.x - Math.Sign(mainTransform.localScale.x) * 0.5f;
        float newY = mainTransform.position.y + 1f;
        mainTransform.position = new Vector3(newX, newY, mainTransform.position.z);

        body.velocityX *= 0.97f;
    }

    #endregion

    #region Jump Movement
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

    #endregion

    #region Updates
    private void UpdateInput()
    {
        xInput = Input.GetAxis("Horizontal");
        if (onGround && Input.GetButtonDown("Jump"))
            jump = true;
        jumpHeld = !onGround && Input.GetButton("Jump");
    }

    private void UpdateSpriteDirection()
    {
        float direction = Mathf.Sign(xInput) * mainTransform.localScale.y;
        mainTransform.localScale = new Vector3(-direction, mainTransform.localScale.y, mainTransform.localScale.z);
    }

    private void UpdateGroundedStatus()
    {
        onGround = Physics2D.OverlapAreaAll(groundCheckCollider.bounds.min, groundCheckCollider.bounds.max, LayerMask.GetMask("Ground")).Length > 0;
    }

    private void UpdateWalkUpStatus()
    {
        bool stepAhead = Physics2D.OverlapAreaAll(stepCollider.bounds.min, stepCollider.bounds.max, LayerMask.GetMask("Ground")).Length > 0;
        bool blockadeAhead = Physics2D.OverlapAreaAll(stepBlockCollider.bounds.min, stepBlockCollider.bounds.max, LayerMask.GetMask("Ground")).Length > 0;

        canWalkUp = stepAhead && !blockadeAhead;
    }

    private void CheckOutOfMapFall()
    {
        if (mainTransform.position.y < -20)
        {
            mainTransform.position = GameObject.FindWithTag("SpawnPoint").transform.position;
        }
    }
    #endregion

    #region Coroutine

    public void ChangeMaxSpeedForTime(float newMaxSpeed, float time)
    {
        time = Mathf.Clamp(time, 1, 60);
        newMaxSpeed = Mathf.Clamp(newMaxSpeed, 0, 100);

        IEnumerator ChangeSpeed()
        {
            float oldMaxSpeed = maxSpeed;
            maxSpeed = newMaxSpeed;

            yield return new WaitForSeconds(time);

            maxSpeed = oldMaxSpeed;
        }

        StartCoroutine(ChangeSpeed());
    }

    #endregion
}
