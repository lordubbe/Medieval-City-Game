//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class ToolBehaviour : MonoBehaviour {
    
//    private AlchemyTool tool;
//    public Canvas toolCanvas;
//    public Transform pentaSpot;

//	// Use this for initialization
//	void Start () {

//        tool = GetComponent<AlchemyTool>();
//        MouseInteractionManager.OnMouseDown += OnMouseDown;
//	}
	
//	// Update is called once per frame
//	void Update () {
		
//	}


//    private void OnMouseDown()
//    {
//        // click and open tool item window.
//        toolCanvas.gameObject.SetActive(true);
//        Alchemy.Instance.DrawElementPentagon(tool.elements, pentaSpot);
//        //find correct position and move ??

//        //get and draw pentagon shape
//    }

//    public void OnMouseEnter()
//    {
//        GameObject g = Alchemy.Instance.DrawElementPentagon(tool.elements, pentaSpot);
//        //outline?
//    }

//    public void OnMouseExit()
//    {
//        //outline?
//    }

//}
