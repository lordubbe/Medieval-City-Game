using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {
	public Inventory inventory;
	public InventoryDrawer drawer;

	[Header("UI")]
	public Image image;
	public Text text;

	public void toggleInventory(){
		//TODO: In the future, the inventory slot should dynamically spawn an inventory in a referenced Canvas ...
		drawer.ToggleInventory ();
	}

	void OnEnable(){
		GameObject inventoryInstance = Instantiate (inventory.gameObject, drawer.transform) as GameObject;
		drawer.inventory = inventoryInstance.GetComponent<Inventory> ();
		if (inventory.connectedItem != null) {
			image.sprite = inventory.connectedItem.icon;
			text.text = inventory.name;
		} else {
			this.enabled = false;
		}
	}

}
