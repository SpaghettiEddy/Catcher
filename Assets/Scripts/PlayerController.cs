using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int speed = 4;

    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;

    public GameObject catnipPrefab;
    private bool hasCatnip = false;
    private bool hasTrainers;

    private float catnipCooldown = 10f; // Cooldown duration in seconds
    private float lastCatnipTime = -10f; // Time when the last catnip was spawned
    private bool hasRunningShoes = false; // Set to true when RunningShoes are collected
    private float speedBoostAmount = 2f;
    private float speedBoostDuration = 2f;
    private float speedBoostCooldown = 5f;
    private float lastSpeedBoostTime = -5f;


    private void Start()
    {
        // Retrieve player state from GameManager
        hasCatnip = GameManager.instance.hasCatnip;
        hasRunningShoes = GameManager.instance.hasTrainers;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnMovement(InputValue value)
    {
        movement = value.Get<Vector2>();

        if (movement.x != 0 || movement.y != 0)
        {
            animator.SetFloat("X", movement.x);
            animator.SetFloat("Y", movement.y);

            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

    private void Update()
    {
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * movement);
        // Inside PlayerController Update() method
        if (hasCatnip && Input.GetKeyDown(KeyCode.C))
        {
            if (Time.time - lastCatnipTime >= catnipCooldown)
            {
                // Spawn the Catnip object next to the player
                GameObject spawnedCatnip = Instantiate(catnipPrefab, transform.position + Vector3.right, Quaternion.identity);

                // Set the spawned catnip to not be collectable by the player
                Catnip catnipScript = spawnedCatnip.GetComponent<Catnip>();
                catnipScript.isCollectableByPlayer = false;

                // Notify all cats in the scene
                CatBehavior[] allCats = FindObjectsOfType<CatBehavior>();
                foreach (CatBehavior cat in allCats)
                {
                    cat.ReactToCatnip(spawnedCatnip.transform.position);
                }

                lastCatnipTime = Time.time; // Reset the cooldown timer
            }
            else
            {
                Debug.Log("Catnip is on cooldown!");
            }
        }


        if (hasRunningShoes && Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (Time.time - lastSpeedBoostTime >= speedBoostCooldown)
            {
                StartCoroutine(SpeedBoost());
                lastSpeedBoostTime = Time.time;
            }
            else
            {
                Debug.Log("Speed boost is on cooldown!");
            }
        }

    }

    public void CollectCatnip()
    {
        hasCatnip = true;
        GameManager.instance.hasCatnip = true; // Update GameManager
        Debug.Log("Catnip collected");

    }

    public void EnableRunningShoes()
    {
        hasRunningShoes = true;
        GameManager.instance.hasTrainers = true; // Update GameManager
        Debug.Log("Running Shoes collected");
    }

    private IEnumerator SpeedBoost()
    {
        speed += (int)speedBoostAmount;
        yield return new WaitForSeconds(speedBoostDuration);
        speed -= (int)speedBoostAmount;
    }
}
