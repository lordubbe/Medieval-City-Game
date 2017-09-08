using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item2D : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public ItemBehaviour IBehaviour;

    public void Start()
    {
        IBehaviour = GetComponentInParent<ItemBehaviour>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IBehaviour.drawer.ShowHoverPanel(IBehaviour);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IBehaviour.drawer.HideHoverPanel();
    }
}
