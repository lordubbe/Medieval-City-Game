using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableWindow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	private RectTransform windowRect;
	private bool draggingWindow;
	private bool ctrlDown;

	Vector3 prevMousePos = Vector3.zero;

	public void OnPointerDown(PointerEventData evtData){
		if (ctrlDown) {
			prevMousePos = Input.mousePosition;
			draggingWindow = true; 
		}
	}

	public void OnPointerUp(PointerEventData evtData){
		// If dragging, no longer drag window
		draggingWindow = false;
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

		if (Input.GetKeyDown (KeyCode.LeftControl)) {
			ctrlDown = true;
		}
		if (Input.GetKeyUp (KeyCode.LeftControl)) {
			ctrlDown = false;
		}

		if (draggingWindow) {
			Vector3 delta = Input.mousePosition - prevMousePos;
			Rect pos = windowRect.rect;
			windowRect.localPosition += new Vector3(delta.x, delta.y, 0); //TODO: Fix to work with screen space camera
			
			//Clamping is fucked right now because the rect width can be larger than Screen.width (yet be smaller visually). I guess it depends on the Canvas rect.
//			windowRect.position = new Vector3(Mathf.Clamp(windowRect.position.x, 0, Screen.width - windowRect.rect.width), windowRect.position.y);

			prevMousePos = Input.mousePosition;
		}
	}
}
