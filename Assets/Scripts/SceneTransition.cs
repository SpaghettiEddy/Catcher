using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {

    public string sceneToLoad;

    public Vector3 spawnPositionInNewScene;


    // Update is called once per frame
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerSpawnPosition = spawnPositionInNewScene;

            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
