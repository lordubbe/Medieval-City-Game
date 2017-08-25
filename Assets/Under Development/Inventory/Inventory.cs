using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//For the inspector
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class Inventory : Item {
	[Header("Inventory")]
	public List<InventorySpace> spaces;
	public int availableSpace;

	public List<GameObject> itemObjects;

	public int inventoryWidth; 
	public int inventoryHeight;

	public Texture2D inventoryBackgroundImage;
	public Sprite inventoryBackgroundSprite;

	public float tileSize;

	public InventoryDrawer drawer;

	public void AddItem(GameObject obj, int x, int y){
		Item item = obj.GetComponent<ItemBehaviour> ().item;
		Debug.Log ("Adding " + item.name + " to inventory...");
		for (int _x = x; _x < x+item.width; _x++) {
			for (int _y = y; _y < y+item.height; _y++) {
				int idx = Util.coordsToIndex (this, _x, _y);
				spaces [idx].isAvailable = false;
				availableSpace--;
			}
		}
		int i = Util.coordsToIndex(this, x,y);
		spaces [i].SetItem (obj);
		itemObjects.Add (obj);
	}


	public void ResetInventory(){
		itemObjects = new List<GameObject> ();
		for (int i = 0; i < spaces.Count; i++) {
			spaces [i].isAvailable = true;
			spaces [i].item = null;
			spaces [i].itemObj = null;
			availableSpace = inventoryWidth * inventoryHeight;
		}
	}
	
}

[CustomEditor(typeof(Inventory))]
public class InventoryInspector : Editor{

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		Inventory inv = (Inventory)target;

		if (GUILayout.Button ("Reset Inventory")) {
			inv.ResetInventory ();
		}
	}

}