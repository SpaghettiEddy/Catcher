using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controllerCatAvoider : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision) 
    {
        movement.x = -movement.x;
        movement.y = -movement.y;
        rb.MovePosition(rb.position + movement * moveSpeed); //* Time.fixedDeltaTime);
    }

    private Animator animator;
    public GameObject thingToAvoid;

    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    private Vector2 movement;
    private int cycle = 0;
    private Transform tf;
    private bool facingRight = true;

    void Update()
    {
        cycle += 1;
        // Determine the location of the player
        float player_x = thingToAvoid.GetComponent<Transform>().position.x; // TODO
        float player_y = thingToAvoid.GetComponent<Transform>().position.y; // TODO

        // Determine the direction away from the player
        float dx = rb.position.x - player_x;
        float dy = rb.position.y - player_y;

        // Create movement direction based on player direction
        movement.x = dx;
        movement.y = dy;

        float scale = .03f;
        if (movement.magnitude > 4) scale = 0;

        // Normalize movement
        float len = movement.magnitude;
        if (len > 0.1)
            movement /= len;
        movement *= scale;


        animator.SetFloat("speed", movement.magnitude);
        Debug.Log("Cat position " + movement);

        
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
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        movement.x = .03f;
        movement.y = 0;
        tf = GetComponent<Transform>();
    }
}