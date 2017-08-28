using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Alchemy : MonoBehaviour {
    
    [SerializeField]
    GameObject testIngredient;


	// Use this for initialization
	void Start () {
        Elements el = new Elements();
        el.sin = 40f;


	}
	

    public Item MixIngredients(Item a, Item b, Vector3 posToSpawn)
    {
        GameObject ing = Instantiate(testIngredient, posToSpawn, Quaternion.identity);

        Item c = ing.GetComponent<Item>();

        //c.properties.AddRange(a.properties);
        //foreach(IngredientProperties p in b.properties)
        //{
        //    if (!c.properties.Exists(x => x == p))
        //    {
        //        c.properties.Add(p);
        //    }
        //}

        //c.states.AddRange(a.states);
        //foreach (IngredientStates p in b.states)
        //{
        //    if (!c.states.Exists(x => x == p))
        //    {
        //        c.states.Add(p);
        //    }
        //}

        //c.ingredientElements[Element.Sin] = a.ingredientElements[Element.Sin] + b.ingredientElements[Element.Sin];
        //c.ingredientElements[Element.Change] = a.ingredientElements[Element.Change] + b.ingredientElements[Element.Change];
        //c.ingredientElements[Element.Force] = a.ingredientElements[Element.Force] + b.ingredientElements[Element.Force];
        //c.ingredientElements[Element.Secrets] = a.ingredientElements[Element.Secrets] + b.ingredientElements[Element.Secrets];
        //c.ingredientElements[Element.Beauty] = a.ingredientElements[Element.Beauty] + b.ingredientElements[Element.Beauty];
        //c.useDefaults = false;

      //  Destroy(a.gameObject);
      //  Destroy(b.gameObject);


     //   print("Created new Ingredient with " + c.ingredientElements[Element.Sin] + " " + c.ingredientElements[Element.Change] + " " + c.ingredientElements[Element.Force] + " " + c.ingredientElements[Element.Secrets] + " " + c.ingredientElements[Element.Beauty]);


        return c;
    }




}
