using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Journal : MonoBehaviour {

    public StoryManager sm;
    [SerializeField] List<Story> quests = new List<Story>();
    List<UIQuestButton> shownButtons = new List<UIQuestButton>();
    public GameObject questButtonPrefab;
    public GameObject questWindowPrefab;
    public Transform questGrid;
    public Transform journalCanvas;

    void Start()
    {
        LoadQuests();
    }

    public void LoadQuests()
    {
        ClearButtonList();
        quests.Clear();
        quests.AddRange(sm.stories); //uhm, this currently adds all quests no matter what. should not!

        foreach (Story q in quests)
        {
            SpawnButton(q);
        }
    }


    public void SpawnButton(Story q)
    {
        GameObject g = Instantiate(questButtonPrefab, questGrid);
        g.GetComponent<UIQuestButton>().SetupButton(q, this);
    }

    public void ClearButtonList()
    {
        for (int i = 0; i < shownButtons.Count; i++)
        {
            Destroy(shownButtons[i].gameObject);
        }
        shownButtons.Clear();
    }
	


    public void OpenQuestWindow(Story q)
    {
        GameObject g = Instantiate(questWindowPrefab, journalCanvas);
        g.GetComponent<UIQuest>().Open(q, this);
        
    }

}
