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

    void Update()
    {
        // Get player input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
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

