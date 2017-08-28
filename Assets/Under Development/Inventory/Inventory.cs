using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//For the inspector
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class Inventory : MonoBehaviour {

	public Item connectedItem;

	[Header("Inventory")]
	public List<InventorySpace> spaces;
	public int availableSpace;

	public List<Item> items;

	public int inventoryWidth; 
	public int inventoryHeight;

	public Sprite inventoryBackground;


	public void AddItem(Item obj, int x, int y){
		Debug.Log ("Adding " + obj.name + " to inventory...");
		for (int _x = x; _x < x+obj.width; _x++) {
			for (int _y = y; _y < y+obj.height; _y++) {
				int idx = Util.coordsToIndex (this, _x, _y);
				spaces [idx].isAvailable = false;
				availableSpace--;
			}
		}
		int i = Util.coordsToIndex(this, x,y);
		spaces [i].SetItem (obj);
		items.Add (obj);
	}
}