using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryDrawer : MonoBehaviour {

	public KeyCode openInventoryKey;

	[Header("GameObject Hookups")]
	public GameObject inventoryCanvas;
	public GameObject gridParent;

	public GameObject inventoryTile;
	public float tileSize;

	public bool isOpen;

	private GridLayoutGroup gridMaster;
	private RectTransform gridParentTransform;
	private Image invBackground;

	[Header("Inventory Info")]
	public Inventory inventory;

	// Private stuff
	private bool transitioning;
	private List<GameObject> tiles = new List<GameObject>();
	private int currentX = 0, currentY = 0;
	public bool isAvailable = false;

	void OnEnable(){ 
		InventoryEvents.OnInventoryExit += OnExitInventory;
		InventoryEvents.OnDropItem += OnItemDrop;

		gridMaster = gridParent.GetComponent<GridLayoutGroup> ();
		gridParentTransform = gridParent.GetComponent<RectTransform>();
		invBackground = gridParent.GetComponent<Image> ();
		gridParent.SetActive (false);
	}

	public void ToggleInventory(){
		if (isOpen) {
			StartCoroutine ("CloseInventory");
		} else {
			StartCoroutine ("OpenInventory");
		}
	}

	void OnItemDrop(GameObject item){
		ItemBehaviour objBeh = ItemHandler.currentlyHeldItem.GetComponent<ItemBehaviour> ();
		if (objBeh.overInventory) {
			inventory.AddItem (item, currentX, currentY);
			item.transform.parent = this.transform;
			objBeh.inInventory = true;
			ItemHandler.Drop (item);
//			Destroy (ItemHandler.currentlyHeldItem);
		}
	}

	void SpawnInventory(){
		if (inventory != null) {
			// Firest spawn the tiles

			//Adjust grid size
			gridMaster.cellSize = new Vector2 (tileSize, tileSize);

			//(Shitty) Adjust size of section
			gridParentTransform.sizeDelta = new Vector2 (inventory.inventoryWidth * tileSize, gridMaster.padding.top + inventory.inventoryHeight * tileSize);

			//set tileSize for inventory object
			inventory.tileSize = this.tileSize;

			//Set background image
			invBackground.sprite = inventory.inventoryBackgroundSprite;
			invBackground.color = Color.white;

			for (int x = 0; x < inventory.spaces.Count; x++) {
				GameObject tile = Instantiate (inventoryTile, gridParentTransform) as GameObject;
				tiles.Add (tile);
				RuntimeInventoryTile script = tile.GetComponent<RuntimeInventoryTile> ();
				script.drawer = this;
				script.x = (int)(x % inventory.inventoryWidth);
				script.y = (int)(x / inventory.inventoryWidth);
//				tile.GetComponent<RuntimeInventoryTile>().y = x / inventory.inventoryWidth
				Image img = tile.GetComponent<Image> ();
				img.color = inventory.spaces [x].isActive ? Color.white.WithAlpha (0.5f) : Color.black.WithAlpha (0f); //toggle color

				// Check if current tile has an item
//				int idx = Util.coordsToIndex (inventory, x, y);

				if (inventory.spaces [x].itemObj != null) {
					Debug.Log ("Item found at " + Util.indexToCoords (inventory, x) + " (" + inventory.spaces [x].itemObj.name + ")"); 
				}

			}
			gridParent.SetActive (true);

		} else {
			Debug.LogError ("No inventory selected!");
		}
	}

	public void CheckOccupiance(Item item, int xCoord, int yCoord){

		if (item == null)
			return;

		// Quit function if the coords are invalid!
		if (xCoord < 0 || xCoord >= inventory.inventoryWidth)
			return;
		if (yCoord < 0 || yCoord >= inventory.inventoryHeight)
			return;

		int availableToPlaceCount = 0;
		int halfWidth = item.width / 2;
		int halfHeight = item.height / 2;

		for (int x = 0; x < inventory.inventoryWidth; x++) {
			for (int y = 0; y < inventory.inventoryHeight; y++) {
				//Get the coordinate in 1D-array format
				int idx = coordsToIndex (x, y);
				InventorySpace currentSpace = inventory.spaces [idx];

				//Make sure we're within bounds
				if (x >= xCoord - halfWidth &&
					x < xCoord + halfWidth &&
					y >= yCoord - halfWidth &&
					y < yCoord + halfWidth +1) {

					if (currentSpace.isActive) {
						if (currentSpace.isAvailable) {
							//Available (green)
							tiles [idx].GetComponent<Image> ().color = Color.green.WithAlpha (0.5f);
							availableToPlaceCount++;
						} else {
							//Occupied (red)
							tiles [idx].GetComponent<Image> ().color = Color.red.WithAlpha (0.5f);
						}
					}
				} else {
					if (currentSpace.isActive) {
						//Not at item (white)
						tiles [idx].GetComponent<Image> ().color = Color.white.WithAlpha (0.5f);
					}
				}
			}
		}

		currentX = xCoord-halfWidth;
		currentY = yCoord-halfHeight;
		isAvailable = availableToPlaceCount == item.width * item.height;
	}

	public void ResetInventoryColoring(){

		for (int x = 0; x < inventory.inventoryWidth; x++) {
			for (int y = 0; y < inventory.inventoryHeight; y++) {
				//Get the coordinate in 1D-array format
				int idx = coordsToIndex (x, y);
				InventorySpace currentSpace = inventory.spaces [idx];

				if (currentSpace.isActive) {
					tiles [idx].GetComponent<Image> ().color = Color.white.WithAlpha (0.5f);
				}
			}
		}
	}

	int coordsToIndex(int x, int y){
		return y * inventory.inventoryWidth + x;
	}

	void OnExitInventory(Inventory inv){
		ResetInventoryColoring ();
	}


	IEnumerator OpenInventory(){
		yield return null;
		//enable
		SpawnInventory();

		isOpen = true;

		inventory.drawer = this;
	}

	IEnumerator CloseInventory(){
		yield return null;
		//disable
		tiles.ForEach (x => Destroy (x));
		tiles = new List<GameObject> ();
		gridParent.SetActive (false);

		isOpen = false;
	}
}
