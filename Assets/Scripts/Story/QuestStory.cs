using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using System.Linq;

public class QuestStory : MonoBehaviour {

	public string storyname;
	public bool isActive = false;
	public bool isCompleted = false;
	[SerializeField] List<string> flagsEditable = new List<string>();
	public Dictionary<string, bool> storyflags = new Dictionary<string,bool>();
	public List<StoryState> states = new List<StoryState>();
	public List<Flowchart> flowcharts = new List<Flowchart>();
	StoryState curState;

	void OnEnable(){

		foreach(string s in flagsEditable){
			storyflags.Add(s,false);
		}

		states = GetComponentsInChildren<StoryState>().ToList();

		flowcharts = GetComponentsInChildren<Flowchart>().ToList();

	}

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
		

		StringVariable va = new StringVariable();
		va.Value = "thebeststring";
		va.Key = "newVar";
		f.Variables.Add(va);

		//SaveVariable(va.Key,va);

		if(flowcharts.Contains(f)){
			f.ExecuteBlock("Start");
		}

	}


	//OK. getting annoyed at fungus now. We can't add variables from script. which makes sense. but how the fuck do I deal with the complete amount of flowcharts we're going to have then?


}

