using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationManager : MonoBehaviour {

    public Dictionary<string, NodeGroup> allconversations = new Dictionary<string, NodeGroup>();
    public StoryManager sm;
    public DialogueHandler dialHandler;

    public List<Person> pplInConvoRange = new List<Person>();

    public void Start()
    {
        //DUMMY THINGSSSSSSS //////
        Person p = new Person();
        p.name = "Karl";

        NodeGroup ng = new NodeGroup();
        ng.id = "firstConversation";

        Node n = new Node();
        n.id = "n1";
        n.text = "Hello, and welcome to my inn!";
        n.characterSpeaking = p;

        Node nn = new Node();
        nn.id = "n2";
        nn.text = "you think this is HEARTHSTONE????";
        nn.characterSpeaking = p;

        Node nnn = new Node();
        nnn.id = "n3";
        nnn.text = "well, alright, then";
        nnn.characterSpeaking = p;

        Option o = new Option();
        o.text = "Pull up in a chair!";
        o.linkToNextNode = nn;

        n.options.Add(o);

        ng.nodes.Add(n.id,n);
        ng.nodes.Add(nn.id,nn);
        ng.nodes.Add(nnn.id,nnn);
        allconversations.Add(ng.id,ng);

        print(n);

        StartConversation(p, n);
        ///// OVER
        
        
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Option po = new Option();
            po.text = "Well, isn't it?";
            po.linkToNextNode = allconversations["firstConversation"].nodes["n3"];
            po.conditions.Add(new FlagCondition(flag.liketheinnkeeper, true));
            allconversations["firstConversation"].nodes["n2"].options.Add(po);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            sm.storyflags[flag.liketheinnkeeper] = true;
        }
    }

    public void AddPersonInRange(Person p)
    {
        pplInConvoRange.Add(p);
    }

    public void RemovePersonFromRange(Person p)
    {
        pplInConvoRange.Remove(p);
    }

    public void StartConversation(Person p, Node n)
    {
        dialHandler.StartDialogue(p, n);
    }






    //SOMEWHERE that handles input or here. if we're looking at them, AND they're in that list, we got it bro. (we'll add an interaction option, and start convo if pressed).


}
