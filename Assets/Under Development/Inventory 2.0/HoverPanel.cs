using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HoverPanel : MonoBehaviour {
    
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI listofAttributes;
    public TextMeshProUGUI flavourText;
    public RectTransform pentaSpot;
    public RectTransform ownRect;

    public void Start()
    {
        ownRect = GetComponent<RectTransform>();
    }


    public void DisplayHoverText(Item i)
    {
        itemName.text = i.name;
        flavourText.text = i.flavorText;
        listofAttributes.text = "";
        foreach(Attribute a in i.attributes)
        {
            listofAttributes.text += a.GetStateAsString()+"\n";
        }
        Alchemy.Instance.DrawElementPentagon(i.GetElements(), pentaSpot);
    }


}
