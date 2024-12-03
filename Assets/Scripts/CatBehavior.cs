using UnityEngine;
using System.Collections;

public class CatBehavior : MonoBehaviour
{
    // Existing variables...
    public string catName;
    public float detectionRadius = 5f;   // How close the player can get before the cat runs
    public float moveSpeed = 3f;         // Speed at which the cat moves away
    public float nearCatnip = 1.5f;
    public LayerMask playerLayer;        // Layer to identify the player
    public LayerMask obstacleLayer;      // Layer to identify obstacles
    public bool isCaught = false;        // Has the cat been caught?
    public bool facingRight = true;

    public GameObject catnip;

    private Transform playerTransform;
    private Rigidbody2D rb;
    private Vector2 currentDirection;

    private Animator animator;

    // Variables for reacting to catnip
    private bool isReactingToCatnip = false;
    private float reactionTime = 0.25f; // Duration of reaction to catnip
    private Vector2 originalVelocity;

    void Start()
    {
        // Find the player in the scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTransform = player.transform;

        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // Prevent rotation
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (isCaught) return; // Stop behavior if caught

        // Handle animations and facing direction
        animator.SetFloat("speed", rb.velocity.magnitude);
        if ((facingRight && rb.velocity.x < -1e-5) || (!facingRight && rb.velocity.x > 1e-5))
        {
            facingRight = !facingRight;
            Vector2 ls = transform.localScale;
            ls.x *= -1;
            transform.localScale = ls;
        }
    }


    void FixedUpdate()
    {
        if (isCaught) return; // Stop behavior if caught

        if (isReactingToCatnip)
        {
            // Do nothing else while reacting to catnip
        }
        else
        {
            if (playerTransform != null)
            {
                float distance = Vector2.Distance(transform.position, playerTransform.position);
                if (distance < detectionRadius)
                {
                    RunAway();
                }
                else
                {
                    IdleMovement(); // Optional idle movement
                }
            }
            else
            {
                IdleMovement(); // If playerTransform is null
            }
        }
    }

    void RunAway()
    {
        // Direction away from the player
        Vector2 desiredDirection = ((Vector2)transform.position - (Vector2)playerTransform.position).normalized;
        Vector2 desiredVelocity = desiredDirection * moveSpeed;

        // Obstacle avoidance
        Vector2 avoidance = Vector2.zero;

        // Parameters
        int numRays = 7; // Number of rays to cast for obstacle detection
        float raySpacing = 15f; // Degrees between each ray
        float rayDistance = 1f; // Distance to check for obstacles ahead

        float baseAngle = Mathf.Atan2(desiredDirection.y, desiredDirection.x) * Mathf.Rad2Deg;

        for (int i = -numRays / 2; i <= numRays / 2; i++)
        {
            float angle = baseAngle + i * raySpacing;
            Vector2 rayDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, rayDistance, obstacleLayer);

            if (hit.collider != null)
            {
                // Calculate a force to avoid the obstacle
                Vector2 obstacleAvoidanceForce = ((Vector2)transform.position - hit.point).normalized / (hit.distance);
                avoidance += obstacleAvoidanceForce;
            }
        }

        // Limit the avoidance force to prevent extreme direction changes
        float maxAvoidanceForce = moveSpeed;
        if (avoidance.magnitude > maxAvoidanceForce)
        {
            avoidance = avoidance.normalized * maxAvoidanceForce;
        }

        // Combine the desired velocity and avoidance
        Vector2 finalVelocity = desiredVelocity + avoidance;

        // Limit the final velocity to the cat's move speed
        if (finalVelocity.magnitude > moveSpeed)
        {
            finalVelocity = finalVelocity.normalized * moveSpeed;
        }

        rb.velocity = finalVelocity;

        // Update facing direction based on movement
        UpdateFacingDirection(rb.velocity);
    }


    bool IsObstacleInDirection(Vector2 direction)
    {
        float rayDistance = 0.5f; // Distance to check for obstacles ahead
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayDistance, obstacleLayer);
        return hit.collider != null;
    }

    Vector2 FindAlternativeDirection(Vector2 originalDirection)
    {
        // Try to rotate the direction slightly to the left and right to find a clear path
        float angleIncrement = 15f;
        for (int i = 1; i <= 12; i++)
        {
            float angle = angleIncrement * i;

            // Rotate direction clockwise
            Vector2 newDirection = RotateVector(originalDirection, angle);
            if (!IsObstacleInDirection(newDirection))
                return newDirection;

            // Rotate direction counter-clockwise
            newDirection = RotateVector(originalDirection, -angle);
            if (!IsObstacleInDirection(newDirection))
                return newDirection;
        }

        // If all directions are blocked, stop moving
        return Vector2.zero;
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if collided with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Cat is caught
            isCaught = true;
            rb.velocity = Vector2.zero; // Stop moving

            GameManager gameManager = FindObjectOfType<GameManager>();

            if (catName == "Shadow")
            {
                gameManager.firstQuestCompleted = true;

                catnip.SetActive(true); // Show the catnip object

                // Destroy the cat GameObject
                Destroy(gameObject);
                Debug.Log("Cat caught!");
            }
            if (catName == "Ansel")
            {
                gameManager.secondQuestCompleted = true;

                // Destroy the cat GameObject
                Destroy(gameObject);
                Debug.Log("Cat caught!");
            }
            if (catName == "Basil")
            {
                gameManager.thirdQuestCompleted = true;

                // Destroy the cat GameObject
                Destroy(gameObject);
                Debug.Log("Cat caught!");
            }

            // Optional: Update quest status or trigger next event
            // QuestManager.Instance.UpdateQuestProgress();
        }
    }

    // Method to react to catnip
    public void ReactToCatnip(Vector3 catnipPosition)
    {
        if (!isReactingToCatnip && !isCaught)
        {
            StartCoroutine(MoveTowardsCatnip(catnipPosition));
        }
    }

    void UpdateFacingDirection(Vector2 velocity)
    {
        if (velocity.x > 1e-5 && !facingRight)
        {
            facingRight = true;
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else if (velocity.x < -1e-5 && facingRight)
        {
            facingRight = false;
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }


    // Coroutine to move towards catnip
    private IEnumerator MoveTowardsCatnip(Vector3 catnipPosition)
    {
        isReactingToCatnip = true;

        // Stop the cat's current movement and save it
        originalVelocity = rb.velocity;
        rb.velocity = Vector2.zero;

        // Calculate direction towards the catnip
        Vector2 directionToCatnip = (catnipPosition - transform.position).normalized;

        // Turn the cat to face the catnip (if applicable)
        if ((facingRight && directionToCatnip.x < -1e-5) || (!facingRight && directionToCatnip.x > 1e-5))
        {
            facingRight = !facingRight;
            Vector2 ls = transform.localScale;
            ls.x *= -1;
            transform.localScale = ls;
        }

        float elapsedTime = 0f;
        while (elapsedTime < reactionTime)
        {
            rb.velocity = directionToCatnip * moveSpeed;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Stop moving towards catnip and resume original movement
        rb.velocity = originalVelocity;

        isReactingToCatnip = false;
    }
}
