using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour {

    public StoryManager storyMan;

	// Use this for initialization
	void Start () {
		
	}
	
	


    public void InConvoRange()
    {
        storyMan.convos.AddPersonInRange(this);
    }

    public void OutsideConvoRange()
    {
        storyMan.convos.RemovePersonFromRange(this);
    }




}
