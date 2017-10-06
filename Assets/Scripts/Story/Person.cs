using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Person : MonoBehaviour {

    public string name = "";
    public StoryManager storyMan;
    public string defaultDialogueID;

    public void InitPerson(StoryManager s)
    {
        storyMan = s;
    }
    


    public void InConvoRange()
    {
        storyMan.convos.AddPersonInRange(this);
    }

    public void OutsideConvoRange()
    {
        storyMan.convos.RemovePersonFromRange(this);
    }




}
