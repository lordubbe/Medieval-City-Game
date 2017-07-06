using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryDrawer : MonoBehaviour {

	[Header("GameObject Hookups")]
	public GameObject inventoryCanvas;
	public GameObject gridParent;

	public GameObject inventoryTile;
	public float tileSize;

	public bool isOpen;

	private RectTransform gridParentRect;

	[Header("Inventory Info")]
	public Inventory inventory;

	private bool transitioning;

	private GameObject[,] tiles;

	void OnEnable(){
		gridParentRect = gridParent.GetComponent<RectTransform> ();
	}

	void Update(){
		//simple key triggering
		if (Input.GetKeyDown (KeyCode.I)) {
			ToggleInventory ();
		}
		inventoryCanvas.transform.Rotate (new Vector3 (10, 10, 10));
	}

	void ToggleInventory(){
		if (isOpen) {
			StartCoroutine ("CloseInventory");
		} else {
			StartCoroutine ("OpenInventory");
		}
	}

	void SpawnInventory(){
//		for (int x = 0; x < width; x++) {
//			for (int y = 0; y < height; y++) {
//
//			}
//		}

		if (tiles == null) {
			tiles = new GameObject[inventory.width, inventory.height];
		}

		for (int x = 0; x < inventory.spaces.GetLength (0); x++) {
			for (int y = 0; y < inventory.spaces.GetLength (1); y++) {
				tiles[x,y] = Instantiate (inventoryTile, gridParentRect) as GameObject;
			}
		}
	}

	IEnumerator OpenInventory(){
		yield return null;
		//enable
		SpawnInventory();
		print("opening!");

		isOpen = true;
	}

	IEnumerator CloseInventory(){
		yield return null;
		//disable
		print("closing!");

		isOpen = false;
	}



//	public void AddItemToInventory(

}
