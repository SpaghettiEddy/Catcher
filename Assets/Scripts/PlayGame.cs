using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class PlayGame : MonoBehaviour
{
    private string filePath = "./";
    private string posFileName = "posTracker.txt";


    public void StartGame()
    {
        if(PlayerPrefs.HasKey("LastSceneIndex"))
        {
            int sceneIndex = PlayerPrefs.GetInt("LastSceneIndex");
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(sceneIndex);
        }   

        else{

            SceneManager.LoadScene("Town Map");
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadPosition();
        // Unsubscribe from the event to avoid duplicate calls
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void LoadPosition()
    {
        if(!File.Exists(Path.Combine(filePath, posFileName)))
        {
            Debug.Log("Position Tracker File not found!");
            return;
        }

        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player exists and is tagged 'Player'.");
            return;
        }

        using(StreamReader inputFile = new StreamReader(Path.Combine(filePath, posFileName)))
        {
            string line = inputFile.ReadLine();

            while(!string.IsNullOrEmpty(line))
            {
                player.position = StringToVector3(line);
                line = inputFile.ReadLine();
            }
        }

    }

    private Vector3 StringToVector3(string line)
    {

        line = line.Trim('(', ')'); // Remove parentheses
        string[] values = line.Split(',');

        if (values.Length != 3)
        {
            Debug.LogError($"Invalid position data: {line}");
            return Vector3.zero;
        }

        return new Vector3(
            float.Parse(values[0].Trim()),
            float.Parse(values[1].Trim()),
            float.Parse(values[2].Trim())
        );
       
    }
}

