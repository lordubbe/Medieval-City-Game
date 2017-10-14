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
    public RectTransform rect;

    public TextMeshProUGUI titleT;
    public TextMeshProUGUI textT;
    public TextMeshProUGUI reqText;
    public RectTransform pentaSpot;

    public void Open(Story a, Journal j)
    {
        journal = j;
        titleT.text = a.name;
        textT.text = a.BuildDescription();

		string s = "";
		if(a.curState.GetType() == typeof(StoryStateAlchemy))
        {
			foreach (Quality q in a.curState.qualityReqs.Keys) {
				print ("test " + q.name);

				if (q is QualityAlchemy) {
					QualityAlchemy qa = q as QualityAlchemy;
					problem = qa.GetElements ();
					penta = Alchemy.Instance.DrawElementBars (problem, pentaSpot as Transform);
				} 
				s += q.description + " " + journal.sm.allQualities.Find(x=>x.id==q.id).GetValue() + "/" + a.curState.qualityReqs[q] + "\n";
			}
        }
        else
        {
			foreach(Quality q in a.curState.qualityReqs.Keys)
            {
				s += q.description + " " + journal.sm.allQualities.Find(x=>x.id==q.id).GetValue() + "/" + a.curState.qualityReqs[q] + "\n";
            }
        }
		reqText.text = s;
    }

    public void CloseWindow()
    {
        journal.uiman.CloseQuest(this);
    }


    void OnDisable()
    {
        Destroy(penta);
    }

}
