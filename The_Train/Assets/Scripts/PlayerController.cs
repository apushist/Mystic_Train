using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb;
    private Vector2 moveDirection;
    public float stepSoundRate = 0.5F;
    public AudioSource stepSound;

    private float nextStep = 0.0F;

     public bool canMove = true; //����� ��������� ������ ���������, ����� �� � ���������

    public static event Action Epressed;
    // Update is called once per frame
    void Update()
    {
        // Processing Inputs
        ProcessInputs();

	}

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        // Physics Calculations
        Move();
	}

    void ProcessInputs()
    {
        if (canMove)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            moveDirection = new Vector2(moveX, moveY).normalized;

            if (Time.time > nextStep && (moveX != 0 || moveY != 0))
            {
                nextStep = Time.time + stepSoundRate;
                stepSound.pitch = UnityEngine.Random.Range(0.4f, 1.0f);
                stepSound.Play();
            }
        }
        else
            moveDirection = new Vector2(0, 0);
		if (Input.GetKeyDown(KeyCode.E))
        {
            Epressed?.Invoke();
        }
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }
}
