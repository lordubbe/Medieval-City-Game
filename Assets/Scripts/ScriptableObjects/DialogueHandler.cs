using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHandler : MonoBehaviour {

    public NodeDummy node;
    string idd;

	// Use this for initialization
	void Start () {

        idd = node.id;
        print(node.text);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
