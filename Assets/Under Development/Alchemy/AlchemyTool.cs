using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemyTool : MonoBehaviour {

    public List<IngredientProperties> requiredProperties = new List<IngredientProperties>();
    public List<IngredientStates> setStates = new List<IngredientStates>();

    [SerializeField, Header("SIN, CHANGE, FORCE, SECRETS, BEAUTY")]
    List<int> elementAffect = new List<int> { 0,0,0,0,0 };

	[SerializeField]
	List<bool> elementsToChange = new List<bool> { false, false, false, false, false };

    public Dictionary<Element, float> toolElementDestinations = new Dictionary<Element, float>
        {
            { Element.Beauty, 0 },
            { Element.Sin, 0 },
            { Element.Change, 0 },
            { Element.Force, 0 },
            { Element.Secrets, 0 }
        };
		
	public float changeSpeed = 0.2f;
	public bool shouldAffect = false;



    [SerializeField]
    GameObject particles;

    // Use this for initialization
    void Start () {
        SetElements();

    }

    void SetElements()
    {
        toolElementDestinations[Element.Sin] = elementAffect[0];
        toolElementDestinations[Element.Change] = elementAffect[1];
        toolElementDestinations[Element.Force] = elementAffect[2];
        toolElementDestinations[Element.Secrets] = elementAffect[3];
        toolElementDestinations[Element.Beauty] = elementAffect[4];
    }


    public virtual AlchemyIngredient AffectIngredient(AlchemyIngredient a)
    {
        if (!TestProperties(a))
        {
            print("test failed");
            return a;
        }

		shouldAffect = true;
		StartCoroutine(AffectIngredientOverTime(a));

        foreach(IngredientStates s in setStates)
        {
            a.states.Add(s);
        }

        Instantiate(particles, transform.position, Quaternion.identity);


        return a; //DO WHATEVS
    }



	public IEnumerator AffectIngredientOverTime(AlchemyIngredient a){

		//while((a.ingredientElements[Element.Sin] < toolElementDestinations[Element.Sin])

		DummyAlchemyController dum = GameObject.FindGameObjectWithTag("Player").GetComponent<DummyAlchemyController>();
		//DUM. FIX. Later. Needs to be changed. Don't like dependency on a controller. should refer to a manager or something. a general "What Object am I looking at" thing.

		print("starting affecting");

		while(shouldAffect){

			if(elementsToChange[0]){ a.ingredientElements[Element.Sin] = Mathf.Lerp(a.ingredientElements[Element.Sin],toolElementDestinations[Element.Sin],Time.deltaTime*changeSpeed); }
			if(elementsToChange[1]){ a.ingredientElements[Element.Change] = Mathf.Lerp(a.ingredientElements[Element.Change],toolElementDestinations[Element.Change],Time.deltaTime*changeSpeed); }
			if(elementsToChange[2]){ a.ingredientElements[Element.Force] = Mathf.Lerp(a.ingredientElements[Element.Force],toolElementDestinations[Element.Force],Time.deltaTime*changeSpeed); }
			if(elementsToChange[3]){ a.ingredientElements[Element.Secrets] = Mathf.Lerp(a.ingredientElements[Element.Secrets],toolElementDestinations[Element.Secrets],Time.deltaTime*changeSpeed); }
			if(elementsToChange[4]){ a.ingredientElements[Element.Beauty] = Mathf.Lerp(a.ingredientElements[Element.Beauty],toolElementDestinations[Element.Beauty],Time.deltaTime*changeSpeed); }

			//need to make the UI update

			//print("INGREDIENT IS " + a.ingredientElements[Element.Sin]+" "+ a.ingredientElements[Element.Change]+" "+ a.ingredientElements[Element.Force] + " " + a.ingredientElements[Element.Secrets] + " " + a.ingredientElements[Element.Beauty]);

			if(dum.lookingAt != gameObject){
				shouldAffect = false;
			}

			yield return new WaitForEndOfFrame();
		}

		print("Stopped affecting");


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
