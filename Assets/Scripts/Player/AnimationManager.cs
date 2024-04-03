using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    PlayerMovement playerMovement;

    float deadzone = 0.5f;
    string currentAnimation = "Idle";

    private void Awake()
    {
        animator = transform.parent.GetComponentInChildren<Animator>();
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
                PlayAnimation("Fall");
            }

            // Jump
            if (rb.velocity.y > deadzone)
            {
                PlayAnimation("Jump");
            }

            return;
        }

        // On ground

        // Not moving
        if (Mathf.Abs(playerMovement.XInput) < deadzone)
        {
            PlayAnimation("Idle");
            return;
        }

        if (Mathf.Abs(playerMovement.XInput) > deadzone)
        {
            PlayAnimation("Walk");
            return;
        }
    }

    private void PlayAnimation(string animation)
    {

        if (currentAnimation == animation)
            return;

        animator.Play(animation);
        currentAnimation = animation;
    }
}
