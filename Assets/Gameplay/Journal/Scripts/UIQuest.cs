using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class UIQuest : MonoBehaviour {

    Journal journal;
    public Elements problem;
    ElementBars penta;

    public TextMeshProUGUI titleT;
    public TextMeshProUGUI textT;
    public TextMeshProUGUI reqText;
    public RectTransform pentaSpot;

    public void Open(Story a, Journal j)
    {
        journal = j;
        titleT.text = a.name;
        textT.text = a.BuildDescription();
        if(a.curState.GetType() == typeof(QualityAlchemy))
        {
            QualityAlchemy qa = a.curState.qualityReqs[0] as QualityAlchemy; //currently just dumbly gets the first element.
            problem = qa.GetElements();
            penta = Alchemy.Instance.DrawElementBars(problem, pentaSpot as Transform);
        }
        else
        {
            string s = "";
            foreach(Quality q in a.curState.qualityReqs)
            {
                s += q.description + " " + journal.sm.allQualities.Find(x=>x.id==q.id).GetValue() + "/" + q.GetValue() + "\n";
            }
            reqText.text = s;
        }
        // 
    }

    void OnDisable()
    {
        Destroy(penta);
    }

}
