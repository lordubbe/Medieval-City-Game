using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalTempManager : MonoBehaviour {

    public Journal journal;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.J))
        {
            journal.gameObject.SetActive(true);
        }

	}
}
