using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ItemIconEditorEnvironment : MonoBehaviour {
	[HideInInspector]
	public bool hasSpawnedModel;
	[HideInInspector]
	public bool BGactive;
	public GameObject BG;
	public Camera camera;
	public Transform itemParent;

	void Update(){
		if (BGactive && !BG.activeInHierarchy) {
			BG.SetActive (true);
		} else if (!BGactive && BG.activeInHierarchy) {
			BG.SetActive (false);
		}
	}

	public void Init(Item i){
		if (!hasSpawnedModel) {
			GameObject itemModel = (GameObject)Instantiate (i.runtimeRepresentation, itemParent.transform.position, Quaternion.identity, itemParent);

			//set layer to the one rendered by the icon environment
			itemModel.layer = LayerMask.NameToLayer ("Item Setup");
			foreach (Transform child in itemModel.transform) {
				child.gameObject.layer = LayerMask.NameToLayer ("Item Setup");
			}
			hasSpawnedModel = true;
		}
	}
}
