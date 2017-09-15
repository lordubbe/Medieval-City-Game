using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class OdinTest : MonoBehaviour {

	[TabGroup("First tab")]
	public Dictionary<string, string> myDictionary;

	[TabGroup("nej")]
	public int hej;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
