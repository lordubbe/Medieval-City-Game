using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySpace {
	public GameObject itemObj;
	public Item item;
	public bool isActive;
	public bool isAvailable;

	public InventorySpace(){
		isActive = true;
		isAvailable = true;
	}

	public void SetItem(GameObject obj){
		itemObj = obj;
		item = obj.GetComponent<ItemBehaviour> ().item ?? null;
	}
}
