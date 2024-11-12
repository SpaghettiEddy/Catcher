using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLAYERcontrol : MonoBehaviour
{
    
    public float moveSpeed = 5f;
    private Rigidbody2D rb;  // Make sure the variable is private, not public

    private Vector2 movement;

    void Start()
    {
        // Automatically assign the Rigidbody2D attached to this GameObject
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Check if the Rigidbody2D was successfully found and assigned
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is not found on the GameObject!");
        } 
        else
        {
            //Debug.LogError("Found!");
        }
    }

    private Animator animator;
    private Transform tf;
    private bool facingRight = true;
    public float speed = 0;

    void Update()
    {
        // Get player input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (movement.magnitude > 1)
            movement /= movement.magnitude;
        speed = movement.magnitude;

        animator.SetFloat("speed", speed);
        Debug.Log("speed is " + speed);
        if(movement.x < -1e-6 && facingRight)
        {
            facingRight = false;
            Vector3 ls = transform.localScale;
            ls.x *= -1;
            transform.localScale = ls;
            //transform.localScale.x *= -1;
        }
        if (movement.x > 1e-6 && !facingRight) 
        {
            facingRight = true;
            Vector3 ls = transform.localScale;
            ls.x *= -1;
            transform.localScale = ls;
        }
    }

    void FixedUpdate()
    {
        // Move the player using Rigidbody2D
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision) 
    {
        movement.x = -movement.x;
        movement.y = -movement.y;
        rb.MovePosition(rb.position + movement * moveSpeed); //* Time.fixedDeltaTime);
    }
}

