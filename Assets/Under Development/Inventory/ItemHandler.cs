using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ItemEvent(Item item);
public static class ItemHandler {

	public static ItemEvent OnItemPickUp;
	public static ItemEvent OnItemDrop;

	public static Item currentItem = null;
	public static ItemBehaviour currentItemBehaviour = null;

	public static void PickUp(Item item){
		currentItem = item;
		currentItemBehaviour = item.GetComponent<ItemBehaviour> ();

		// Trigger event
		if (OnItemPickUp != null) {
			OnItemPickUp (item);
		}
	}

	public static void Drop(Item item){
		if (currentItem == item) {
			currentItem = null;
			currentItemBehaviour = null;

			// Trigger event
			if (OnItemDrop != null) {
				OnItemDrop (item);
			}
		}
	}

	public static void OnEnterInventoryFrame(InventoryDrawer drawer){
		if (currentItem != null) {
			currentItemBehaviour.OnEnterInventoryFrame (drawer);
			Debug.Log (currentItem.name + " entered " + drawer.gameObject.name);
		}
	}

	public static void OnExitInventoryFrame(InventoryDrawer drawer){
		if (currentItem != null) {
			currentItemBehaviour.OnExitInventoryFrame (drawer);
			Debug.Log (currentItem.name + " exited " + drawer.gameObject.name);

		}
	}
}
