using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AlchemyUI : MonoBehaviour {

    [SerializeField]
	List<Image> ingImages = new List<Image>();

	[SerializeField]
	List<Image> toolImages = new List<Image>();

    [SerializeField]
    Text lookAtText;

    [SerializeField]
    Text propertyText;

    [SerializeField]
    List<Image> goalBars = new List<Image>();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void UpdateUIING(Dictionary<Element, float> elements, GameObject g, List<IngredientProperties> props)
    {
       // print("----------------------- "+elements[Element.Sin]);
        List<float> elVals = new List<float>() { elements[Element.Sin], elements[Element.Change], elements[Element.Force], elements[Element.Secrets], elements[Element.Beauty] };
	//	 print("FORCE: "+elVals[2]);
        for (int i = 0; i < 5; i++)
        {
           
            Vector3 scale = ingImages[i].rectTransform.localScale;
            scale.y = elVals[i] / 100f;
            ingImages[i].rectTransform.localScale = scale;
            //print(i + "  " + (elVals[i]));
        }

        lookAtText.text = "" + g.name;

        propertyText.text = "PROPERTIES ";
        foreach(IngredientProperties p in props)
        {
            propertyText.text += " "+p.ToString();
        }

    }

	public void UpdateUITOOL(Dictionary<Element, float> elements, GameObject g, List<IngredientProperties> props)
	{
		List<float> elVals = new List<float>() { elements[Element.Sin], elements[Element.Change], elements[Element.Force], elements[Element.Secrets], elements[Element.Beauty] };

		for (int i = 0; i < 5; i++)
		{
			// print(elVals[i]);
			Vector3 scale = toolImages[i].rectTransform.localScale;
			scale.y = elVals[i] / 100f;
			toolImages[i].rectTransform.localScale = scale;
			//print(i + "  " + (elVals[i]));
		}

		lookAtText.text = "" + g.name;

		propertyText.text = "PROPERTIES ";
		foreach(IngredientProperties p in props)
		{
			propertyText.text += " "+p.ToString();
		}

	}



    public void SetGoal(Dictionary<Element, float> elements)
    {
        List<float> elVals = new List<float>() { elements[Element.Sin], elements[Element.Change], elements[Element.Force], elements[Element.Secrets], elements[Element.Beauty] };

        for (int i = 0; i < 5; i++)
        {
           // print(elVals[i]);
            Vector3 scale = goalBars[i].rectTransform.localScale;
            scale.y = (float)elVals[i] / 100f;
            goalBars[i].rectTransform.localScale = scale;
        }
    }


}


//DO UI, bars on look. maybe bars on look at tools too :D
//create scenario.