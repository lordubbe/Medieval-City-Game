using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemHandler {

	public static Item currentlyHeldItem;

	public static void Equip(Item item){
		currentlyHeldItem = item;
//		item.Equip ();
	}

	public static void Drop(Item item){
		if (currentlyHeldItem == item) {
			currentlyHeldItem = null;
//			item.Drop ();
		}
	}
}
