using UnityEngine;

public class CatActivator : MonoBehaviour
{
    public GameObject catGameObject; // Assign via Inspector

    void Start()
    {
        if (GameState.isJaneFirstDialogueCompleted)
        {
            if (!GameManager.instance.firstQuestCompleted)
            {
                catGameObject.SetActive(true);
            }
        }
    }
}
