using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AlchemyProblem : MonoBehaviour {


    //ORDER: SIN, CHANGE, FORCE, SECRETS, BEAUTY

    [SerializeField, Header("SIN, CHANGE, FORCE, SECRETS, BEAUTY")]
    List<int> elementValues = new List<int> { 0, 0, 0, 0, 0 };

    [SerializeField]
    AlchemyUI alcUI;

    public bool useDefaults = true;

    public Dictionary<Element, int> unbalancedElements = new Dictionary<Element, int>
        {
            { Element.Beauty, 0 },
            { Element.Sin, 0 },
            { Element.Change, 0 },
            { Element.Force, 0 },
            { Element.Secrets, 0 }
        };

    // Use this for initialization
    virtual public void Start () {
        if(useDefaults)
            SetElementTargets();

        alcUI.SetGoal(unbalancedElements);
    }

    void SetElementTargets()
    {
        unbalancedElements[Element.Sin] = elementValues[0];
        unbalancedElements[Element.Change] = elementValues[1];
        unbalancedElements[Element.Force] = elementValues[2];
        unbalancedElements[Element.Secrets] = elementValues[3];
        unbalancedElements[Element.Beauty] = elementValues[4];

    }

    public bool CheckSuccess(AlchemyIngredient a) {
        int successes = 0;
        foreach(Element e in unbalancedElements.Keys)
        {
            if(a.ingredientElements[e] < (unbalancedElements[e] + 10) && a.ingredientElements[e] > (unbalancedElements[e] -10))
            {
                successes++;
            }
        }
        if(successes == 5)
        {
            print("SUCCESS");
            return true;
        }
        else
        {
            print("FAILURE");
            return false;
        }
        
    }



}
