using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEventRerouter : MonoBehaviour {

	public MonoBehaviour rerouteTo;

	public void OnMouseEnter(){
		//Outline on?
	}

	public void OnMouseExit(){
		//Outline off?
	}

	public void OnMouseDown(){
		if (rerouteTo != null) {
			rerouteTo.Invoke ("OnMouseDown", 0f);
		}
	}

	public void OnMouseUp(){
		if (rerouteTo != null) {
			rerouteTo.Invoke ("OnMouseUp", 0f);
		}
	}

}
