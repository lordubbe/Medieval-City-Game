using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySpace {
	public Item item;
	public bool isActive;
	public bool isAvailable;

	public InventorySpace(){
		isActive = true;
		isAvailable = true;
	}

	public void SetItem(Item item){
		item = item.GetComponent<Item> () ?? null;
	}

	public void RemoveItem(Item item){
		item = null;
	}
}
