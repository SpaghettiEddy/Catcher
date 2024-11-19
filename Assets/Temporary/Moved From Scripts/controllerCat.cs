using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controllerCat : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision) 
    {
        movement.x = -movement.x;
        movement.y = -movement.y;
        rb.MovePosition(rb.position + movement * moveSpeed); //* Time.fixedDeltaTime);
    }

    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    private Vector2 movement;
    private int cycle = 0;
    private Transform tf;
    private bool facingRight = true;

    void Update()
    {
        cycle += 1;
        if (cycle % 500 == 0) 
        {
            float tmp = movement.x;
            movement.x = -movement.y;
            movement.y = tmp;
        }
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
        // Move the player
        //rb.AddForce(movement);
        rb.MovePosition(rb.position + movement * moveSpeed); //* Time.fixedDeltaTime);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movement.x = .03f;
        movement.y = 0;
        tf = GetComponent<Transform>();
    }
}