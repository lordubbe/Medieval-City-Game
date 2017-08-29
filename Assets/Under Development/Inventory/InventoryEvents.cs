using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void InventoryEvent(InventoryDrawer inventory);
public delegate void InventoryItemEvent(Item item);
public class InventoryEvents : MonoBehaviour {

	public static InventoryEvent OnInventoryEnter;
	public static InventoryEvent OnInventoryExit;
	public static InventoryItemEvent OnAddToInventory;
	public static InventoryItemEvent OnRemoveFromInventory;

}
