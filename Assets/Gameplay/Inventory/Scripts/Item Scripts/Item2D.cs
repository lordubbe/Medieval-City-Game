using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item2D : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public ItemBehaviour itemBehaviour;

    public void Start()
    {
        itemBehaviour = GetComponentInParent<ItemBehaviour>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        itemBehaviour.drawer.ShowHoverPanel(itemBehaviour);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemBehaviour.drawer.HideHoverPanel();
    }

    public void OnInteract(){
        itemBehaviour.OnInteract();
    }
}
