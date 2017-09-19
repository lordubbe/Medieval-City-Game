using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Person {

    public string name = "";
    public StoryManager storyMan;
    


    public void InConvoRange()
    {
        storyMan.convos.AddPersonInRange(this);
    }

    public void OutsideConvoRange()
    {
        storyMan.convos.RemovePersonFromRange(this);
    }




}
