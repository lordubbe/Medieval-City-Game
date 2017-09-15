using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Story : MonoBehaviour {

    public StoryManager sm;
	public string storyname;
	public bool isActive = false;
	public bool isCompleted = false;
	[SerializeField] List<string> flagsEditable = new List<string>();
	public Dictionary<string, bool> storyflags = new Dictionary<string,bool>();
    [SerializeField] List<Quality> qualityEditable = new List<Quality>(); //dumb thing because dictionaries not serializable.
    [SerializeField] List<StoryState> statesEditable = new List<StoryState>();
    public Dictionary<Quality, StoryState> states = new Dictionary<Quality, StoryState>();
    public StoryState startState;
	StoryState curState;

    public Story(StoryManager s) { sm = s; }

    public Story(StoryManager s, string nam, Dictionary<string, bool> flags)
    {
        sm = s;
        storyname = nam;
        storyflags = flags;
    }


	void OnEnable(){

		foreach(string s in flagsEditable){
			storyflags.Add(s,false);
		}

        for (int i = 0; i < statesEditable.Count; i++)
        {
            states.Add(qualityEditable[i], statesEditable[i]);
        }

	}

	public void ChangeState(Quality qDeterminant){
		curState.OnExit();

		curState = states[qDeterminant];

		curState.OnEnter();
	}

    public void ChangeState(StoryState s)
    {
        curState.OnExit();

        curState = s;

        curState.OnEnter();
    }


    public void StartStory(){


        

	}
    

}

