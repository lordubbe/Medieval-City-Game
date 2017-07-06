using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Item {

	public InventorySpace[,] spaces;
	public int availableSpace;

	public Inventory(int width, int height){
		this.width = width;
		this.height = height;
		spaces = new InventorySpace[width, height];
		availableSpace = width * height;
	}
}