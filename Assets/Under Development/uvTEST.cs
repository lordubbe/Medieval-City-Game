using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uvTEST : MonoBehaviour {

    public Mesh m;

    // Use this for initialization
    void Start() {

        m = GetComponent<MeshFilter>().mesh;

        Vector2[] uvsss = new Vector2[4];
        uvsss[0] = Vector2.zero;
        uvsss[1] = Vector2.zero;
        uvsss[2] = Vector2.one;
        uvsss[3] = Vector2.one;

        m.uv = uvsss;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
