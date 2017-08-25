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
		drawer.ToggleInventory ();
	}

	void OnEnable(){
		image.sprite = inventory.icon;
		text.text = inventory.name;
	}

}
