using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class UIQuest : MonoBehaviour {

    Journal journal;
    public Elements problem;
    GameObject penta;

    public TextMeshProUGUI titleT;
    public TextMeshProUGUI textT;
    public RectTransform pentaSpot;

    public void Open(AlchemyProblem a, Journal j)
    {
        journal = j;
        titleT.text = a.name;
        textT.text = a.description;
        problem = a.elementsRequired;
        penta = Alchemy.Instance.DrawElementPentagon(problem, pentaSpot as Transform);
    }

    void OnDisable()
    {
        Destroy(penta);
    }

}
