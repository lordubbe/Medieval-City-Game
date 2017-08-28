using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class INeedAPentagon : MonoBehaviour {

    public Elements el;

    public PentagonShape pen;

    public Material customMat;

	// Use this for initialization
	void Start () {

       // pen.DrawElementPentagon(el, GetComponent<CanvasRenderer>());
        StartCoroutine(waittodraw());
		
	}
	
	// Update is called once per frame
	void Update () {
        pen.DrawElementPentagon(el, transform);
        pen.DrawElementPentagonWithMat(new Elements(100,100,100,88,19), transform, customMat);




    }


    IEnumerator waittodraw()
    {
        yield return new WaitForSeconds(0.05f);
     //   pen.DrawElementPentagon(el, transform);

    }

}
