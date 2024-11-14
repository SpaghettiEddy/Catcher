using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;
    public Vector3 spawnPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Store the spawn position in the GameManager
            GameManager.instance.nextSpawnPosition = spawnPosition;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
