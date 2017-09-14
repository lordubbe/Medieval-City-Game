using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Canvas inventoryCanvas;
    public Canvas journalCanvas;
    public Journal journal;

    private void Start()
    {
        InteractionManager.OnJDown += OpenJournal;
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
	


}
