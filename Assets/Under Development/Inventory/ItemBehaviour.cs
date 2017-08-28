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

	void Awake(){
		//Prepare the runtime objects for spawning/despawning
		Init (); 

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
		//change from object to icon
		if (holdingObject) {
			SetIconSize (GlobalInventorySettings.INVENTORY_TILE_SIZE);
			clickDistance = (Camera.main.transform.position - inv.transform.position).magnitude;
			ToggleDisplayState ();

			transform.parent = inv.inventoryCanvas.transform;
			transform.localScale = Vector3.one;
			transform.localRotation = Quaternion.identity;
			transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z + 100f));
		}
		overInventory = true;
	}
		
	public void OnExitInventory(InventoryDrawer inv){
		drawer = null;
		//change from icon to object
		if (holdingObject) {
			ToggleDisplayState ();
			transform.parent = null;
			runtimeIcon.transform.position = transform.position;
			overInventory = false;
		}

	}
		
	void ToggleDisplayState(){
		if (runtimeIcon.activeInHierarchy) {
			runtimeIcon.SetActive (false);
			runtimeGraphics.SetActive (true);
		} else {
			runtimeIcon.SetActive (true);
			runtimeGraphics.SetActive (false);

		}
	}


	public void PickUp(Vector3 clickPos){
		//pick up / drag
		ItemHandler.Equip(item);

		//spawn joint, etc.
		if (clickPoint != null) {
			Destroy (clickPoint);
		}
		clickPoint = new GameObject ("clickPoint");
		clickPoint.transform.position = clickPos;

		if (clickPoint.GetComponent<CharacterJoint> () == null) {
			activeJoint = clickPoint.AddComponent<CharacterJoint> ();
		}

		SoftJointLimit lim = new SoftJointLimit ();
		lim.limit = 360f;
		lim.contactDistance = 20f;
		activeJoint.swing1Limit = activeJoint.swing2Limit = lim;

		activeJoint.connectedBody = runtimeGraphics.GetComponent<Rigidbody> ();
		clickPoint.GetComponent<Rigidbody> ().useGravity = false;
		clickPoint.GetComponent<Rigidbody> ().isKinematic = true;

		holdingObject = true;
	}

	void SetIconSize(float tileSize){
		Vector2 size = new Vector2 (tileSize * item.width, tileSize * item.height);
		iconImgRect.sizeDelta = size;
		iconBorderRect.sizeDelta = size;
	}

	public void OnMouseEnter(){
		//Outline on?
	}

	public void OnMouseExit(){
		//Outline off?
	}

	public void OnMouseDown(){
		//If item is physical
		if (!inInventory) {
			Ray camToMouse = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			float maxDistance = 100f;
			if (Physics.Raycast (camToMouse, out hit, maxDistance)){
				PickUp (hit.point); 
				clickDistance = hit.distance - Camera.main.nearClipPlane;
			}
		}
	}

	public void OnMouseUp(){
		if (holdingObject && !overInventory) {
			Vector3 vel = activeJoint.connectedBody.velocity;
			activeJoint.connectedBody = null;
			holdingObject = false;
			ItemHandler.Drop (item);
		}
	}

	void OnPickup(){
		holdingObject = true;
	}

	void OnDrop(){
		holdingObject = false;
	}

	//Bad code from here
	void Update(){
		if (clickPoint != null && holdingObject) {
			if (drawer != null) { // Item is over inventory
				clickPoint.transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, drawer.gridParent.transform.position.z));
				transform.position = clickPoint.transform.position;
			} else { // Item is not over inventory
				clickPoint.transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, clickDistance));
			}
		}

		if (runtimeIcon.activeInHierarchy && holdingObject) {
//			transform.position = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z);
			SetIconSize (50f); //CHANGE
			runtimeIcon.transform.rotation = Quaternion.identity;
			Rect fis = runtimeIcon.GetComponent<RectTransform> ().rect;
			runtimeIcon.transform.position = transform.position - new Vector3 (fis.width / 2, fis.height / 2, 0);
			Vector2 relPos = new Vector2 (Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
			runtimeIcon.GetComponent<RectTransform> ().localPosition = new Vector3 ((relPos.x * fis.width) - fis.width / 2, (relPos.y * fis.height) - fis.height / 2, 0);
		}

//		if (runtimeIcon.activeInHierarchy && holdingObject) {
//			//NEW
//
//
//			//OLD
//			runtimeIcon.GetComponent<RectTransform> ().position = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z + 100f);
//			SetIconSize (50f); //CHANGE
//			runtimeIcon.transform.rotation = Quaternion.identity;
//			Rect fis = runtimeIcon.transform.parent.GetComponent<RectTransform> ().rect;
//			Vector2 relPos = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
//			runtimeIcon.GetComponent<RectTransform> ().localPosition = new Vector3 ((relPos.x * fis.width) - fis.width/2, (relPos.y * fis.height)  - fis.height/2, 0);
//
//			//check for inventory space occupiance
//			Rect space = iconImgRect.rect;
//
//		}
	}

}
