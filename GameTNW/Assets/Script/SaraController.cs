using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SaraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float runSpeedMultiplier = 2f;
    [SerializeField] private AudioClip walkingSound;
    [SerializeField] private AudioSource audioSource;

    private SaraControls saracontrols;

    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;

    private SpriteRenderer mySpriteRender;
    private bool isRunning;
    private bool isMoving;

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

    private void OnDisable()
    {
        saracontrols.Disable();
    }

    private void Update()
    {
        PlayerInput();
        AdjustPlayerFacingDirection();
        HandleWalkingSound();
    }

    private void FixedUpdate()
    {
        MovePlayer();
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
        movement = saracontrols.Movement.Move.ReadValue<Vector2>();

        if (movement == Vector2.zero)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (movement.x != 0) movement.y = 0;
        }

        isMoving = movement != Vector2.zero;

        if (movement != Vector2.zero)
        {
            myAnimator.SetFloat("moveX", movement.x);
            myAnimator.SetFloat("moveY", movement.y);
            myAnimator.SetFloat("Speed", movement.sqrMagnitude);
        }
        else
        {
            myAnimator.SetFloat("Speed", 0);
        }

        myAnimator.SetBool("isMoving", isMoving);
    }

    private void MovePlayer()
    {
        if (movement != Vector2.zero)
        {
            float currentSpeed = isRunning ? moveSpeed * runSpeedMultiplier : moveSpeed;
            Vector2 newPosition = rb.position + movement * (currentSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);
        }
    }

    private void StartRunning()
    {
        isRunning = true;
    }

    private void StopRunning()
    {
        isRunning = false;
    }

    private void HandleWalkingSound()
    {
        if (isMoving && !audioSource.isPlaying)
        {
            audioSource.clip = walkingSound;
            audioSource.loop = true;
            audioSource.Play();
        }
        else if (!isMoving && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
     private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "NextMap")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if(collision.tag == "PreviousMap")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}
   


