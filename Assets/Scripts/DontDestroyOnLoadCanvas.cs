using UnityEngine;

public class DontDestroyOnLoadCanvas : MonoBehaviour
{
    private static DontDestroyOnLoadCanvas instance;

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
        }
    }
}
