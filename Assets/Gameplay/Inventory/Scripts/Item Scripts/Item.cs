﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTag { Liquid, Solid, Powder, Gas, Organic, Dead };
[System.Serializable]
public class Item : MonoBehaviour{

    public string name;

	public GameObject runtimeRepresentation;
	public string flavorText;

	public int width = 1;
	public int height = 1;

	public Texture2D iconTexture;
	public Sprite icon;
	public Sprite iconBorder;

	//[HideInInspector]
	public IconSettings iconSettings;

    private Elements elements = new Elements();
    public List<ItemAttribute> attributes;
    public List<ItemTag> tags = new List<ItemTag>();

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

	public Item(){
		width = 1;
		height = 1;
	}

	void OnValidate(){
		if (width < 1) {
			width = 1;
		}
		if (height < 1) {
			height = 1;
		}
	}

    public virtual Elements GetElements()
    {
        Elements elToSend = elements;
        foreach(ItemAttribute a in attributes)
        {
            elToSend += a.elementsModifier;
        }
        return elToSend;
    }

}

[System.Serializable]
public class IconSettings{
	public Vector3 itemOffset = Vector3.zero;
	public Vector3 itemRotation = Vector3.zero;
	public float itemDistance = 4f;
	public bool orthographicCamera = true;
	public float orthographicScale = 5f;

	public IconSettings(Vector3 offset, Vector3 rotation, bool orthographic, float orthographicSize){
		itemOffset = offset;
		itemRotation = rotation;
		orthographicCamera = orthographic;
		orthographicScale = orthographicSize;
		itemDistance = 4f;
	}
}
