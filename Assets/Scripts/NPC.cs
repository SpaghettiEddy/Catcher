using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPC : MonoBehaviour
{
    public string npcName;
    public GameObject dialoguePannel;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public string[] dialogueBeforeItem;
    public string[] dialogueAfterItem;
    public string[] currentDialogue;
    private int index;

    public float wordSpeed;
    public float sentenceWait;
    public bool playerIsClose;
    public bool itemDelivered;

    void Start()
    {
        currentDialogue = dialogueBeforeItem;
    }

    public void NextLine()
    {
        if (index < currentDialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            zeroText();
        }
    }

    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        nameText.text = "";
        dialoguePannel.SetActive(false);
    }

    IEnumerator Typing()
    {
        nameText.text = npcName;

        foreach (char letter in currentDialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }

        yield return new WaitForSeconds(sentenceWait);

        NextLine();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameState.isJaneFirstDialogueCompleted = true; // Set the flag

            playerIsClose = true;

            if (!dialoguePannel.activeInHierarchy)
            {

                dialoguePannel.SetActive(true);
                StartCoroutine(Typing());
            }
            else
            {
                GameState.isJaneFirstDialogueCompleted = true; // Set the flag
                zeroText();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            zeroText();
        }
    }

    public void DeliverCat()
    {
        if (!itemDelivered)
        {
            itemDelivered = true;
            currentDialogue = dialogueAfterItem; // Switch to post-item dialogue
            zeroText(); // Reset dialogue
        }
    }

}


