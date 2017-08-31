using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryDrawer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	[Header("GameObject Hookups")]
	public GameObject inventoryCanvas;
	public GameObject gridParent;
	public GameObject inventoryTile;

	public bool isOpen;

	private GridLayoutGroup gridMaster;
	private RectTransform gridParentTransform;
	private Image invBackground;

	[Header("Inventory Info")]
	public Inventory inventory;

	private bool transitioning;
	private List<RuntimeInventoryTile> tiles = new List<RuntimeInventoryTile>();
	private int currentX = 0, currentY = 0;
	public bool sufficientSpace = false;

	void OnEnable(){ 
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

	// TODO: Make this function return a bool for whether the drop was successful
	public bool OnItemDrop(Item item){
		ItemBehaviour objBeh = ItemHandler.currentItem.GetComponent<ItemBehaviour> ();
		if (sufficientSpace) {
			inventory.AddItem (item, currentX, currentY);

			// Parent the item object to the tile
			Debug.Log (tiles [Util.coordsToIndex (inventory, currentX, currentY)].name);
			item.transform.parent = tiles [Util.coordsToIndex (inventory, currentX, currentY)].transform;

			// TODO: Move to ItemBehaviour
			RectTransform itemRect = item.transform.Find ("Icon").GetComponent<RectTransform> ();
			Rect r = itemRect.rect;
			item.gameObject.transform.localPosition = new Vector3 (r.width / 4, -r.height / 4, 0); // Y is inverted in the UI system :/

			item.transform.parent = this.gridParentTransform;
			objBeh.inInventory = true;
			objBeh.holdingObject = false; // TODO: Make better. It maybe return a bool whether the inventory placement was successful or not?
			ItemHandler.Drop (item);

			return true;
		} else {
			return false; //No space or invalid position
		}
	}

	public void OnItemPickup(Item item){
		ItemBehaviour objBeh = item.GetComponent<ItemBehaviour>();

		inventory.RemoveItem (item);
		item.transform.parent = null;
	}

	void SpawnInventory(){
		if (inventory != null) {
			// First spawn the tiles

			//Adjust grid size
			gridMaster.cellSize = new Vector2 (GlobalInventorySettings.INVENTORY_TILE_SIZE, GlobalInventorySettings.INVENTORY_TILE_SIZE);

			//(Shitty) Adjust size of section
			gridParentTransform.sizeDelta = new Vector2 (inventory.inventoryWidth * GlobalInventorySettings.INVENTORY_TILE_SIZE, inventory.inventoryHeight * GlobalInventorySettings.INVENTORY_TILE_SIZE);

			//Set background image
			invBackground.sprite = inventory.inventoryBackground;
			invBackground.color = Color.white;

			if (tiles.Count == 0) {
				for (int x = 0; x < inventory.spaces.Count; x++) {
					GameObject tile = Instantiate (inventoryTile, gridParentTransform) as GameObject;
					RuntimeInventoryTile script = tile.GetComponent<RuntimeInventoryTile> ();
					tiles.Add (script);
					script.inventorySpace = inventory.spaces [x];
					script.drawer = this;
					script.x = (int)(x % inventory.inventoryWidth);
					script.y = (int)(x / inventory.inventoryWidth);

					tile.name = ("Tile " + script.x + "," + script.y);

					Image img = tile.GetComponent<Image> ();
					img.color = inventory.spaces [x].isActive ? Color.white.WithAlpha (0.5f) : Color.black.WithAlpha (0f); //toggle color

					// Check if current tile has an item
					if (inventory.spaces [x].item != null) {
//					Debug.Log ("Item found at " + Util.indexToCoords (inventory, x) + " (" + inventory.spaces [x].itemObj.name + ")"); 
					}
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
				int idx = Util.coordsToIndex (inventory, x, y);
				InventorySpace currentSpace = inventory.spaces [idx];

				//Make sure we're within bounds
				if (x >= xCoord - halfWidth &&
					x < xCoord + halfWidth &&
					y >= yCoord - halfHeight &&
					y < yCoord + halfHeight) {

					if (currentSpace.isActive) {
						if (currentSpace.isAvailable) {
							//Available (green)
							tiles [idx].image.color = Color.green.WithAlpha (0.5f);
							availableToPlaceCount++;
						} else {
							//Occupied (red)
							tiles [idx].image.color = Color.red.WithAlpha (0.5f);
						}
					}
				} else {
					if (currentSpace.isActive) {
						//Not at item (white)
						tiles [idx].image.color = Color.white.WithAlpha (0.5f);
					}
				}
			}
		}

		currentX = xCoord-halfWidth;
		currentY = yCoord-halfHeight;
		sufficientSpace = availableToPlaceCount == item.width * item.height;
	}

	public void ResetInventoryColoring(){

		for (int x = 0; x < inventory.inventoryWidth; x++) {
			for (int y = 0; y < inventory.inventoryHeight; y++) {
				//Get the coordinate in 1D-array format
				int idx = Util.coordsToIndex (inventory, x, y);
				InventorySpace currentSpace = inventory.spaces [idx];

				if (currentSpace.isActive) {
					tiles [idx].image.color = Color.white.WithAlpha (0.5f);
				}
			}
		}
	}


	IEnumerator OpenInventory(){
		yield return null; //Some kind of animation here?

		//enable
		SpawnInventory();

		isOpen = true;
	}

	IEnumerator CloseInventory(){
		yield return null;

		//disable
		gridParent.SetActive (false);
		isOpen = false;
	}

	#region Pointer Event Callbacks
	//Pointer events
	public void OnPointerEnter(PointerEventData evtData){
		ItemHandler.OnEnterInventoryFrame(this);
	}

	public void OnPointerExit(PointerEventData evtData){
		ItemHandler.OnExitInventoryFrame (this);
		ResetInventoryColoring ();
	}
	#endregion
}
