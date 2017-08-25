using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemyTestPlace : MonoBehaviour {

    Alchemy alc;

    public AlchemyIngredient a;
    public AlchemyIngredient b;

    public AlchemyProblem goal;

	// Use this for initialization
	void Start () {
        alc = GetComponent<Alchemy>();

        CreateProblem();

    }


    void CreateProblem()
    {
        List<int> randomNrs = new List<int>() { 0, 0, 0, 0, 0 };

        for (int i = 0; i < 5; i++)
        {
            int r = Random.Range(0, 2);
            if (r == 1)
            {
                randomNrs[i] = 50;
            }
            else
            {
                randomNrs[i] = 0;
            }
            
        }

        int j = 0;
        List<Element> els = new List<Element>(a.ingredientElements.Keys);
        foreach (Element e in els)
        {
            a.ingredientElements[e] = randomNrs[j];
            j++;
        }





        for (int i = 0; i < 5; i++)
        {
            int r = Random.Range(0, 2);
            if (r == 1)
            {
                randomNrs[i] = 50;
            }
            else
            {
                randomNrs[i] = 0;
            }

        }

        els = new List<Element>(b.ingredientElements.Keys);
        j = 0;
        foreach (Element e in els)
        {
            b.ingredientElements[e] = randomNrs[j];
            j++;
        }


        for (int i = 0; i < 5; i++)
        {
            int r = Random.Range(0, 2);
            if (r == 1)
            {
                randomNrs[i] = 50;
            }
            else
            {
                randomNrs[i] = 0;
            }

        }

        els = new List<Element>(goal.unbalancedElements.Keys);
        j = 0;
        foreach (Element e in els)
        {
            goal.unbalancedElements[e] = randomNrs[j];
            j++;
        }

        a.useDefaults = false;
        b.useDefaults = false;
        goal.useDefaults = false;

		print("done "+goal.unbalancedElements[Element.Beauty]+" "+goal.unbalancedElements[Element.Change]+" "+goal.unbalancedElements[Element.Force]);
    }

}