using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSpawnerTest : MonoBehaviour {

	public Item itemToSpawn;
	bool itemSpawned = false;
	public KeyCode spawnKey;
	public Canvas inventoryCanvas;

	private GameObject displayedObject;
	private InventoryDrawer drawer;

	GameObject mouse;

	void OnEnable(){
		mouse = new GameObject ("mouseObj");
		mouse.transform.position = Camera.main.transform.position;
//		mouse.transform.parent = inventoryCanvas.transform;
		mouse.transform.localScale = Vector3.one;
		UnityEditor.Selection.activeGameObject = mouse;


		InventoryEvents.OnInventoryEnter += OnOverInventory;
		InventoryEvents.OnInventoryExit += OnNotOverInventoryAnymore;


		//assign vars
		drawer = GetComponent<InventoryDrawer>();

	}

	void OnOverInventory(Inventory inv){
		if (itemSpawned) {
			Destroy (displayedObject);

			displayedObject = new GameObject ("icon");

			displayedObject.AddComponent<Canvas> ();
			displayedObject.AddComponent<CanvasRenderer> ();

			displayedObject.transform.position = mouse.transform.position;
			displayedObject.transform.parent = mouse.transform;
			displayedObject.transform.localScale = Vector3.one;
			Image img = displayedObject.AddComponent<Image> ();
			img.sprite = itemToSpawn.icon;
			img.GetComponent<RectTransform> ().sizeDelta = new Vector2 (drawer.tileSize * itemToSpawn.width, drawer.tileSize * itemToSpawn.height);

			GameObject border = new GameObject ("icon_border");
			border.transform.position = displayedObject.transform.position;
			border.transform.parent = displayedObject.transform;
			border.transform.localScale = Vector3.one;
			img = border.AddComponent<Image> ();
			img.sprite = itemToSpawn.iconBorder;
			img.GetComponent<RectTransform> ().sizeDelta = new Vector2 (drawer.tileSize * itemToSpawn.width, drawer.tileSize * itemToSpawn.height);
		}
	}

	void OnNotOverInventoryAnymore(Inventory inv){
		if (itemSpawned) {
			Destroy (displayedObject);
			SpawnItem (itemToSpawn);

			itemSpawned = true;
		}
	}

	void Update(){
		if (Input.GetKeyDown (spawnKey)) {
			SpawnItem (itemToSpawn);
		}
		float distance = inventoryCanvas.GetComponent<Canvas> ().planeDistance;
		mouse.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance)); //TODO: Make distance less arbitrary
	}

	void SpawnItem(Item itemToSpawn){
		displayedObject = Instantiate (itemToSpawn.runtimeRepresentation, mouse.transform.position, Quaternion.identity) as GameObject;
		displayedObject.transform.localScale = Vector3.one;
		displayedObject.transform.parent = mouse.transform;
		itemSpawned = true;
	}

}
