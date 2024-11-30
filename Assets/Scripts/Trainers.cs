using UnityEngine;

public class RunningShoes : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Remove the RunningShoes from the scene
            Destroy(gameObject);

            // Notify GameManager that RunningShoes have been collected
            GameManager.instance.AddRunningShoesToInventory();

            // Optional: Update player's abilities
            // other.GetComponent<PlayerController>().EnableRunningShoes();
        }
    }
}
