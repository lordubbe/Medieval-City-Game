using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element { Force, Secrets, Beauty, Sin, Change }

public enum IngredientProperties { Distillable, Boilable, Burnable, Pulverizable, Will_Rot, Will_Dry }

public enum IngredientStates { Distilled, Boiled, Burned, Pulverized, Rotten, Dried }


public class Alchemy : MonoBehaviour {

    public Dictionary<Element, int> elements = new Dictionary<Element, int>
        {
            { Element.Beauty, 0 },
            { Element.Sin, 0 },
            { Element.Change, 0 },
            { Element.Force, 0 },
            { Element.Secrets, 0 }
        };

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
