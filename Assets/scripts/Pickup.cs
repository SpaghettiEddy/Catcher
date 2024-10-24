using UnityEngine;

public class Pickup : MonoBehaviour
{
   void OnCollisionEnter2D(Collision2D collision)
    {   
        Debug.Log("Booped into " + collision.gameObject.tag);
        // Check if the object that collided with this item has the tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Optionally, you can add any logic here for what happens when the player picks it up (e.g., increase score)
            Debug.Log("Player picked up the item!");

            // Destroy this gameObject (the item)
            Destroy(gameObject);
        }
    }
}