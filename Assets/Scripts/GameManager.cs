using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Variable to store the player's spawn position
    public Vector3 playerSpawnPosition;

    void Awake()
    {
        // Implementing the Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Makes the GameManager persist between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find the player in the new scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Set the player's position to the stored spawn position
            player.transform.position = playerSpawnPosition;
        }
    }
}
