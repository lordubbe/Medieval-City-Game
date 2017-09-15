using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationManager : MonoBehaviour {

    [SerializeField] List<NodeCreator> creators = new List<NodeCreator>();
    public Dictionary<string, NodeGroup> allconversations = new Dictionary<string, NodeGroup>();
    public StoryManager sm;
    public DialogueHandler dialHandler;

    public List<Person> pplInConvoRange = new List<Person>();

    public void Start()
    {
        foreach(NodeCreator nc in creators)
        {
            NodeGroup nn = new NodeGroup();
            nn.id = nc.nodes[0].id;
            foreach(Node n in nc.nodes)
            {
                nn.nodes.Add(n.id, n);
            }
            allconversations.Add(nn.id, nn);
        }
        print(allconversations.Count+" "+creators.Count);

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
