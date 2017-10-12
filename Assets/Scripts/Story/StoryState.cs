using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class StoryState : SerializedMonoBehaviour {

    public Story myStory;
	public string statename;
    public string description;
    [SerializeField] public Dictionary<string, Option> optionsToAdd = new Dictionary<string, Option>();
	public Dictionary<Quality, int> qualityReqs = new Dictionary<Quality, int>();

    public StoryState() { }
    
    public StoryState(Story s) { myStory = s; }

	public StoryState(Story s, string nam, string desc, Dictionary<Quality,int> requirements)
    {
        myStory = s;
        statename = nam;
        description = desc;
        qualityReqs = requirements;
    }
    
    public virtual void OnStateEnter(){
        
        foreach(string n in optionsToAdd.Keys)
        {
            myStory.sm.convos.FindNode(n).options.Add(optionsToAdd[n]);
        }
        StartCoroutine("CheckCompletion");
	}

	public virtual void OnStateExit(){
        foreach (string n in optionsToAdd.Keys)
        {
            myStory.sm.convos.FindNode(n).options.Remove(optionsToAdd[n]);
        }
        StopCoroutine("CheckCompletion");
    }

    public virtual IEnumerator CheckCompletion()
    {
        while (true)
        {
			foreach(Quality q in qualityReqs.Keys)
            {
                if (myStory.sm.allQualities.Exists(x=>x.id == q.id))
                {
					if(myStory.sm.allQualities[myStory.sm.allQualities.FindIndex(x=>x.id==q.id)].GetValue() == qualityReqs[q])
                    {
                        myStory.ChangeState(q); //Now changes if ONE OF THEM WAS true, which allows us to branch :D
                    }
                }
            }
            yield return new WaitForSeconds(1);
        }
    }
}



