using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyAlchemyController : MonoBehaviour {

    public AlchemyIngredient ingredient;
    public AlchemyTool tool;
    public AlchemyProblem prob;

    public GameObject lookingAt;


    [SerializeField]
    AlchemyUI alcUI;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                print(objectHit.name);


                if (objectHit.gameObject.GetComponent<AlchemyIngredient>() != null)
                {
                    ingredient = objectHit.gameObject.GetComponent<AlchemyIngredient>();
                    print("picked up " + ingredient.name);
                }


                    if (objectHit.gameObject.GetComponent<AlchemyTool>() != null)
                {
                    tool = objectHit.gameObject.GetComponent<AlchemyTool>();
                    if (tool != null)
                    {
                        tool.AffectIngredient(ingredient);
                    }
                }

                if (objectHit.gameObject.GetComponent<AlchemyProblem>() != null)
                {
                    prob = objectHit.gameObject.GetComponent<AlchemyProblem>();
                    prob.CheckSuccess(ingredient);
                }



                if(objectHit.gameObject.CompareTag("AlchemyMixSurface"))
                {
                    if(ingredient != null)
                    {
                        ingredient.transform.position = hit.point;
                        objectHit.gameObject.GetComponent<AlchemyMixer>().AddIngredient(ingredient);
                        ingredient = null;
                    }
                    else
                    {
                        objectHit.gameObject.GetComponent<AlchemyMixer>().Mix();
                    }
                }

                // Do something with the object that was hit by the raycast.
            }
        }



        Ray lookRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit lookHit;
        if (Physics.Raycast(lookRay, out lookHit))
        {
            Transform objectHit = lookHit.transform;
			lookingAt = objectHit.gameObject;

            if (objectHit.gameObject.GetComponent<AlchemyIngredient>() != null)
            {

                alcUI.UpdateUIING(objectHit.gameObject.GetComponent<AlchemyIngredient>().ingredientElements, objectHit.gameObject, objectHit.gameObject.GetComponent<AlchemyIngredient>().properties);
                // }
            }
            else if (objectHit.gameObject.GetComponent<AlchemyTool>() != null)
            {
				if(ingredient != null){
					alcUI.UpdateUIING(ingredient.ingredientElements, ingredient.gameObject, ingredient.properties);
				}
				alcUI.UpdateUITOOL(objectHit.gameObject.GetComponent<AlchemyTool>().toolElementDestinations, objectHit.gameObject, objectHit.gameObject.GetComponent<AlchemyTool>().requiredProperties);
            }
            else
            {
                if (ingredient != null)
                {
                    alcUI.UpdateUIING(ingredient.ingredientElements, objectHit.gameObject,ingredient.properties);
                }


            }

        }
            



    }
}
