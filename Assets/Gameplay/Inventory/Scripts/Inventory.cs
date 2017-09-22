using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Inventory : MonoBehaviour {

    public delegate void ItemAdded(Item i);
    public ItemAdded itemAdded;
    public delegate void ItemRemoved(Item i);
    public ItemRemoved itemRemoved;

    public Item connectedItem;

	[Header("Inventory")]
	public List<InventorySpace> spaces;
	public int availableSpace;

	public List<Item> items;

	public int inventoryWidth; 
	public int inventoryHeight;

	public Sprite inventoryBackground;


	public void AddItem(Item item, int x, int y){
		for (int _x = x; _x < x+item.width; _x++) {
			for (int _y = y; _y < y+item.height; _y++) {
				int idx = Util.coordsToIndex (this, _x, _y);
				spaces [idx].isAvailable = false;
				availableSpace--;
			}
		}
		int i = Util.coordsToIndex(this, x,y);
		spaces [i].SetItem (item);
		items.Add (item);
        if(itemAdded != null)
        {
            itemAdded(item);
        }
	}

	public void RemoveItem(Item item){
		InventorySpace space = spaces.Find (a => a.item == item);
		int i = spaces.IndexOf (space);
		Vector2 coords = Util.indexToCoords(this, i);
		int x = (int)coords.x; 
		int y = (int)coords.y;

		for (int _x = x; _x < x + item.width; _x++) {
			for (int _y = y; _y < y + item.height; _y++) {
				int idx = Util.coordsToIndex (this, _x, _y);
				spaces [idx].isAvailable = true;
				availableSpace++;
			}
		}
		space.RemoveItem (item);
		items.Remove (item);

        if(itemRemoved != null)
        {
            itemRemoved(item);
        }

	}
}