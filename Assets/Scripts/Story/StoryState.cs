using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class StoryState : SerializedMonoBehaviour {

    public Story myStory;
	public string statename;
    public string description;
    [SerializeField] public Dictionary<string, Option> optionsToAdd = new Dictionary<string, Option>();
    public Quality[] qualityReqs;

    public StoryState() { }
    
    public StoryState(Story s) { myStory = s; }

    public StoryState(Story s, string nam, string desc, Quality[] requirements)
    {
        myStory = s;
        statename = nam;
        description = desc;
        qualityReqs = requirements;
    }
    
    public virtual void OnStateEnter(){
        
        foreach(string n in optionsToAdd.Keys)
        {
            print(n);
            myStory.sm.convos.FindNode(n).options.Add(optionsToAdd[n]);
        }
        StartCoroutine("CheckCompletion");
	}

	public virtual void OnStateExit(){
        StopCoroutine("CheckCompletion");
    }

    public virtual IEnumerator CheckCompletion()
    {
        while (true)
        {
            print("checking qualities");
            foreach(Quality q in qualityReqs)
            {
                if (myStory.sm.allQualities.Exists(x=>x.id == q.id))
                {
                    if(myStory.sm.allQualities[myStory.sm.allQualities.FindIndex(x=>x.id==q.id)].GetValue() == q.GetValue())
                    {
                        myStory.ChangeState(q); //Now changes if ONE OF THEM WAS true, which allows us to branch :D
                    }
                }
            }
            yield return new WaitForSeconds(1);
        }
    }
}




public class LovePotionStoryStateBegin : StoryState
{
    public LovePotionStoryStateBegin() { }

    public LovePotionStoryStateBegin(Story s) : base(s) { }

    public LovePotionStoryStateBegin(Story s, string nam, string desc, Quality[] reqss) : base(s, nam, desc, reqss) { }
    
    public override void OnStateEnter()
    {
        base.OnStateEnter();
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }
    


}

