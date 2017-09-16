using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AlchemyStoryState : StoryState {

    public Elements elementsRequired;

    public AlchemyStoryState() { }

    public AlchemyStoryState(Story s) : base(s) { }

    public AlchemyStoryState(Story s, string nam, string desc, Quality[] reqss) : base(s, nam, desc, reqss) { }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }

    public override IEnumerator CheckCompletion()
    {
        while (true)
        {
            foreach (Quality q in qualityReqs)
            {
                if (myStory.sm.allQualities.Exists(x => x.id == q.id))
                {
                    if (myStory.sm.allQualities[myStory.sm.allQualities.FindIndex(x => x.id == q.id)].GetValue() == q.GetValue())
                    {
                        myStory.ChangeState(q); //Now changes if ONE OF THEM WAS true, which allows us to branch :D
                    }
                }
            }
            yield return new WaitForSeconds(1);
        }
    }

}
