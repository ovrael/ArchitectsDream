using System.Collections;
using System.Collections.Generic;

using AssemblyCSharp.Assets.Tools;

using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField]
    Animator playerAnimator;
    [SerializeField]
    Animator weaponAnimator;

    Rigidbody2D rb;
    PlayerMovement playerMovement;

    float deadzone = 0.5f;
    string currentAnimation = "Idle";

    private void Awake()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>();
        playerMovement = transform.parent.GetComponentInChildren<PlayerMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!playerMovement.OnGround)
        {
            // Fall
            if (rb.velocity.y < -deadzone)
            {
                currentAnimation = playerAnimator.PlayAnimation("Fall", currentAnimation);
            }

            // Jump
            if (rb.velocity.y > deadzone)
            {
                currentAnimation = playerAnimator.PlayAnimation("Jump", currentAnimation);
            }

            return;
        }

        // On ground

        // Not moving
        if (Mathf.Abs(playerMovement.XInput) < deadzone)
        {
            currentAnimation = playerAnimator.PlayAnimation("Idle", currentAnimation);
            return;
        }

        if (Mathf.Abs(playerMovement.XInput) > deadzone)
        {
            currentAnimation = playerAnimator.PlayAnimation("Walk", currentAnimation);
            return;
        }
    }
}
