using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float runSpeedMultiplier = 2f;

    private SaraControls saracontrols;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;
    private bool isRunning;

    private void Awake()
    {
        saracontrols = new SaraControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();


        saracontrols.Movement.Run.performed += ctx => StartRunning();
        saracontrols.Movement.Run.canceled += ctx => StopRunning();
    }

    private void OnEnable()
    {
        saracontrols.Enable();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        //AdjustPlayerFacingDirection();
        Move();
    }

    private void AdjustPlayerFacingDirection()
    {
        if (movement.x < 0)
        {
            mySpriteRender.flipX = false;
        }
        else if (movement.x > 0)
        {
            mySpriteRender.flipX = false;
        }
    }

    private void PlayerInput()
    {
        movement =  saracontrols.Movement.Move.ReadValue<Vector2>();
        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY",movement.y);
        myAnimator.SetFloat("Speed" , movement.sqrMagnitude);
    }

    private void Move()
    {
        float currentSpeed = isRunning ? moveSpeed * runSpeedMultiplier : moveSpeed;
        rb.MovePosition(rb.position + movement * (currentSpeed * Time.fixedDeltaTime));
    }

    private void StartRunning()
    {
        isRunning = true;
    }

    private void StopRunning()
    {
        isRunning = false;
    }


}
