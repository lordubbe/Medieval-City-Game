using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ConversationManager : MonoBehaviour {

    List<NodeCreator> creators = new List<NodeCreator>();
    public Dictionary<string, NodeGroup> allconversations = new Dictionary<string, NodeGroup>();
    public StoryManager sm;
    public DialogueHandler dialHandler;

    public List<Person> pplInConvoRange = new List<Person>();

    public bool inConversation;
    public Person personSpeakingWith;

    public string DEBUGSTORY = "firstconvo_0";

    public void Awake()
    {
        creators = GetComponentsInChildren<NodeCreator>().ToList();



        foreach (NodeCreator nc in creators)
        {
            NodeGroup nn = new NodeGroup();
            nn.id = nc.id;
            foreach(Node n in nc.nodes)
            {
                nn.nodes.Add(n.id, n);
            }
            allconversations.Add(nn.id, nn);
        }
        ////DUMMY THINGSSSSSSS //////
        //Person p = new Person();
        //p.name = "Karl";

        //NodeGroup ng = new NodeGroup();
        //ng.id = "firstConversation";

        //Node n = new Node();
        //n.id = "n1";
        //n.text = "Hello, and welcome to my inn!";
        //n.characterSpeaking = p;

        //Node nn = new Node();
        //nn.id = "n2";
        //nn.text = "you think this is HEARTHSTONE????";
        //nn.characterSpeaking = p;

        //Node nnn = new Node();
        //nnn.id = "n3";
        //nnn.text = "well, alright, then";
        //nnn.characterSpeaking = p;

        //Option o = new Option();
        //o.text = "Pull up in a chair!";
        //o.linkToNextNode = nn.id;

        //n.options.Add(o);

        //ng.nodes.Add(n.id,n);
        //ng.nodes.Add(nn.id,nn);
        //ng.nodes.Add(nnn.id,nnn);
        //allconversations.Add(ng.id,ng);

        //StartConversation(p, n);
        ///// OVER
        
        
    }

    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    Option po = new Option();
        //    po.text = "Well, isn't it?";
        //    po.linkToNextNode = allconversations["firstConversation"].nodes["n3"].id;
        //    po.conditions.Add(new FlagCondition(flag.liketheinnkeeper, true));
        //    allconversations["firstConversation"].nodes["n2"].options.Add(po);
        //}

        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    sm.storyflags[flag.liketheinnkeeper] = true;
        //}


        if (Input.GetKeyDown(KeyCode.H))
        {
            print(FindNode(DEBUGSTORY).id);
            StartConversation(new Person(), FindNode(DEBUGSTORY));
        }

    }

    public void AddPersonInRange(Person p)
    {
        print("add person");
        pplInConvoRange.Add(p);
        dialHandler.uiMan.ShowDialoguePrompt();
        InteractionManager.OnUseDown += OnUse;
    }

    public void RemovePersonFromRange(Person p)
    {
        print("remove person");
        pplInConvoRange.Remove(p);
        dialHandler.uiMan.HideDialoguePrompt();
        InteractionManager.OnUseDown -= OnUse;
    }

    public void StartConversation(Person p, Node n)
    {
        print("start convo");
        dialHandler.StartDialogue(p, n);
        inConversation = true;
        personSpeakingWith = p;
    }

    public void EndConversation()
    {
        print("end convo");
        dialHandler.EndDialogue();
        inConversation = false;
        personSpeakingWith = null;
    }

    public void OnUse()
    {
        print("on use!");
        if(pplInConvoRange.Count > 0 && !inConversation)
        {
            StartConversation(pplInConvoRange[0], FindNode(pplInConvoRange[0].defaultDialogueID));
        }
        else if (inConversation)
        {
            EndConversation();
        }
    }





    public Node FindNode(string id)
    {
        foreach(NodeGroup ng in allconversations.Values)
        {
            foreach(Node n in ng.nodes.Values)
            {
                if(n.id == id)
                {
                    return n;
                }
            }
        }
        Debug.LogError("Could not find Node requested " + id);
        return null;
    }



}
