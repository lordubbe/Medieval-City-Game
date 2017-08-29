using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBehaviour : MonoBehaviour {

	private Item item;

	private bool holdingObject = false;

	public bool inInventory;
	public bool overInventory;

	//Behavior variables
	private GameObject displayedObject;
	private GameObject clickPoint;
	private float clickDistance;
	private CharacterJoint activeJoint;

//	[Header("Assign these if possible (read tooltip)")]
//	[SerializeField, Tooltip("If this object was placed in the game world, assign the graphics object to this field.")]
	private GameObject runtimeIcon;
//	[SerializeField, Tooltip("If this object was placed directly in an inventory, assign the icon object to this field.")]
	private GameObject runtimeGraphics;

	//Icon
	private RectTransform iconImgRect;
	private RectTransform iconBorderRect;

	private InventoryDrawer drawer;

	private Vector3 pickupOffset = Vector3.zero;

	void Awake(){
		//Prepare the runtime objects for spawning/despawning
		Init (); 

		//Subscribe to interaction events
		MouseInteractionManager.OnMouseDown += OnMouseDown;

		//Subscribe to inventory events
		InventoryEvents.OnInventoryEnter += OnEnterInventory;
		InventoryEvents.OnInventoryExit += OnExitInventory;
	}

	void Init(){
		//Make sure Item is linked
		if (item == null) {
			item = GetComponent<Item> ();
		}

		// Spawn and reference runtime objects 
		runtimeGraphics = transform.Find ("Graphics").gameObject;
		runtimeIcon = transform.Find ("Icon").gameObject;
		iconImgRect = runtimeIcon.GetComponent<RectTransform> ();
		iconBorderRect = runtimeIcon.transform.Find ("Icon_border").GetComponent<RectTransform> ();

		// Init display mode
		if (inInventory) {
			runtimeIcon.SetActive (true);
			runtimeGraphics.SetActive (false);
		} else {
			runtimeGraphics.SetActive (true);
			runtimeIcon.SetActive (false);
		}
	}
	
	public void OnEnterInventory(InventoryDrawer inv){
		drawer = inv;
		overInventory = true;

		//change from object to icon
		if (holdingObject) {
			SetIconSize (GlobalInventorySettings.INVENTORY_TILE_SIZE);
			ToggleDisplayState ();

			transform.parent = inv.inventoryCanvas.transform;
			transform.localScale = Vector3.one;
			transform.localRotation = Quaternion.identity;
			transform.position = MouseInteractionManager.hoverPoint;
		}
	}
		
	public void OnExitInventory(InventoryDrawer inv){
		drawer = null;
		overInventory = false;

		//change from icon to object
		if (holdingObject) {
			ToggleDisplayState ();
			transform.parent = null;
			runtimeIcon.transform.position = transform.position;
			transform.localScale = Vector3.one;
		}
	}
		
	void ToggleDisplayState(){
		runtimeIcon.SetActive (!runtimeIcon.activeInHierarchy);
		runtimeGraphics.SetActive (!runtimeGraphics.activeInHierarchy);
	}


	void PickUp(){
		// Subscribe to OnMouseUp event
		MouseInteractionManager.OnMouseUp += OnMouseUp;

		// Pick up / drag
		ItemHandler.Equip(item);
		holdingObject = true;

		Rigidbody rb = item.GetComponentInChildren<Rigidbody> ();
		rb.isKinematic = true;
		rb.useGravity = false;
	}

	void Drop(){
		ItemHandler.Drop (item);
		holdingObject = false;

		Rigidbody rb = item.GetComponentInChildren<Rigidbody> ();
		rb.isKinematic = false;
		rb.useGravity = true;
	}

	void SetIconSize(float tileSize){
		Vector2 size = new Vector2 (tileSize * item.width, tileSize * item.height);
		iconImgRect.sizeDelta = size;
		iconBorderRect.sizeDelta = size;
	}

	public void OnMouseDown(){
		if (MouseInteractionManager.currentHoverObject == runtimeGraphics) { //TODO: Unlink from runtime graphics somehow, so it'll work with the icon as well?
			clickDistance = (MouseInteractionManager.hoverPoint - Camera.main.transform.position).magnitude;

			//If item is physical
			if (!inInventory) {
				PickUp ();
			} else {
				// Pick it up, but in the inventory
			}
		}
	}

	public void OnMouseUp(){
		if (holdingObject && !overInventory) {
			Drop ();
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


//		if (clickPoint != null && holdingObject) {
//			if (drawer != null) { // Item is over inventory
//				clickPoint.transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, drawer.gridParent.transform.position.z));
//				transform.position = clickPoint.transform.position;
//			} else { // Item is not over inventory
//				clickPoint.transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, clickDistance));
//			}
//		}
//
//		if (runtimeIcon.activeInHierarchy && holdingObject) {
//			SetIconSize (50f); //CHANGE
//			runtimeIcon.transform.rotation = Quaternion.identity;
//			Rect fis = runtimeIcon.GetComponent<RectTransform> ().rect;
//			runtimeIcon.transform.position = transform.position - new Vector3 (fis.width / 2, fis.height / 2, 0);
//			Vector2 relPos = new Vector2 (Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
//			runtimeIcon.GetComponent<RectTransform> ().localPosition = new Vector3 ((relPos.x * fis.width) - fis.width / 2, (relPos.y * fis.height) - fis.height / 2, 0);
//		}
	}

}
