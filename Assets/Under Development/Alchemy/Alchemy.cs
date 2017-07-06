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

    [SerializeField]
    GameObject baseIngredient;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public AlchemyIngredient MixIngredients(AlchemyIngredient a, AlchemyIngredient b, Vector3 posToSpawn)
    {
        GameObject ing = Instantiate(baseIngredient, posToSpawn, Quaternion.identity);

        AlchemyIngredient c = ing.GetComponent<AlchemyIngredient>();

        c.properties.AddRange(a.properties);
        foreach(IngredientProperties p in b.properties)
        {
            if (!c.properties.Exists(x => x == p))
            {
                c.properties.Add(p);
            }
        }

        c.states.AddRange(a.states);
        foreach (IngredientStates p in b.states)
        {
            if (!c.states.Exists(x => x == p))
            {
                c.states.Add(p);
            }
        }

        c.ingredientElements[Element.Sin] = a.ingredientElements[Element.Sin] + b.ingredientElements[Element.Sin];
        c.ingredientElements[Element.Change] = a.ingredientElements[Element.Change] + b.ingredientElements[Element.Change];
        c.ingredientElements[Element.Force] = a.ingredientElements[Element.Force] + b.ingredientElements[Element.Force];
        c.ingredientElements[Element.Secrets] = a.ingredientElements[Element.Secrets] + b.ingredientElements[Element.Secrets];
        c.ingredientElements[Element.Beauty] = a.ingredientElements[Element.Beauty] + b.ingredientElements[Element.Beauty];


        Destroy(a.gameObject);
        Destroy(b.gameObject);


        print("Created new Ingredient with " + c.ingredientElements[Element.Sin] + " " + c.ingredientElements[Element.Change] + " " + c.ingredientElements[Element.Force] + " " + c.ingredientElements[Element.Secrets] + " " + c.ingredientElements[Element.Beauty]);


        return c;
    }




}
