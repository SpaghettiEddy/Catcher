using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class SavingGame : MonoBehaviour
{
    public Transform playerPosition;

    void SaveGame()
    {
        PlayerSave playerSaveData = new PlayerSave();
        playerSaveData.position = new float[] {playerPosition.position.x, playerPosition.position.y, playerPosition.position.z};

        string json = JsonUtility.ToJson(playerSaveData);
        string path = Application.persistentDataPath + "/playerSaveData.json";
        System.IO.File.WriteAllText(path, json);
        Debug.Log("Game Saved at " + path);
    }
    
    void LoadGame()
    {
        string path = Application.persistentDataPath + "/playerSaveData.json";

        if(File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            PlayerSave loadData = JsonUtility.FromJson<PlayerSave>(json);

            playerPosition.position = new Vector3(loadData.position[0], loadData.position[1], loadData.position[2]);
            Vector3 loadedPostion = new Vector3(loadData.position[0], loadData.position[1], loadData.position[2]);
            playerPosition.position = loadedPostion;

            Debug.Log("Game Loaded");
        }
        else
        {
            Debug.LogWarning("Save file not found at " + path);
        }

    }
}