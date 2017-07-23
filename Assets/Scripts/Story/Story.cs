using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Story : MonoBehaviour {

	public string storyname;
	public bool isActive = false;
	public bool isCompleted = false;
	public List<Flowchart> flowcharts = new List<Flowchart>();
	public Dictionary<string, bool> storyflags = new Dictionary<string,bool>();
	public List<StoryState> states = new List<StoryState>();
	StoryState curState;


	public void ChangeState(StoryState s){
		curState.OnExit();

		curState = s;

		curState.OnCall();
	}


	public void StartStory(bool activateFlowchart){


		if(activateFlowchart){
			ActivateFlowchart(flowcharts[0]); //???????????????
		}

	}

	public void ActivateFlowchart(Flowchart f){

		if(flowcharts.Contains(f)){
			f.ExecuteBlock("Start");
		}

	}


	//OH I KNOW. make stories actual gameobjects. and fiddle with them there (then I can do editorstuff as well.). and then add them from flowcharts
	//would be nice with an automatic.. like, find the flowchart..
	//Yeah, that should work. send in the flowchart instead, and I'll find the appropriate story for it. Yup.



}

