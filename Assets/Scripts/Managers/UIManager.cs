using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {

	public AnimationCurve lerp;
	public AnimationCurve smooth;
	public AnimationCurve overShoot;
	public AnimationCurve underShoot;
	public AnimationCurve quickOverShoot;
	public AnimationCurve quickUnderShoot;
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
			journal.StartCoroutine(Util.MoveToPos (journalDefPos, journalDefPos-outofScreenY, journal.GetComponent<RectTransform> (), underShoot, 2));
			journal.isOpen = false;
        }
        else
        {
			journal.StopAllCoroutines ();
            journal.gameObject.SetActive(true);
			journal.StartCoroutine(Util.MoveToPos (journalDefPos-outofScreenY, journalDefPos, journal.GetComponent<RectTransform> (), overShoot, 1));
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
		StartCoroutine (Util.MoveToPos (-outofScreenX, Vector2.zero, dialogueText, quickOverShoot, 2));
		StartCoroutine (Util.MoveToPos (outofScreenX, Vector2.zero, dialogueOptions, quickOverShoot, 2));
    }

    public void CloseDialogueUI()
    {
		im.ChangeAlchemyMode ();
		StopAllCoroutines ();
		StartCoroutine (Util.MoveToPos (Vector2.zero, -outofScreenX, dialogueText, smooth, 2));
		StartCoroutine (Util.MoveToPos (Vector2.zero, outofScreenX, dialogueOptions, smooth, 2));
		StartCoroutine (Util.WaitToDisable (dialogueObject.gameObject,1f));
    }




    public void UpdateSurvivalUI(float f, float w, float s)
    {
        foodIMG.color = Color.Lerp(Color.green, Color.red, (Util.Map(f, 100, 0, 0, 1)));
        waterIMG.color = Color.Lerp(Color.green, Color.red, (Util.Map(w, 100, 0, 0, 1)));
        sleepIMG.color = Color.Lerp(Color.green, Color.red, (Util.Map(s, 100, 0, 0, 1)));
    }




    public IEnumerator RollText(string s, TextMeshProUGUI text){

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
	
	}


}
