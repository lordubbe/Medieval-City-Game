using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Journal : MonoBehaviour {

    public StoryManager sm;
    public UIManager uiman;
    [SerializeField] List<Story> quests = new List<Story>();
    List<UIQuestButton> shownButtons = new List<UIQuestButton>();
    public GameObject questButtonPrefab;
    public GameObject questWindowPrefab;
    public Transform questGrid;
    public Transform journalCanvas;
	public bool isOpen = false;

    void Start()
    {
        LoadQuests();
    }

    public void LoadQuests()
    {
        ClearButtonList();


        foreach (Story q in sm.stories)
        {
            if (q.isActive)
            {
                SpawnButton(q);
            }
        }
    }


    public void SpawnButton(Story q)
    {
        if (quests.Contains(q))
        {
            return;
        }

        quests.Add(q);
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
        UIQuest uq = g.GetComponent<UIQuest>();
        uq.Open(q, this);
        uiman.OpenQuest(uq);
    }

}
