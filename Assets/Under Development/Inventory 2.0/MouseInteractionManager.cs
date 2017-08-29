﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void InteractionEvent();
public class MouseInteractionManager : MonoBehaviour {

	public static InteractionEvent OnMouseDown;
	public static InteractionEvent OnMouseUp;

	public static GameObject currentHoverObject = null;
	public static Vector3 hoverPoint = Vector3.zero;

	// Visualisation
	public bool showVisualisation = true;
	private GameObject visualiser;

	// Settings
	[Header("Settings")]
	public float defaultDistance = 5f;
	public float maxDistance = 100f;

	// Cache main cam
	private Camera cam;

	// Raycasting
	private Ray camToMouse;
	private RaycastHit hit;


	void OnEnable(){
		cam = Camera.main;

		if (showVisualisation) {
			InitializePreviewObject ();
		}
	}

	void Update(){

		HandleHoverAwareness ();

		HandleMouseInputs ();

		if (showVisualisation) {
			visualiser.transform.position = hoverPoint;
		}

	}

	void HandleHoverAwareness(){
		camToMouse = cam.ScreenPointToRay (Input.mousePosition);

		// First check for UI, then physical objects
		PointerEventData evt = new PointerEventData(EventSystem.current);
		evt.position = Input.mousePosition;

		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(evt, results);

		Vector3 camToUIHit = cam.transform.position + camToMouse.direction;

		if (results.Count > 0) { // Over UI object
			visualiser.SetActive (true);
			hoverPoint = camToUIHit.normalized * results [0].distance;
			currentHoverObject = results [0].gameObject;
		} else { // Over physical object
			// Physical objects
			if (Physics.Raycast (camToMouse, out hit, maxDistance)) {
				// Update the position of the visualiser
				if (visualiser != null) {
					visualiser.SetActive (true);
					hoverPoint = hit.point;
				}
				currentHoverObject = hit.transform.gameObject;
			} else {
				// Nothing is hit, so set the hoverpoint to the default distance 
				hoverPoint = cam.transform.position + (camToMouse.direction)*defaultDistance;
			}
		}
	}

	void HandleMouseInputs(){
		if (Input.GetMouseButtonDown (0)) { // Left click
			if (OnMouseDown != null) {
				OnMouseDown ();
			}
		}

		if (Input.GetMouseButtonUp (0)) { // Left release
			if (OnMouseUp != null) {
				OnMouseUp ();
			}
		}
	}

	private void InitializePreviewObject(){
		visualiser = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		visualiser.name = "Interaction Point Visualisation";
		visualiser.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
		Destroy (visualiser.GetComponent<Collider> ()); // Remove the collider, since it's only a visualisation and not an interactable object
		visualiser.GetComponent<MeshRenderer>().sharedMaterial.color = Color.yellow;

	}

}