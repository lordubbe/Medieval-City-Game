using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIQuestButton : MonoBehaviour {

    [SerializeField] Button b;
    AlchemyProblem questLink;
    Journal journal;

    public void SetupButton(AlchemyProblem q, Journal j)
    {
        journal = j;
        questLink = q;
        b.onClick.AddListener(() => journal.OpenQuestWindow(questLink));
    }
}
