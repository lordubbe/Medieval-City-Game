﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableWindow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler {

	public Inventory inventory; //TODO: Automatically assign pls

	private RectTransform windowRect;
	private bool draggingWindow;
	private bool altDown;

	Vector3 prevMousePos = Vector3.zero;

	//Pointer events
	public void OnPointerEnter(PointerEventData evtData){
		//Something about checking if player is holding an object, and if it does then send that object along
		if (InventoryEvents.OnInventoryEnter != null) {
			InventoryEvents.OnInventoryEnter (inventory);
		}
	}

	public void OnPointerExit(PointerEventData evtData){
		if (InventoryEvents.OnInventoryExit != null) {
			InventoryEvents.OnInventoryExit (inventory);
		}
	}

	public void OnPointerDown(PointerEventData evtData){
		if (altDown) {
			prevMousePos = Input.mousePosition;
			draggingWindow = true;
		}
	}

	public void OnPointerUp(PointerEventData evtData){
		// If dragging, no longer drag window
		draggingWindow = false;

		// Handle adding an item to the inventory
//		if (ItemHandler.currentlyHeldItem != null) {
			
//		}
	}
		
	void OnEnable(){
		windowRect = GetComponent<RectTransform>();
	}

	void Update(){

		Ray testRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		Rect testRect = windowRect.rect;
		Vector3 mousePos = Input.mousePosition;
		mousePos.x = Mathf.Clamp (mousePos.x, 0, Screen.width);
		mousePos.y = Mathf.Clamp (mousePos.y, 0, Screen.height);

//		Debug.Log (mousePos + "(" + testRect + ")");



		if (Input.GetKeyDown (KeyCode.LeftAlt)) {
			altDown = true;
		}
		if (Input.GetKeyUp (KeyCode.LeftAlt)) {
			altDown = false;
		}

		if (draggingWindow) {
			Vector3 delta = Input.mousePosition - prevMousePos;
			Rect pos = windowRect.rect;
			windowRect.localPosition += new Vector3(delta.x, delta.y, 0); //TODO: Fix to work with screen space camera
			
			//Clamping is fucked right now because the rect width can be larger than Screen.width (yet be smaller visually). I guess it depends on the Canvas rect.
//			windowRect.position = new Vector3(Mathf.Clamp(windowRect.position.x, 0, Screen.width - windowRect.rect.width), windowRect.position.y);

			prevMousePos = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp (0)) {
			if (ItemHandler.currentlyHeldItem != null) {
				//add to inventory pls m'jacob
				if (InventoryEvents.OnDropItem != null) {

					InventoryEvents.OnDropItem (ItemHandler.currentlyHeldItem);
				}
			}
		}

	}
}