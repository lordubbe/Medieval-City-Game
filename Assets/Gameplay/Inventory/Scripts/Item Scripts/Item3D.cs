using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item3D : MonoBehaviour {

    public ItemBehaviour itemBehaviour;

    public void Start()
    {
        itemBehaviour = GetComponentInParent<ItemBehaviour>();
    }

    public void OnMouseEnter()
    {
    }

    public void OnMouseExit()
    {
    }

    public void OnInteract(){
        Debug.Log("INTERACTING HEHEHEHEHEHHEHE");
        itemBehaviour.OnInteract();
    }

}
