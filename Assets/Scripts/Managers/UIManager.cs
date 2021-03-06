﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum UpdateType { StoryStart, StoryUpdate, ItemGiven }

public class UIManager : MonoBehaviour {

	public AnimationCurve lerp;
	public AnimationCurve smooth;
	public AnimationCurve overShoot;
	public AnimationCurve underShoot;
	public AnimationCurve quickOverShoot;
	public AnimationCurve quickUnderShoot;
    public AnimationCurve ultrasmooth;
	public float textSpeed = 0.1f;

	public InteractionManager im;
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
	public RectTransform dialogueText;
	public RectTransform dialogueOptions;


    //SURVIVAL
    public Image foodIMG;
    public Image waterIMG;
    public Image sleepIMG;

    //UPDATER
    public RectTransform updater;
    public TextMeshProUGUI updaterTitle;
    public TextMeshProUGUI updaterText;


    private void Start()
    {
        InteractionManager.OnJDown += OpenCloseJournal;
		HideDialoguePrompt ();
		dialogueAvailable.gameObject.SetActive (false);
		dialogueObject.gameObject.SetActive (false);
        journal.gameObject.SetActive(false);

		RectTransform screenSize = dialogueCanvas.GetComponent<RectTransform> ();
		outofScreenX = new Vector2 (screenSize.rect.width, 0);
		outofScreenY = new Vector2 (0,screenSize.rect.height);
    }

    public void OpenCloseJournal()
    {
		if (journal.isOpen)
        {
			journal.StopAllCoroutines ();
            
			journal.StartCoroutine(Util.WaitToDisable(journal.gameObject,1));
			journal.StartCoroutine(Util.MoveToPos (journalDefPos, journalDefPos-outofScreenY, journal.GetComponent<RectTransform> (), ultrasmooth, 2));
			journal.isOpen = false;
        }
        else
        {
			journal.StopAllCoroutines ();
            journal.LoadQuests();
            journal.gameObject.SetActive(true);
			journal.StartCoroutine(Util.MoveToPos (journalDefPos-outofScreenY, journalDefPos, journal.GetComponent<RectTransform> (), ultrasmooth, 2.5f));
			journal.isOpen = true;
        }
		im.ChangeAlchemyMode ();
    }

    public void ShowDialoguePrompt()
    {
        dialogueAvailable.gameObject.SetActive(true);
		StartCoroutine (Util.Scale (Vector3.zero, Vector3.one, dialogueAvailable, smooth, 6));
    }

    public void HideDialoguePrompt()
    {
		StartCoroutine (Util.Scale (Vector3.one, Vector3.zero, dialogueAvailable, smooth, 4));
		StartCoroutine(Util.WaitToDisable(dialogueAvailable.gameObject,0.3f));
    }

    public void OpenDialogueUI()
    {
		im.ChangeAlchemyMode ();
        HideDialoguePrompt();
        dialogueObject.gameObject.SetActive(true);
		//StopAllCoroutines ();
		StartCoroutine (Util.MoveToPos (-outofScreenX, Vector2.zero, dialogueText, ultrasmooth, 2));
		StartCoroutine (Util.MoveToPos (outofScreenX, Vector2.zero, dialogueOptions, ultrasmooth, 2));
    }

    public void CloseDialogueUI()
    {
		im.ChangeAlchemyMode ();
		//StopAllCoroutines ();
		StartCoroutine (Util.MoveToPos (Vector2.zero, -outofScreenX, dialogueText, ultrasmooth, 2));
		StartCoroutine (Util.MoveToPos (Vector2.zero, outofScreenX, dialogueOptions, ultrasmooth, 2));
		StartCoroutine (Util.WaitToDisable (dialogueObject.gameObject,1f));
    }

    public void OpenQuest(UIQuest q)
    {
        Vector2 pos = q.rect.anchoredPosition;
        Vector2 posout = new Vector2(pos.x, -outofScreenY.y);
        StartCoroutine(Util.MoveToPos(posout, pos, q.rect, ultrasmooth, 2));

    }

    public void CloseQuest(UIQuest q)
    {
        Vector2 posout = new Vector2(q.rect.anchoredPosition.x, -outofScreenY.y);
        StartCoroutine(Util.MoveToPos(q.rect.anchoredPosition, posout, q.rect, smooth, 2));
        StartCoroutine(Util.WaitToDisable(q.gameObject,1));
    }


    public void DisplayUpdate(UpdateType ut, string details)
    {
        switch (ut)
        {
            case UpdateType.StoryStart:
                updaterTitle.text = "New Journal Entry";
                break;
            case UpdateType.StoryUpdate:
                updaterTitle.text = "Journal Entry Updated";
                break;
            case UpdateType.ItemGiven:
                updaterTitle.text = "Item gained";
                break;
            default:
                updaterTitle.text = "Journal Update";
                break;
        }

        updaterText.text = details;

        StartCoroutine(Util.MoveToPos(new Vector2(600,0), Vector2.zero, updater, ultrasmooth, 2));
        StartCoroutine(WaitToMoveUpdaterBack());
    }

    IEnumerator WaitToMoveUpdaterBack()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(Util.MoveToPos(Vector2.zero, new Vector2(600, 0), updater, ultrasmooth, 2));
    }



    public void UpdateSurvivalUI(float f, float w, float s)
    {
        foodIMG.color = Color.Lerp(Color.green, Color.red, (Util.Map(f, 100, 0, 0, 1)));
        waterIMG.color = Color.Lerp(Color.green, Color.red, (Util.Map(w, 100, 0, 0, 1)));
        sleepIMG.color = Color.Lerp(Color.green, Color.red, (Util.Map(s, 100, 0, 0, 1)));
    }




    public IEnumerator RollText(string s, TextMeshProUGUI text, Node n, System.Action<Node> callback){

		char[] sarray = s.ToCharArray ();
		text.text = "";
		for (int i = 0; i < sarray.Length; i++) {
			text.text += sarray [i];
			if (sarray [i] == '.') {
				yield return new WaitForSeconds (0.2f);
			} else if (sarray [i] == '!') {
				yield return new WaitForSeconds (0.3f);
			}
			else if (sarray [i] == '?') {
				yield return new WaitForSeconds (0.4f);
			}
			else if (sarray [i] == ',') {
				yield return new WaitForSeconds (textSpeed*2);
			}
			else {
				yield return new WaitForSeconds (textSpeed);
			}

		}
        callback(n);
	
	}


}
