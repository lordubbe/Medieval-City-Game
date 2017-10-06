using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Canvas inventoryCanvas;
    public Canvas journalCanvas;
    public Canvas dialogueCanvas;

    ///JOURNAL
    public Journal journal;


    ///DIALOGUE
    public RectTransform dialogueObject;
    public RectTransform dialogueAvailable;

    private void Start()
    {
        InteractionManager.OnJDown += OpenJournal;
        CloseDialogueUI();
        HideDialoguePrompt();
        journal.gameObject.SetActive(false);
    }


    public void OpenJournal()
    {
        if (journal.gameObject.activeInHierarchy)
        {
            journal.gameObject.SetActive(false);
        }
        else
        {
            journal.gameObject.SetActive(true);
        }
    }

    public void ShowDialoguePrompt()
    {
        dialogueAvailable.gameObject.SetActive(true);
    }

    public void HideDialoguePrompt()
    {
        dialogueAvailable.gameObject.SetActive(false);
    }

    public void OpenDialogueUI()
    {
        HideDialoguePrompt();
        dialogueObject.gameObject.SetActive(true);
    }

    public void CloseDialogueUI()
    {
        dialogueObject.gameObject.SetActive(false);
    }

}
