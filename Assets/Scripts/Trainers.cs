using UnityEngine;

public class RunningShoes : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Remove the RunningShoes from the scene
            Destroy(gameObject);
        }
    }
}
