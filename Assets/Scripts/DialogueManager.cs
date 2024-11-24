using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public GameObject helloButton;
    public GameObject readyButton;

    private Queue<string> sentences;
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();

        readyButton.SetActive(false);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        
        helloButton.SetActive(false);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StartCoroutine(TypeSentence(sentence));
    
    }
    
    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";

        float typingSpeed = 0.05f;

        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(1f);

        DisplayNextSentence();
    }

    void EndDialogue()
    {
        Debug.Log("End of conversation");

        readyButton.SetActive(true);
    }
}
