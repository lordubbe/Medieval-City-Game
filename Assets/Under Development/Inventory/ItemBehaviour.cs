using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBehaviour : MonoBehaviour {
	public Item item;

	[SerializeField]
	private bool holdingObject = false;

	public bool inInventory;
	public bool overInventory;

	//Behavior variables
	private GameObject displayedObject;
	private GameObject clickPoint;
	private float clickDistance;
	private CharacterJoint activeJoint;

	[Header("Assign these if possible (read tooltip)")]
	[SerializeField, Tooltip("If this object was placed in the game world, assign the graphics object to this field.")]
	private GameObject runtimeIcon;
	[SerializeField, Tooltip("If this object was placed directly in an inventory, assign the icon object to this field.")]
	private GameObject runtimeGraphics;

	//Icon
	private Image iconImg;
	private RectTransform iconImgRect;
	private RectTransform iconBorderRect;

	void Awake(){
		//Prepare the runtime objects for spawning/despawning
		Init (); 

		//Subscribe to inventory events
		InventoryEvents.OnInventoryEnter += OnEnterInventory;
		InventoryEvents.OnInventoryExit += OnExitInventory;
	}

	void Init(){
		//Spawn and reference runtime objects 
		if (runtimeIcon == null) {
			//Item icon
			runtimeIcon = new GameObject ("Icon");
			iconImg = runtimeIcon.AddComponent<Image> ();
			iconImg.sprite = item.icon;
			iconImg.raycastTarget = false;
			iconImgRect = iconImg.GetComponent<RectTransform> ();
			//Item border
			GameObject border = new GameObject("Icon_border");
			border.transform.parent = iconImg.transform;
			border.transform.localPosition = Vector3.zero;
			border.transform.localScale = Vector3.one;
			Image img = border.AddComponent<Image> ();
			img.sprite = item.iconBorder;
			img.raycastTarget = false;
			iconBorderRect = border.GetComponent<RectTransform> ();

			//set parents
			runtimeIcon.transform.parent = transform;
			runtimeGraphics.transform.parent = transform;
		}

		if (runtimeGraphics == null) {
			runtimeGraphics = Instantiate (item.runtimeRepresentation, transform.position, Quaternion.identity, transform) as GameObject;
		}

		//Toggle display state
		if (inInventory) {
			runtimeGraphics.GetComponent<MeshRenderer> ().enabled = false;
			runtimeIcon.SetActive (true);
		} else {
			runtimeGraphics.GetComponent<MeshRenderer> ().enabled = true;
			runtimeIcon.SetActive (false);
		}
	}
	
	public void OnEnterInventory(Inventory inv){
		//change from object to icon
		if (holdingObject) {

			//Set the size of the tiles in this inventory
			if (inv.tileSize != null) {
				SetIconSize (inv.tileSize);
			}
			ToggleDisplayState ();

			runtimeIcon.transform.parent = inv.drawer.inventoryCanvas.transform;
			runtimeIcon.transform.localScale = Vector3.one;
			runtimeIcon.transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z + 100f));
		}
		overInventory = true;
	}
		
	public void OnExitInventory(Inventory inv){
		//change from icon to object
		if (holdingObject) {
			ToggleDisplayState ();
			runtimeIcon.transform.parent = transform;
			runtimeIcon.transform.position = transform.position;
			overInventory = false;
		}

	}
		
	void ToggleDisplayState(){
		if (runtimeIcon.activeInHierarchy) {
			runtimeIcon.SetActive (false);
			runtimeGraphics.GetComponent<MeshRenderer> ().enabled = true;
			runtimeGraphics.GetComponent<Collider> ().enabled = true;
		} else {
			runtimeIcon.SetActive (true);
			runtimeGraphics.GetComponent<MeshRenderer> ().enabled = false;
			runtimeGraphics.GetComponent<Collider> ().enabled = false;

		}
	}


	public void OnPickup(Vector3 clickPos){
		//pick up / drag
		ItemHandler.Equip(gameObject);

		//spawn joint, etc.
		if (clickPoint != null) {
			Destroy (clickPoint);
		}
		clickPoint = new GameObject ("clickPoint");
		clickPoint.transform.position = clickPos;

		if (clickPoint.GetComponent<CharacterJoint> () == null) {
			activeJoint = clickPoint.AddComponent<CharacterJoint> ();
			SoftJointLimit lim = new SoftJointLimit ();
			lim.limit = 360f;
			lim.contactDistance = 20f;
			activeJoint.swing1Limit = activeJoint.swing2Limit = lim;
		}
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
				OnPickup (hit.point);
				clickDistance = hit.distance - Camera.main.nearClipPlane;
			}
		}
	}

	public void OnMouseUp(){
		if (holdingObject && !overInventory) {
			Vector3 vel = activeJoint.connectedBody.velocity;
			activeJoint.connectedBody = null;
			holdingObject = false;
			ItemHandler.Drop (gameObject);
		}
	}



	//Bad code from here
	void Update(){
		if (clickPoint != null && holdingObject) {
			clickPoint.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, clickDistance));
		}
		if (runtimeIcon.activeInHierarchy) {
			runtimeIcon.GetComponent<RectTransform> ().position = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z + 100f);
			SetIconSize (50f); //CHANGE
			runtimeIcon.transform.rotation = Quaternion.identity;
			Rect fis = runtimeIcon.transform.parent.GetComponent<RectTransform> ().rect;
			Vector2 relPos = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
			runtimeIcon.GetComponent<RectTransform> ().localPosition = new Vector3 ((relPos.x * fis.width) - fis.width/2, (relPos.y * fis.height)  - fis.height/2, 0);

			//check for inventory space occupiance
			Rect space = iconImgRect.rect;

		}
	}

}
