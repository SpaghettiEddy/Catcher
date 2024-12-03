using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catnip : MonoBehaviour
{

    public GameObject catnipIcon;


    public bool isCollectableByPlayer = true; // Default is true
    // Start is called before the first frame update
    void Start()
    {
        if (!isCollectableByPlayer)
        {
            // Destroy the catnip after 20 seconds if it's not collectable by the player
            Destroy(gameObject, 15f);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            if (isCollectableByPlayer)
            {
                // Deactivate catnip in the scene
                GameManager.instance.SetCatnipIconActive(true);

                Destroy(gameObject);

                // Notify GameManager that catnip has been collected
                other.GetComponent<PlayerController>().CollectCatnip();
            }
            else
            {
                // Do nothing if the catnip is not collectable by the player
            }

        }
    }

}
