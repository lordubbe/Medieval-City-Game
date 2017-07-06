using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemyMixer : MonoBehaviour {

    public Alchemy alc;

    List<AlchemyIngredient> ingredientsToMix = new List<AlchemyIngredient>();

	// Use this for initialization
	void Start () {
		
	}



    public void AddIngredient(AlchemyIngredient i)
    {
        ingredientsToMix.Add(i);
    }

    public void Mix()
    {
        alc.MixIngredients(ingredientsToMix[0], ingredientsToMix[1],transform.position + Vector3.up);
    }



}
