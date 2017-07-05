using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : ScriptableObject{

	public string name;
	public GameObject runtimeRepresentation;
	public string flavorText;

	public int width = 1;
	public int height = 1;

	public Texture2D iconTexture;
	public Sprite icon;

	[HideInInspector]
	public IconSettings iconSettings;

	//Settings
	

	/// <summary>
	/// Initializes a new <see cref="Item"/> with the given information.
	/// </summary>
	/// <param name="prefab">The runtime representation of the Item.</param>
	/// <param name="dimensions">The inventory dimensions of the item in the format "WxH" (eg. "2x4" is an item that is 2 squares wide and 4 squares tall)</param>
	/// <param name="text">The flavor text of the item.</param>
	public Item(string name, GameObject prefab, string dimensions, string text){
		string[] dim = dimensions.Split (new char[]{'x'}, 2);
		width = int.Parse(dim [0]);
		height = int.Parse(dim [1]);
		flavorText = text;
		runtimeRepresentation = prefab;
	}

	void OnValidate(){
		if (width < 1) {
			width = 1;
		}
		if (height < 1) {
			height = 1;
		}
	}

}

[System.Serializable]
public class IconSettings{
	public Vector3 itemOffset = Vector3.zero;
	public Vector3 itemRotation = Vector3.zero;
	public bool orthographicCamera = true;
	public float orthographicScale = 5f;

	public IconSettings(Vector3 offset, Vector3 rotation, bool orthographic, float orthographicSize){
		itemOffset = offset;
		itemRotation = rotation;
		orthographicCamera = orthographic;
		orthographicScale = orthographicSize;
	}
}
