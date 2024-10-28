using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapTest : MonoBehaviour
{
    public string sceneToLoad;  // Assign the scene name in the Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player triggered the door
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

}
