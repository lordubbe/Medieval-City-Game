using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RuntimeInventoryTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public int x, y;

	public InventoryDrawer drawer;
	public Image image;
	public InventorySpace inventorySpace;

	RectTransform space;
	bool inTile = false;
	Vector2 currentMousePos = Vector2.zero;
	Vector2 currentLocalPos = Vector2.zero;

	void Awake(){
		space = GetComponent<RectTransform> ();
		image = GetComponent<Image> ();
	}

	public void OnPointerEnter(PointerEventData evt){
		inTile = true;
		if(ItemHandler.currentItem != null){

			StartCoroutine("checkMousePos");
		}
	}

	public void OnPointerExit(PointerEventData evt){
		inTile = false;
	}

	IEnumerator checkMousePos(){
		while (inTile) {
			if (ItemHandler.currentItem != null) {
				Vector2 newMousePos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);

				if (currentMousePos != newMousePos) {
					currentMousePos = newMousePos;
					currentLocalPos = Vector2.zero;
					RectTransformUtility.ScreenPointToLocalPointInRectangle (space, currentMousePos, Camera.main, out currentLocalPos);
					currentLocalPos = Rect.PointToNormalized (space.rect, currentLocalPos);

					int _x = currentLocalPos.x < 0.5 ? x : Mathf.Clamp (x + 1, 0, drawer.inventory.inventoryWidth - 1);
					int _y = currentLocalPos.y > 0.5 ? y : Mathf.Clamp (y + 1, 0, drawer.inventory.inventoryHeight - 1);

					drawer.CheckOccupiance (ItemHandler.currentItem, _x, _y); //put in actual item
				}
			}
			yield return null;
		}
	}
		
}
