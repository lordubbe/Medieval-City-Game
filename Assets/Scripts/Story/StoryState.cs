using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OptionDictionary : SerializableDictionary<Node, Option>
{

}

public class StoryState : MonoBehaviour {

    public Story myStory;
	public string statename;
    public string description;
    [SerializeField] public OptionDictionary optionsToAdd = new OptionDictionary();
    [SerializeField] public SerializableDictionary<string, int> opp = new SerializableDictionary<string, int>();
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
    
    public virtual void OnEnter(){
        foreach(Node n in optionsToAdd.Keys)
        {
            n.options.Add(optionsToAdd[n]);
        }
        StartCoroutine("CheckCompletion");
	}

	public virtual void OnExit(){
        StopCoroutine("CheckCompletion");
    }

    public virtual IEnumerator CheckCompletion()
    {
        while (true)
        {
            foreach(Quality q in qualityReqs)
            {
                if (myStory.sm.allQualities.ContainsKey(q.id))
                {
                    if(myStory.sm.allQualities[q.id].GetValue() == q.GetValue())
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
    
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override IEnumerator CheckCompletion()
    {
        while (true)
        {

            yield return new WaitForSeconds(1);
        }
    }


}

