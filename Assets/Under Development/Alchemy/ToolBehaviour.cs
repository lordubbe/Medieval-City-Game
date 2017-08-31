using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBehaviour : MonoBehaviour {

    private AlchemyTool tool;
    public Canvas toolCanvas;

	// Use this for initialization
	void Start () {


        MouseInteractionManager.OnMouseDown += OnMouseDown;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnMouseDown()
    {
        // click and open tool item window.
        toolCanvas.gameObject.SetActive(true);
        //find correct position and move ??

        //get and draw pentagon shape
    }

    public void OnMouseEnter()
    {
        //outline?
    }

    public void OnMouseExit()
    {
        //outline?
    }

}
