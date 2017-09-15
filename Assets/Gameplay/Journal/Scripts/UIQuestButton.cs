using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIQuestButton : MonoBehaviour {

    [SerializeField] Button b;
    AlchemyStoryState questLink;
    Journal journal;
    [SerializeField] TextMeshProUGUI buttontext;

    public void SetupButton(AlchemyStoryState q, Journal j)
    {
        journal = j;
        questLink = q;
        b.onClick.AddListener(() => journal.OpenQuestWindow(questLink));
        buttontext.text = q.name;
    }
}
