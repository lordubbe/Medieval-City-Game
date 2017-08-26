using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AlchemyProblem : MonoBehaviour {

    public Elements elementsRequired;

    [SerializeField]
    AlchemyUI alcUI;

    public bool useDefaults = true;
    
    // Use this for initialization
    virtual public void Start () {
		print("problem start");
		// if(useDefaults)
        //    SetElementTargets();

     //   alcUI.SetGoal(unbalancedElements);

    }
    

    //public bool CheckSuccess(AlchemyIngredient a) {
    //    int successes = 0;
    //    foreach(Element e in unbalancedElements.Keys)
    //    {
    //        if(a.ingredientElements[e] < (unbalancedElements[e] + 10) && a.ingredientElements[e] > (unbalancedElements[e] -10))
    //        {
    //            successes++;
    //        }
    //    }
    //    if(successes == 5)
    //    {
    //        print("SUCCESS");
    //        return true;
    //    }
    //    else
    //    {
    //        print("FAILURE");
    //        return false;
    //    }
        
    //}



}
