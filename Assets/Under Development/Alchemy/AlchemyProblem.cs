using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AlchemyProblem : MonoBehaviour {


    //ORDER: SIN, CHANGE, FORCE, SECRETS, BEAUTY

    [SerializeField, Header("SIN, CHANGE, FORCE, SECRETS, BEAUTY")]
    List<int> elementValues = new List<int> { 0, 0, 0, 0, 0 };

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
        SetElementTargets();
    }

    void SetElementTargets()
    {
        unbalancedElements[Element.Sin] = elementValues[0];
        unbalancedElements[Element.Change] = elementValues[1];
        unbalancedElements[Element.Force] = elementValues[2];
        unbalancedElements[Element.Secrets] = elementValues[3];
        unbalancedElements[Element.Beauty] = elementValues[4];

    }

    public bool CheckSuccess() {
        foreach(Element e in unbalancedElements.Keys)
        {
            if(unbalancedElements[e] < 10 && unbalancedElements[e] > -10)
            {
                print("SUCESS");
                return true;
            }
        }
        return false;
    }



}
