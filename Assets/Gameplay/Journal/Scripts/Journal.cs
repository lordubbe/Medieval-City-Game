using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Journal : MonoBehaviour {

    [SerializeField] List<AlchemyProblem> quests = new List<AlchemyProblem>();
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

        foreach (AlchemyProblem q in quests)
        {
            SpawnButton(q);
        }
    }


    public void SpawnButton(AlchemyProblem q)
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
	


    public void OpenQuestWindow(AlchemyProblem q)
    {
        GameObject g = Instantiate(questWindowPrefab, journalCanvas);
        g.GetComponent<UIQuest>().Open(q, this);
        
    }

}
