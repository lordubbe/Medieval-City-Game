using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item3D : MonoBehaviour {

    public ItemBehaviour IBehaviour;

    public void Start()
    {
        IBehaviour = GetComponentInParent<ItemBehaviour>();
    }

    public void OnMouseEnter()
    {
    }

    public void OnMouseExit()
    {
    }


}
