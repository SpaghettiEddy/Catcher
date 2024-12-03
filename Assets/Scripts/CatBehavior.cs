using UnityEngine;

public class CatBehavior : MonoBehaviour
{
    public float detectionRadius = 5f;   // How close the player can get before the cat runs
    public float moveSpeed = 3f;         // Speed at which the cat moves away
    public LayerMask playerLayer;        // Layer to identify the player
    public LayerMask obstacleLayer;      // Layer to identify obstacles
    public bool isCaught = false;        // Has the cat been caught?
    public bool facingRight = true;

    public GameObject catnip;

    private Transform playerTransform;
    private Rigidbody2D rb;
    private Vector2 currentDirection;


    private Animator animator;

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
            animator.SetFloat("speed", rb.velocity.magnitude);
            if ((facingRight && rb.velocity.x < -1e-5) || (!facingRight && rb.velocity.x > 1e-5))
            {
                facingRight = !facingRight;
                Vector2 ls = transform.localScale;
                ls.x *= -1;
                transform.localScale = ls;
            }
        }
    }

    void RunAway()
    {
        // Direction away from the player
        Vector2 direction = (Vector2)(transform.position - playerTransform.position).normalized;

        // Check for obstacles ahead
        if (IsObstacleInDirection(direction))
        {
            // Find an alternate direction
            direction = FindAlternativeDirection(direction);
        }

        currentDirection = direction;
        rb.velocity = currentDirection * moveSpeed;
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
            gameManager.firstQuestCompleted = true;

            catnip.SetActive(true); // Show the catnip object


            // Destroy the cat GameObject
            Destroy(gameObject);
            Debug.Log("Cat caught!");

            // Optional: Update quest status or trigger next event
            // QuestManager.Instance.UpdateQuestProgress();
        }
    }
}
