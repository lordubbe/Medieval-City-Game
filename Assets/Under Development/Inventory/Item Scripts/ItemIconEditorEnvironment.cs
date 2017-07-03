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
}
