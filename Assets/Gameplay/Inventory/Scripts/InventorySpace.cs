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
		this.item = item;
	}

	public void RemoveItem(Item item){
		this.item = null;
	}
}
