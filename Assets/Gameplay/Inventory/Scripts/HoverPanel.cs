﻿using System.Collections;
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
    ElementBars pentaObj;

    public void Start()
    {
        ownRect = GetComponent<RectTransform>();
    }

    public void OnDisable()
    {
        Destroy(pentaObj.gameObject);
    }
    
    public void DisplayHoverText(Item i)
    {
        itemName.text = i.name;
        flavourText.text = i.flavorText;
        listofAttributes.text = "";
        foreach(ItemAttribute a in i.attributes)
        {
            listofAttributes.text += a.GetStateAsString()+"\n";
        }
        //Destroy(pentaSpot.GetChild(0));
        pentaObj = Alchemy.Instance.DrawElementBars(i.GetElements(), pentaSpot);
    }


}
