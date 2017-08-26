using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemHandler {

	public static GameObject currentlyHeldItem;

	public static void Equip(GameObject item){
		currentlyHeldItem = item;
	}

	public static void Drop(GameObject item){
		if (currentlyHeldItem == item) {
			currentlyHeldItem = null;
		}
	}
}
