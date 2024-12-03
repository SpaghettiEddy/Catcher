using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    // Reference to the catnip icon in the UI
    public GameObject catnipIcon; // Assign in the Inspector

    private void Awake()
    {
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

    public void UpdateCatnipIcon(bool isActive)
    {
        if (catnipIcon != null)
        {
            catnipIcon.SetActive(isActive);
            Debug.Log("Catnip icon set to " + isActive);
        }
        else
        {
            Debug.LogWarning("Catnip icon is not assigned in UIManager.");
        }
    }
}
