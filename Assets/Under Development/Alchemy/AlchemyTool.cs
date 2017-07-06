using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemyTool : MonoBehaviour {

    public List<IngredientProperties> requiredProperties = new List<IngredientProperties>();
    public List<IngredientStates> setStates = new List<IngredientStates>();

    [SerializeField, Header("SIN, CHANGE, FORCE, SECRETS, BEAUTY")]
    List<int> elementAffect = new List<int> { 0,0,0,0,0 };

    public Dictionary<Element, int> toolElements = new Dictionary<Element, int>
        {
            { Element.Beauty, 0 },
            { Element.Sin, 0 },
            { Element.Change, 0 },
            { Element.Force, 0 },
            { Element.Secrets, 0 }
        };


    [SerializeField]
    GameObject particles;

    // Use this for initialization
    void Start () {
        SetElements();

    }

    void SetElements()
    {
        toolElements[Element.Sin] = elementAffect[0];
        toolElements[Element.Change] = elementAffect[1];
        toolElements[Element.Force] = elementAffect[2];
        toolElements[Element.Secrets] = elementAffect[3];
        toolElements[Element.Beauty] = elementAffect[4];

    }


    public virtual AlchemyIngredient AffectIngredient(AlchemyIngredient a)
    {
        if (!TestProperties(a))
        {
            print("test failed");
            return a;
        }

        foreach(IngredientStates s in setStates)
        {
            if (a.states.Exists(x => x == s))
            {
                print("this ingredient has already been through this. Why torture it?");
                return a;
            }
        }

        
        a.IngredientElements[Element.Sin] += toolElements[Element.Sin];
        a.IngredientElements[Element.Beauty] += toolElements[Element.Beauty];
        a.IngredientElements[Element.Secrets] += toolElements[Element.Secrets];
        a.IngredientElements[Element.Force] += toolElements[Element.Force];
        a.IngredientElements[Element.Change] += toolElements[Element.Change];

        foreach(IngredientStates s in setStates)
        {
            a.states.Add(s);
        }

        Instantiate(particles, transform.position, Quaternion.identity);

        print("INGREDIENT IS " + a.IngredientElements[Element.Sin]+" "+ a.IngredientElements[Element.Change]+" "+ a.IngredientElements[Element.Force] + " " + a.IngredientElements[Element.Secrets] + " " + a.IngredientElements[Element.Beauty]);

        return a; //DO WHATEVS
    }
	
	
    public bool TestProperties(AlchemyIngredient i)
    {

        foreach(IngredientProperties p in requiredProperties)
        {
            if (!i.properties.Exists(x => x == p) )
            {
                return false;
            }
        }
        print("ALL GOOD");
        return true;

        
    }
}
