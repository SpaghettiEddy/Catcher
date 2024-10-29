using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    [SerializeField] private int speed = 4;
    private Vector2 movement;
    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnMovement(InputValue value) {
        movement = value.Get<Vector2>();
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * movement);
        // rb.velocity = movement * speed;
        // rb.AddForce(movement * speed);
    }
}
