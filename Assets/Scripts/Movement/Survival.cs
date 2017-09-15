using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survival : MonoBehaviour {

    public float food = 100;
    public float water = 100;
    public float sleep = 100;

	// Update is called once per frame
	void Update () {

        food -= Time.deltaTime;
        water -= Time.deltaTime;
        sleep -= Time.deltaTime;

    }
}
