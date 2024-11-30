using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehavior : MonoBehaviour
{
    public float detectionRadius = 1f;   // How close the player can get before the cat runs
    public float moveSpeed = 3f;         // Speed at which the cat moves away
    public LayerMask playerLayer;        // Layer to identify the player
    public LayerMask obstacleLayer;      // Layer to identify obstacles
    public bool isCaught = false;        // Has the cat been caught?

    private Transform playerTransform;
    private Rigidbody2D rb;
    private Vector2 currentDirection;
    private Vector2 movement;

    private Animator animator;

    private float actionTimer = 0f; // Tracks time for the current action
    private bool isWalking = true; // Determines if NPC is walking or idle
    private float currentActionDuration = 0f; // Duration of the current action


    void Start()
    {
        // Find the player in the scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTransform = player.transform;

        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // Prevent rotation
        animator = GetComponent<Animator>();

        SetNewActionDuration();
    }

    void Update()
    {
        if (isCaught) return; // Stop behavior if caught

        if (playerTransform != null)
        {
            float distance = Vector2.Distance(transform.position, playerTransform.position);
            if (distance < detectionRadius)
            {
                IdleMovement(); // Adjust as necessary for this behavior
            }
            else
            {
                HandleTimedBehavior();
            }
            animator.SetFloat("speed", rb.velocity.magnitude);
        }
    }

    void HandleTimedBehavior()
    {
        actionTimer += Time.deltaTime;

        if (actionTimer >= currentActionDuration)
        {
            // Switch between walking and idling
            isWalking = !isWalking;
            SetNewActionDuration();
            actionTimer = 0f; // Reset the timer
        }

        if (isWalking)
        {
            WalkAround(); // Define walking behavior
        }
        else
        {
            IdleMovement(); // Define idling behavior
        }
    }

    void SetNewActionDuration()
    {
        if (isWalking)
        {
            currentActionDuration = Random.Range(10f, 15f); // Set walking duration
        }
        else
        {
            currentActionDuration = Random.Range(3f, 6f); // Set idle duration
        }
    }

    void WalkAround()
    {
        Vector2 direction = (Vector2)(transform.position).normalized;
        float angle = -15;

        // Check for obstacles ahead
        while (IsObstacleInDirection(direction))
        {
            direction = RotateVector(direction, angle);
        }

        currentDirection = direction;
        rb.velocity = currentDirection * moveSpeed;

        if (currentDirection.x != 0 || currentDirection.y != 0)
        {
            animator.SetFloat("X", currentDirection.x);
            animator.SetFloat("Y", currentDirection.y);

            animator.SetBool("IsWalking", true);
        }
    }

    bool IsObstacleInDirection(Vector2 direction)
    {
        float rayDistance = 1f; // Distance to check for obstacles ahead
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayDistance, obstacleLayer);
        return hit.collider != null;
    }

    Vector2 RotateVector(Vector2 vector, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);
        float tx = vector.x;
        float ty = vector.y;
        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty).normalized;
    }

    void IdleMovement()
    {
        // Optional: Implement idle movement or animations
        rb.velocity = Vector2.zero;
        animator.SetBool("IsWalking", false);
    }
}
