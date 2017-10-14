using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

public class Story : SerializedMonoBehaviour {

    public StoryManager sm;
	public string storyname;
    public string description;
	public bool isActive = false;
	public bool isCompleted = false;
	public Dictionary<string, bool> storyflags = new Dictionary<string,bool>();
    public Dictionary<Quality, StoryState> states = new Dictionary<Quality, StoryState>();
    public StoryState startState;
	public StoryState curState;

    public Story(StoryManager s) { sm = s; }

    public Story(StoryManager s, string nam, Dictionary<string, bool> flags)
    {
        sm = s;
        storyname = nam;
        storyflags = flags;
    }
    

	public void ChangeState(Quality qDeterminant)
    {
        print("Changing state by " + qDeterminant.id);
        curState.OnStateExit();

		curState = states[qDeterminant];

		curState.OnStateEnter();

        if (sm.uiman != null)
        {
            sm.uiman.DisplayUpdate(UpdateType.StoryUpdate, storyname);
        }
    }

    public void ChangeState(StoryState s)
    {
        if(curState != null)
        {
            curState.OnStateExit();
        }

        curState = s;

        curState.OnStateEnter();


        if (sm.uiman != null)
        {
            sm.uiman.DisplayUpdate(UpdateType.StoryUpdate, storyname);
        }

    }


    public void StartStory(){
        ChangeState(startState);
	}
    
    public string BuildDescription()
    {
        return description + "\n\n" + curState.description;
    }


}

