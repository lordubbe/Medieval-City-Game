using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survival : MonoBehaviour {

    public float food = 100;
    public float water = 100;
    public float sleep = 100;

    public float foodrate = 0.5f;
    public float waterrate = 0.8f;
    public float sleeprate = 0.1f;

    public UIManager uiman;


	// Update is called once per frame
	void Update () {

        if(food > 0)
        {
            food -= Time.deltaTime * foodrate;
        }
        if(water > 0)
        {
            water -= Time.deltaTime * waterrate;
        }
        if(sleep > 0)
        {
            sleep -= Time.deltaTime * sleeprate;
        }

        uiman.UpdateSurvivalUI(food, water, sleep);

    }


    public void AddFood(float toAdd)
    {
        food += toAdd;
    }

    public void AddWater(float toAdd)
    {
        water += toAdd;
    }

    public void AddSleep(float toAdd)
    {
        water += toAdd;
    }



}
