using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemyProblemFever : AlchemyProblem {

	// Use this for initialization
	public override void Start () {
        base.Start();
        print(unbalancedElements[Element.Sin]);
	}
	
}
