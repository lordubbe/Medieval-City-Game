using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoryStateAlchemy : StoryState {

    public Item itemToTest;
    public float threshold = 0.05f;

    public StoryStateAlchemy() { }

    public StoryStateAlchemy(Story s) : base(s) { }

    public StoryStateAlchemy(Story s, string nam, string desc, Quality[] reqss) : base(s, nam, desc, reqss) { }

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
            if(itemToTest != null)
            {
                foreach(QualityAlchemy qa in qualityReqs)
                {
                    if (Alchemy.Instance.TestSimilarity(itemToTest.GetElements(), qa.GetElements()) < threshold)
                    {
                        myStory.ChangeState(qa);
                    }
                }
            }
            yield return new WaitForSeconds(1);
        }
    }

}
