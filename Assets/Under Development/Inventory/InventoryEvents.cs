using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void InventoryEvent(Inventory inventory);
public delegate void InventoryItemEvent(GameObject item);
public class InventoryEvents : MonoBehaviour {

	public static InventoryEvent OnInventoryEnter;
	public static InventoryEvent OnInventoryExit;
	public static InventoryItemEvent OnDropItem;
	



}
