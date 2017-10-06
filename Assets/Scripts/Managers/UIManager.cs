using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public AnimationCurve lerp;
	public AnimationCurve smooth;
	public AnimationCurve overShoot;
	public AnimationCurve underShoot;

    public Canvas inventoryCanvas;
    public Canvas journalCanvas;
    public Canvas dialogueCanvas;

	public Vector2 outofScreenX;
	public Vector2 outofScreenY;

    ///JOURNAL
    public Journal journal;
	public Vector2 journalDefPos = new Vector2 (-400, 0);


    ///DIALOGUE
    public RectTransform dialogueObject;
    public RectTransform dialogueAvailable;

    private void Start()
    {
        InteractionManager.OnJDown += OpenJournal;
        CloseDialogueUI();
        HideDialoguePrompt();
        journal.gameObject.SetActive(false);

		RectTransform screenSize = dialogueCanvas.GetComponent<RectTransform> ();
		outofScreenX = new Vector2 (screenSize.rect.width, 0);
		outofScreenY = new Vector2 (0,screenSize.rect.height);

    }

    public void OpenJournal()
    {
		if (journal.isOpen)
        {
			StopAllCoroutines ();
			StartCoroutine(Util.WaitToDisable(journal.gameObject,1));
			StartCoroutine(Util.MoveToPos (journalDefPos, journalDefPos-outofScreenY, journal.GetComponent<RectTransform> (), underShoot, 2));
			journal.isOpen = false;
        }
        else
        {
			StopAllCoroutines ();
            journal.gameObject.SetActive(true);
			StartCoroutine(Util.MoveToPos (journalDefPos-outofScreenY, journalDefPos, journal.GetComponent<RectTransform> (), overShoot, 1));
			journal.isOpen = true;
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
