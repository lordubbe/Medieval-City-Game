using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBehaviour : MonoBehaviour {

	private enum DisplayMode{ graphics, icon };
	private DisplayMode displayMode;

	private Item item;

	public bool holdingObject = false;

	public bool inInventory;
	public bool overInventory;

	//Behavior variables
	private GameObject displayedObject;
	private GameObject clickPoint;
	private float clickDistance;
	private CharacterJoint activeJoint;

	private GameObject runtimeIcon;
	private Image runtimeIconImage;
	private GameObject runtimeGraphics;
	private Rigidbody runtimeGraphicsRb;

	//Icon
	private RectTransform iconImgRect;
	private RectTransform iconBorderRect;

	public InventoryDrawer drawer;

	private Vector3 originalScale = Vector3.one;

	void Awake(){
		//Prepare the runtime objects for spawning/despawning
		Init (); 

		//Subscribe to interaction events
		MouseInteractionManager.OnMouseDown += OnMouseDown;

		//Subscribe to Item events
		ItemHandler.OnItemPickUp += OnItemPickup;
	}

	void Init(){
		originalScale = transform.localScale;
		//Make sure Item is linked
		if (item == null) {
			item = GetComponent<Item> ();
		}

		// Spawn and reference runtime objects 
		runtimeGraphics = transform.Find ("Graphics").gameObject;
		runtimeIcon = transform.Find ("Icon").gameObject;
		iconImgRect = runtimeIcon.GetComponent<RectTransform> ();
		iconBorderRect = runtimeIcon.transform.Find ("Icon_border").GetComponent<RectTransform> ();

		runtimeGraphicsRb = runtimeGraphics.GetComponent<Rigidbody> ();
		runtimeIconImage = runtimeIcon.GetComponent<Image> ();

		// Init display mode
		if (inInventory) {
			runtimeIcon.SetActive (true);
			runtimeGraphics.SetActive (false);
		} else {
			runtimeGraphics.SetActive (true);
			runtimeIcon.SetActive (false);
		}
	}

	void OnItemPickup(Item item){
		runtimeIconImage.raycastTarget = false;
		ItemHandler.OnItemDrop += OnItemDrop;
	}

	void OnItemDrop(Item item){
		runtimeIconImage.raycastTarget = true;
	}

	public void OnEnterInventoryFrame(InventoryDrawer inv){
		drawer = inv;
		overInventory = true;

		//change from object to icon
		if (holdingObject) {
			SetIconSize (GlobalInventorySettings.INVENTORY_TILE_SIZE);
			SetDisplayMode (DisplayMode.icon);

			transform.parent = inv.inventoryCanvas.transform;
			transform.localScale = Vector3.one;
			transform.localRotation = Quaternion.identity;
			transform.position = MouseInteractionManager.hoverPoint;
		}
	}
		
	public void OnExitInventoryFrame(InventoryDrawer inv){
		drawer = null;
		overInventory = false;

		//change from icon to object
		if (holdingObject) {
			SetDisplayMode (DisplayMode.graphics);
			transform.parent = null;
			runtimeIcon.transform.position = transform.position;
			transform.localScale = originalScale;
		}
	}

	void SetDisplayMode(DisplayMode mode){
		switch (mode) {
		case DisplayMode.graphics:
			runtimeGraphics.SetActive (true);
			runtimeIcon.SetActive (false);
			break;
		case DisplayMode.icon:
			runtimeIcon.SetActive (true);
			runtimeGraphics.SetActive (false);
			break;
		}
	}


	void PickUp(){
		// Subscribe to OnMouseUp event
		MouseInteractionManager.OnMouseUp += OnMouseUp;

		// Pick up / drag
		ItemHandler.PickUp(item);
		holdingObject = true;

		runtimeGraphicsRb.isKinematic = true;
		runtimeGraphicsRb.useGravity = false;
	}

	void Drop(){
		ItemHandler.Drop (item);
		holdingObject = false;

		runtimeGraphicsRb.isKinematic = false;
		runtimeGraphicsRb.useGravity = true;
	}

	void SetIconSize(float tileSize){
		Vector2 size = new Vector2 (tileSize * item.width, tileSize * item.height);
		iconImgRect.sizeDelta = size;
		iconBorderRect.sizeDelta = size;
	}

	public void OnMouseDown(){
		if (MouseInteractionManager.currentHoverObject == runtimeGraphics) { //TODO: Unlink from runtimeGraphics somehow, so it'll work with the icon as well?
			clickDistance = (MouseInteractionManager.hoverPoint - Camera.main.transform.position).magnitude;

			//If item is physical
			if (!inInventory) {
				PickUp ();
			}

		} else if (MouseInteractionManager.currentHoverObject == runtimeIcon) { //TODO: Unlink from runtimeIcon somehow
			if (inInventory) {
				drawer.OnItemPickup (item);
				runtimeIcon.GetComponent<Image> ().raycastTarget = false;
				inInventory = false;
				PickUp ();
			}
		}
	}

	public void OnMouseUp(){
		if (holdingObject && !overInventory) {
			Drop ();
		} else if(holdingObject && overInventory){
			if (drawer.OnItemDrop (item)) {
				runtimeIcon.GetComponent<Image> ().raycastTarget = true;
				Debug.Log ("Item drop successful");
			} else {
				Debug.Log ("Couldn't drop item");
			}
		}
	}

	public void OnMouseEnter(){
		//Outline on?
	}

	public void OnMouseExit(){
		//Outline off?
	}

	//Bad code from here
	void Update(){
		
		if (holdingObject) {
			if (!overInventory) {
				transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, clickDistance));
				runtimeGraphics.transform.position = transform.position;
			} else {
				transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, drawer.gridParent.transform.position.z));
			}
		}
	}

}
