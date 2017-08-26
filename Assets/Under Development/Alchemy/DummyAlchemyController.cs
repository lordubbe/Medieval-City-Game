using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyAlchemyController : MonoBehaviour {
    
    public AlchemyTool tool;
    public AlchemyProblem prob;

    public GameObject lookingAt;

    public Item item;
    public Item item2;
    public Container cont;


    [SerializeField]
    AlchemyUI alcUI;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }


        HandleLook();

        if (Input.GetKeyDown(KeyCode.Y))
        {
            tool.PlaceItem(item);
            tool.TurnOn();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            tool.TurnOff();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            print(cont.GetElements());
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            cont.AddItem(item);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            cont.AddItem(item2);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            cont.EmptyContainer();
        }
    }


    void HandleLook()
    {
        Ray lookRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit lookHit;
        if (Physics.Raycast(lookRay, out lookHit))
        {
            Transform objectHit = lookHit.transform;
            lookingAt = objectHit.gameObject;

            if (objectHit.gameObject.GetComponent<Item>() != null)
            {
                alcUI.SetPentagon(objectHit.gameObject.GetComponent<Item>().GetElements(), alcUI.itemSpot);
            }

            //if (objectHit.gameObject.GetComponent<AlchemyIngredient>() != null)
            //{

            //    alcUI.UpdateUIING(objectHit.gameObject.GetComponent<AlchemyIngredient>().ingredientElements, objectHit.gameObject, objectHit.gameObject.GetComponent<AlchemyIngredient>().properties);
            //    // }
            //}
            //else if (objectHit.gameObject.GetComponent<AlchemyTool>() != null)
            //{
            //    if (ingredient != null)
            //    {
            //        alcUI.UpdateUIING(ingredient.ingredientElements, ingredient.gameObject, ingredient.properties);
            //    }
            //    alcUI.UpdateUITOOL(objectHit.gameObject.GetComponent<AlchemyTool>().toolElementDestinations, objectHit.gameObject, objectHit.gameObject.GetComponent<AlchemyTool>().requiredProperties);
            //}
            //else
            //{
            //    if (ingredient != null)
            //    {
            //        alcUI.UpdateUIING(ingredient.ingredientElements, objectHit.gameObject, ingredient.properties);
            //    }
            // }

        }
    }


    void HandleMouseClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            print(objectHit.name);


            //if (objectHit.gameObject.GetComponent<AlchemyIngredient>() != null)
            //{
            //    ingredient = objectHit.gameObject.GetComponent<AlchemyIngredient>();
            //    print("picked up " + ingredient.name);
            //}


            //if (objectHit.gameObject.GetComponent<AlchemyTool>() != null)
            //{
            //    tool = objectHit.gameObject.GetComponent<AlchemyTool>();
            //    if (tool != null)
            //    {
            //        tool.AffectIngredient(ingredient);
            //    }
            //}

            //if (objectHit.gameObject.GetComponent<AlchemyProblem>() != null)
            //{
            //    prob = objectHit.gameObject.GetComponent<AlchemyProblem>();
            //    prob.CheckSuccess(ingredient);
            //}



            //if (objectHit.gameObject.CompareTag("AlchemyMixSurface"))
            //{
            //    if (ingredient != null)
            //    {
            //        ingredient.transform.position = hit.point;
            //        objectHit.gameObject.GetComponent<AlchemyMixer>().AddIngredient(ingredient);
            //        ingredient = null;
            //    }
            //    else
            //    {
            //        objectHit.gameObject.GetComponent<AlchemyMixer>().Mix();
            //    }
            //}

            // Do something with the object that was hit by the raycast.
        }
    }




}
