using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Player data (you can add more as needed)
    public GameObject playerPrefab;
    [HideInInspector]
    public GameObject playerInstance;

    public bool firstQuestCompleted = false;
    public bool secondQuestCompleted = false;
    public bool thirdQuestCompleted = false;

    public bool isCatnipIconActive = false;
    
    public bool hasCatnip = false;
    public bool hasTrainers = false;


    private void Awake()
    {
        // Ensure only one instance of GameManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }


    private void Start()
    {
        // // Instantiate player if not already in the scene
        // if (GameObject.FindGameObjectWithTag("Player") == null)
        // {
        //     SpawnPlayer(Vector3.zero); // You can set a default spawn position
        // }
    }

    // Method to spawn player at a specific position
    public void SpawnPlayer(Vector3 position)
    {
        playerInstance = Instantiate(playerPrefab, position, Quaternion.identity);
    }

    public Vector3 nextSpawnPosition = Vector3.zero;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Start Game" && scene.name != "Introduction")
        {
            // Move player to the next spawn position
            if (playerInstance == null)
            {
                SpawnPlayer(nextSpawnPosition);
            }
            else
            {
                playerInstance.transform.position = nextSpawnPosition;
            }

            CinemachineVirtualCamera vCam = FindObjectOfType<CinemachineVirtualCamera>();

            if (vCam != null)
            {
                // Assign the player's transform to the camera's Follow and LookAt
                vCam.Follow = playerInstance.transform;
            }
        }
    }

    public void SetCatnipIconActive(bool isActive)
    {
        isCatnipIconActive = isActive;

        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateCatnipIcon(isActive);
        }
        else
        {
            Debug.LogWarning("UIManager.instance is null in GameManager.");
        }
    }



    public void LoadIntroductionScene()
    {
        SceneManager.LoadScene("Introduction");
    }

    public void LoadTownMap()
    {
        SceneManager.LoadScene("Town Map");
    }
}