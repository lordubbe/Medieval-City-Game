using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationManager : MonoBehaviour {

    public List<Person> pplInConvoRange = new List<Person>();


    public void AddPersonInRange(Person p)
    {
        pplInConvoRange.Add(p);
    }

    public void RemovePersonFromRange(Person p)
    {
        pplInConvoRange.Remove(p);
    }




    public void StartConversation(Person p)
    {

    }






    //SOMEWHERE that handles input or here. if we're looking at them, AND they're in that list, we got it bro. (we'll add an interaction option, and start convo if pressed).


}
