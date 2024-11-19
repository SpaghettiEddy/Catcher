using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catnip : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Catnip collided with: " + other.name);
        if (other.CompareTag("Player"))
        {
            // Deactivate catnip in the scene
            Destroy(gameObject);

            // Notify GameManager that catnip has been collected
            GameManager.instance.AddCatnipToInventory();
            other.GetComponent<PlayerController>().CollectCatnip();

        }
    }

}
