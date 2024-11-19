using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    private string filePath = "./";
    private string posFileName = "posTracker.txt";

    public void Save(){
        SavePosition();
        PlayerPrefs.SetInt("LastSceneIndex", SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("Start Game");
    }

    private void SavePosition(){
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;

        Debug.Log("Save Position to: " + filePath + posFileName);

        File.Delete(Path.Combine(filePath, posFileName));

        using(StreamWriter outputFile = new StreamWriter(Path.Combine(filePath, posFileName), true))
        {
            outputFile.WriteLine(player.position);
        }
    
    }
}
