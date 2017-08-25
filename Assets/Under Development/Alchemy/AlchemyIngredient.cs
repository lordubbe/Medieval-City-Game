using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AlchemyIngredient : MonoBehaviour {

    public List<IngredientProperties> properties = new List<IngredientProperties>();
    public List<IngredientStates> states = new List<IngredientStates>();

    [SerializeField, Header("SIN, CHANGE, FORCE, SECRETS, BEAUTY")]
    List<float> elementValues = new List<float>() { 0f, 0f, 0f, 0f, 0f };

    public bool useDefaults = true;

    public Dictionary<Element, float> ingredientElements = new Dictionary<Element, float>
        {
            { Element.Beauty, 0 },
            { Element.Sin, 0 },
            { Element.Change, 0 },
            { Element.Force, 0 },
            { Element.Secrets, 0 }
        };

    // Use this for initialization
    void Start () {
        if(useDefaults)
            SetElements();

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    void SetElements()
    {
        ingredientElements[Element.Sin] = elementValues[0];
        ingredientElements[Element.Change] = elementValues[1];
        ingredientElements[Element.Force] = elementValues[2];
        ingredientElements[Element.Secrets] = elementValues[3];
        ingredientElements[Element.Beauty] = elementValues[4];

    }
}
