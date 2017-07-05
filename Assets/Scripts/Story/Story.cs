using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Story : MonoBehaviour {

	public string storyname;
	public bool isActive = false;
	public List<Flowchart> flowcharts = new List<Flowchart>();
	public Dictionary<string, bool> storyflags = new Dictionary<string,bool>();
	public List<StoryState> states = new List<StoryState>();
	StoryState curState;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	public void ChangeState(StoryState s){
		curState.OnExit();

		curState = s;

		curState.OnCall();
	}




}

