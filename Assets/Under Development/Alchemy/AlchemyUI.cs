using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AlchemyUI : MonoBehaviour {
    
    public RectTransform itemSpot;
    public RectTransform toolSpot;
    public RectTransform goalSpot;

    public GameObject itemLookingAt = null;

    [SerializeField] PentagonShape penta;

    [SerializeField]
    Text lookAtText;

    [SerializeField]
    Text propertyText;



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    public void HandleItemLook(GameObject g)
    {
        if (g == itemLookingAt)
        {
            return;
        }
        if(g == null)
        {
            ResetPentagon(itemSpot);
            return;
        }
        ChangeLookedAtItem(g);        
    }

    

    public void ChangeLookedAtItem(GameObject g)
    {
        itemLookingAt = g;
        if (IsItem(g))
        {
            Item i = g.GetComponent<Item>();
            SetPentagon(i.GetElements(), itemSpot);
            return;
        }
        else
        {
            ResetPentagon(itemSpot);
        }

        if (IsTool(g))
        {
            AlchemyTool t = g.GetComponent<AlchemyTool>();
            print(t.elements);
            SetPentagon(t.elements, toolSpot);
            return;
        }
        else
        {
            ResetPentagon(toolSpot);
        }

    }

    public void SetPentagon(Elements e, RectTransform rt)
    {
        penta.DrawElementPentagon(e, rt.transform);
    }

    public void ResetPentagon(RectTransform rt)
    {
        penta.DrawElementPentagon(Elements.zero, rt);
    }

    public void ResetAllPentagons()
    {
        penta.DrawElementPentagon(Elements.zero, itemSpot);
        penta.DrawElementPentagon(Elements.zero, toolSpot);
        penta.DrawElementPentagon(Elements.zero, goalSpot);
    }


    public bool IsItem(GameObject g)
    {
        if (g.GetComponent<Item>() != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsTool(GameObject g)
    {
        if (g.GetComponent<AlchemyTool>() != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //   public void UpdateUIING(Dictionary<Element, float> elements, GameObject g, List<IngredientProperties> props)
    //   {
    //      // print("----------------------- "+elements[Element.Sin]);
    //       List<float> elVals = new List<float>() { elements[Element.Sin], elements[Element.Change], elements[Element.Force], elements[Element.Secrets], elements[Element.Beauty] };
    ////	 print("FORCE: "+elVals[2]);
    //       for (int i = 0; i < 5; i++)
    //       {

    //           Vector3 scale = ingImages[i].rectTransform.localScale;
    //           scale.y = elVals[i] / 100f;
    //           ingImages[i].rectTransform.localScale = scale;
    //           //print(i + "  " + (elVals[i]));
    //       }

    //       lookAtText.text = "" + g.name;

    //       propertyText.text = "PROPERTIES ";
    //       foreach(IngredientProperties p in props)
    //       {
    //           propertyText.text += " "+p.ToString();
    //       }

    //   }

    //public void UpdateUITOOL(Dictionary<Element, float> elements, GameObject g, List<IngredientProperties> props)
    //{
    //	List<float> elVals = new List<float>() { elements[Element.Sin], elements[Element.Change], elements[Element.Force], elements[Element.Secrets], elements[Element.Beauty] };

    //	for (int i = 0; i < 5; i++)
    //	{
    //		// print(elVals[i]);
    //		Vector3 scale = toolImages[i].rectTransform.localScale;
    //		scale.y = elVals[i] / 100f;
    //		toolImages[i].rectTransform.localScale = scale;
    //		//print(i + "  " + (elVals[i]));
    //	}

    //	lookAtText.text = "" + g.name;

    //	propertyText.text = "PROPERTIES ";
    //	foreach(IngredientProperties p in props)
    //	{
    //		propertyText.text += " "+p.ToString();
    //	}

    //}



    //   public void SetGoal(Dictionary<Element, float> elements)
    //   {
    //       List<float> elVals = new List<float>() { elements[Element.Sin], elements[Element.Change], elements[Element.Force], elements[Element.Secrets], elements[Element.Beauty] };

    //       for (int i = 0; i < 5; i++)
    //       {
    //          // print(elVals[i]);
    //           Vector3 scale = goalBars[i].rectTransform.localScale;
    //           scale.y = (float)elVals[i] / 100f;
    //           goalBars[i].rectTransform.localScale = scale;
    //       }
    //   }


}


//DO UI, bars on look. maybe bars on look at tools too :D
//create scenario.