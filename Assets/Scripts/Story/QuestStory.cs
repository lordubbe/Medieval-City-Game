using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestStory : MonoBehaviour {

	public string storyname;
	public bool isActive = false;
	public bool isCompleted = false;
	[SerializeField] List<string> flagsEditable = new List<string>();
	public Dictionary<string, bool> storyflags = new Dictionary<string,bool>();
	public List<StoryState> states = new List<StoryState>();
	StoryState curState;

	void OnEnable(){

		foreach(string s in flagsEditable){
			storyflags.Add(s,false);
		}

		states = GetComponentsInChildren<StoryState>().ToList();

	}

	public void ChangeState(StoryState s){
		curState.OnExit();

		curState = s;

		curState.OnCall();
	}


	public void StartStory(bool activateFlowchart){


        

	}
    

}

