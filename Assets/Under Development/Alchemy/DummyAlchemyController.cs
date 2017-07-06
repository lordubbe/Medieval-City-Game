using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyAlchemyController : MonoBehaviour {

    public AlchemyIngredient ingredient;
    public AlchemyTool tool;
    public AlchemyProblem prob;
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
                    prob.CheckSuccess();
                }



                // Do something with the object that was hit by the raycast.
            }
        }
	}
}
