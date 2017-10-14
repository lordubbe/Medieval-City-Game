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
		creators.AddRange (CreateConversationsTheDumbWay ());


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

		//TEST TO MAKE SURE ALL OPTIONS HAVE WORKING LINKS
		foreach(NodeGroup ng in allconversations.Values.ToList()){
			foreach (Node n in ng.nodes.Values.ToList()) {
				foreach (Option o in n.options) {

					if(!allconversations.Values.ToList().Exists(x=>x.nodes.Values.ToList().Exists(y=>y.id == o.linkToNextNode))){
						Debug.LogError ("OPTION " + o.text + " in " + n.id + " does not have link to existing node");
					}
				}
			}
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
        pplInConvoRange.Add(p);
        dialHandler.uiMan.ShowDialoguePrompt();
        InteractionManager.OnUseDown += OnUse;
    }

    public void RemovePersonFromRange(Person p)
    {
        pplInConvoRange.Remove(p);
        dialHandler.uiMan.HideDialoguePrompt();
        InteractionManager.OnUseDown -= OnUse;
    }

    public void StartConversation(Person p, Node n)
    {
        dialHandler.StartDialogue(p, n);
        inConversation = true;
        personSpeakingWith = p;
    }

    public void EndConversation()
    {
        dialHandler.EndDialogue();
        inConversation = false;
        personSpeakingWith = null;
    }

    public void OnUse()
    {
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



    public Item SpawnItem(GameObject prefab, Vector3 pos)
    {
        GameObject g = Instantiate(prefab, pos, Quaternion.identity);
        Item i = g.GetComponent<Item>();
        return i;
    }








	public List<NodeCreator> CreateConversationsTheDumbWay()
	{
		List<NodeCreator> nodesToSend = new List<NodeCreator> ();


		NodeCreator nc = new NodeCreator ();
		nc.id = "LovePotion_Intro";

		Node n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_0";
		n.text = "Hey, uh, you there. You.. you’re one of them alchemists, right? From the University.";
		n.options.Add (new Option ("Yeah?", "LovePotion_Intro_1"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_1";
		n.text = "I… uh, have a problem I think you can help me with. You make potions and stuff like that, right?";
		n.options.Add (new Option ("Sometimes.", "LovePotion_Intro_2"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_2";
		n.text = "Right. Can you… make me a love potion?\n\nI can pay. *He holds up a bag of coins. Looks like there’s a lot in there.*";
		n.options.Add (new Option ("*sigh* ...Love potions aren't real.", "LovePotion_Intro_3"));
		n.options.Add (new Option ("Sorry. I don't do that kind of thing.", "LovePotion_Intro_Negative_0"));
		n.options.Add (new Option ("Uhh, I need a little more information than that. Who's it for?", "LovePotion_Intro_5"));
		n.options.Add (new Option ("Sure. What do you need?", "LovePotion_Intro_6"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_5";
		n.text = "A... girl I like.";
		n.options.Add (new Option ("Who is she?", "LovePotion_Intro_7"));
		n.options.Add (new Option ("And why do you want to give her a love potion?", "LovePotion_Intro_8"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_8";
		n.text = "Because I'm in love with her. And I don't know what else to do.";
		n.options.Add (new Option ("Sorry, then. I don't do personal cases.", "LovePotion_Intro_Negative_1"));
		n.options.Add (new Option ("Who is she?", "LovePotion_Intro_7"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_6";
		n.text = "A... love potion.\nTo make a person fall in love with me.";
		n.options.Add (new Option ("Who are we talking about?", "LovePotion_Intro_10"));
		n.options.Add (new Option ("Why?", "LovePotion_Intro_8"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_10";
		n.text = "... A girl.";
		n.options.Add (new Option ("Who?", "LovePotion_Intro_7"));
		nc.nodes.Add (n);

	//Negative
		n = new Node ();
		n.id = "LovePotion_Intro_Negative_0";
		n.text = "No? Hmm... Do you... know of any other alchemists I can ask?";
		n.options.Add (new Option ("No, sorry.", "LovePotion_Intro_Negative_1"));
		n.options.Add (new Option ("*sigh* ...Love potions aren't real.", "LovePotion_Intro_3"));
		nc.nodes.Add (n);

		n = new Node ();
		n.id = "LovePotion_Intro_Negative_1";
		n.text = "Oh... ok.\n\nWell. If you hear of anyone, I'll be here. Please. I could really use the help.\n";
		n.options.Add (new Option ("I'll let you know if I hear of anyone.", "LovePotion_Exit"));
		n.options.Add (new Option ("What's the problem? Maybe I can help in some other way?", "LovePotion_Intro_Negative_2"));
		nc.nodes.Add (n);

		n = new Node ();
		n.id = "LovePotion_Intro_Negative_2";
		n.text = "...You can guess.";
		n.options.Add (new Option ("Is there a special someone?", "LovePotion_Intro_Negative_3"));
		nc.nodes.Add (n);

		n = new Node ();
		n.id = "LovePotion_Intro_Negative_3";
		n.text = "...What do you think? Of course there is. A girl.";
		n.options.Add (new Option ("Who is she?", "LovePotion_Intro_Negative_4"));
		nc.nodes.Add (n);

		n = new Node ();
		n.id = "LovePotion_Intro_Negative_4";
		n.text = "If you won't help, I won't say. What, so you can just ridicule me? No.\nAgree to help first or scram.";
		n.options.Add (new Option ("Who is she?", "LovePotion_Intro_7"));
		n.options.Add (new Option ("Ok. No problem. I'm out of here.", "LovePotion_Exit"));
		nc.nodes.Add (n);

	
		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_7";
		n.text = "Before I tell you, you have to agree to do it.";
		n.options.Add (new Option ("Fine... I'll do it. Who is she?", "LovePotion_Intro_11"));
		n.options.Add (new Option ("No, sorry. Can't do it. I'll let you know if I hear of anyone.", "LovePotion_Exit"));
		n.options.Add (new Option ("*sigh* ...Love potions aren't real.", "LovePotion_Intro_3"));
		nc.nodes.Add (n);


	//Agree
		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_11";
		n.text = "She's... the innkeeper in The Cat's Eye.";
		n.OnEnter.AddListener (() => sm.StartStory (sm.stories.Find (x => x.storyname == "Love Potion")));
		n.options.Add (new Option ("Ok. Have you tried asking her?", "LovePotion_Intro_Talk_0"));
		n.options.Add (new Option ("Ok. I can make a love potion, then.", "LovePotion_Intro_CanMake_0"));
		nc.nodes.Add (n);

        

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_CanMake_0";
		n.text = "Really? You'll do it?";
		n.options.Add (new Option ("*Lie* It's not easy though. Love potions are dangerous work.", "LovePotion_Intro_CanMake_1"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_CanMake_1";
		n.text = "What? Everyone knows what goes into it! You just mix some gold dust and some hair. Look, I already have the hair here. Just take it! *gives you a tuft of his dirty blonde hair.*";
        n.OnEnter.AddListener(() => SpawnItem(Alchemy.Instance.hairGuy, Alchemy.Instance.player.transform.position));
        n.options.Add (new Option ("It's... a little more complicated than that.", "LovePotion_Intro_CanMake_2"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_CanMake_2";
		n.text = "Right. Of course. What do you need?";
		n.options.Add (new Option ("It'll take time.", "LovePotion_Intro_CanMake_3"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_CanMake_3";
		n.text = "Oh. Ok. How long will that take?";
		n.options.Add (new Option ("I don't know. Depends how easily I can get the ingredients. What's the pay?", "LovePotion_Intro_CanMake_4"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_CanMake_4";
		n.text = "*He hesitates. Looks at you a second time.* I'll pay half up front. Half when its done.";
		n.options.Add (new Option ("...Deal. She was in the Cat's Eye, yeah?", "LovePotion_Intro_CanMake_5"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_CanMake_5";
		n.text = "Wait, do you need to see her?";
		n.options.Add (new Option ("Yes", "LovePotion_Intro_CanMake_6"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_CanMake_6";
		n.text = "Why?";
		n.options.Add (new Option ("...I need the hair from her too. It’s not going to work unless it has both of your hairs.", "LovePotion_Intro_CanMake_7"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_CanMake_7";
		n.text = "Oh... Right. Ok. Well, don't tell her what you're doing, right?";
		n.options.Add (new Option ("Of course not.", "LovePotion_Intro_CanMake_8"));
		n.options.Add (new Option ("*Lie* Of course not.", "LovePotion_Intro_CanMake_8"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_CanMake_8";
		n.text = "...All right.\n\nThank you again. This means the world to me. I'll wait here.";
		n.options.Add (new Option ("See ya when I have something.", "LovePotion_Intro_CanMake_9"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_CanMake_9";
		n.OnEnter.AddListener (() => sm.SetQuality ("acceptedlovepotionquest", 1));
		n.text = "[END]";
		n.OnEnter.AddListener (() => EndConversation ());
		nc.nodes.Add (n);

	
	//love potions aren't real & can't

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_3";
		n.text = "Oh, you just not good enough, is that it? ";
		n.options.Add (new Option ("No, they... they don't exist.", "LovePotion_Intro_NotTrue_1"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_NotTrue_1";
		n.text = "But “Mariel and Carvo”? She got the mage to make her a love potion! “The Bewitching Flower?” There it was an alchemist! We all know you can do it. My aunt said she got one made for her first marriage.";
		n.options.Add (new Option ("Yeah, those are… fairytales. Fairy. Tales. Got little to do with actual alchemy. Which is what I know how to do.", "LovePotion_Intro_NotTrue_2"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_NotTrue_2";
		n.text = "You’re just hiding it aren’t you? It’s just difficult? Dangerous? \nOr you don’t trust me. Look, I get that. But listen, I… really like this girl. And I think she would like me too, if she gave it a chance. I just need something to push it over the edge. That’s all I’m asking for. A little help.";
		n.options.Add (new Option ("Even if I could, do you realize... You could just talk to her? Just ask her.", "LovePotion_Intro_NotTrue_3"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_NotTrue_3";
		n.text = "Yeah, right. I didn’t ask you for advice. Can you make the potion or not? I can pay upfront. *He shakes the bag, confirming its contents.*";
		n.options.Add (new Option ("I don't even know how to do it.", "LovePotion_Intro_NotTrue_4"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_NotTrue_4";
		n.text = "Everyone knows what goes into it! You just mix some gold dust and some hair. Look, I already have the hair here. Just take it! *He gives you a tuft of his dirty blonde hair.*";
		n.options.Add (new Option ("Right. Ok. That the pay? Fine. I'll try to do it.", "LovePotion_Intro_NotTrue_5"));
		n.options.Add (new Option ("No, sorry, I'm good. I can't make a potion for you.", "LovePotion_Intro_Cant_0"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_NotTrue_5";
		n.text = "Really? You will? Oh, thank you. Thank you.";
		n.options.Add (new Option ("Try, I said. I first need to figure it out.", "LovePotion_Intro_CanMake_3"));
		nc.nodes.Add (n);


		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_Cant_0";
		n.text = "You said you'd try. I told you who she is.";
		n.options.Add (new Option ("Right. I promise I won't do anything.", "LovePotion_Intro_Cant_1"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_Cant_1";
		n.text = "... I was told you alchemists were filthy liars. Should've never trusted you.";
		n.options.Add (new Option ("Fine. Whatever. Raeth la'en.", "LovePotion_Intro_Cant_2"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_Cant_2";
		n.text = "Fuck off.";
		n.options.Add (new Option ("Fine.", "LovePotion_Intro_Cant_3"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_Cant_3";
		n.text = "[END]";
		n.OnEnter.AddListener (() => EndConversation ());
		nc.nodes.Add (n);


		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_Talk_0";
		n.text = "..I can't. She... I'll mess it up. Look like a fool.";
		n.options.Add (new Option ("And you think a love potion will help?", "LovePotion_Intro_Talk_1"));
		n.options.Add (new Option ("You might do. But if she likes you back, it won't matter.", "LovePotion_Intro_Talk_2"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_Talk_1";
		n.text = "Well... yes.\nIf I can only get her to see me, notice me. Maybe I can talk to her again.";
		n.options.Add (new Option ("The best way to get her to notice you is to talk to her.", "LovePotion_Intro_Talk_3"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_Talk_2";
		n.text = "...No. I can't. I need a love potion.\n\nIf I can only get her to see me... If she can pause and listen, I might be able to talk to her.";
		n.options.Add (new Option ("Just talk to her. It'll work.", "LovePotion_Intro_Talk_3"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_Talk_3";
		n.text = "But I can't.\nI... I'm too nervous. Around her, I... lose my mouth. I can't talk.";
		n.options.Add (new Option ("It's hard. I'm not denying you that. But I know any kind of love potion won't help.", "LovePotion_Intro_Talk_4"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_Talk_4";
		n.text = "Why? How can you be sure?";
		n.options.Add (new Option ("*sigh* ...Because love potions aren't real.", "LovePotion_Intro_3"));
		n.options.Add (new Option ("*Lie*...I've seen it before. A friend of mine tried to create a love potion. It only worked while the potion was in effect. The day after, the other person was furious.", "LovePotion_Intro_Talk_5"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_Talk_5";
		n.text = "Really?\nBut... aren't love potions supposed to harbour love? Create the spark that otherwheres never existed? Conjoin the two flowers in the wind and make them dance?";
		n.options.Add (new Option ("You've been reading too many fairy tales.", "LovePotion_Intro_Talk_6"));
		nc.nodes.Add (n);

		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Intro_Talk_6";
		n.text = "I...\nI just need a solution. I don't care if it might not work. I'm willing to try it. I don't have any other choice.";
		n.options.Add (new Option ("*sigh* ...OK, fine. Love potions aren't real.", "LovePotion_Intro_3"));
		n.options.Add (new Option ("Fine. I'll make you a love potion then.", "LovePotion_Intro_CanMake_0"));
		n.options.Add (new Option ("No, sorry, I'm good. I can't make a potion for you.", "LovePotion_Intro_Cant_0"));
		nc.nodes.Add (n);


		n = new Node ();
		n.characterSpeaking = sm.people [0];
		n.id = "LovePotion_Exit";
		n.text = "[END]";
		n.OnEnter.AddListener (() => EndConversation ());
		nc.nodes.Add (n);


        nodesToSend.Add(nc);



        nc = new NodeCreator();
        nc.id = "blacksmith_gold";

        n = new Node();
        n.characterSpeaking = sm.people[1];
        n.id = "blacksmith_gold_0";
        n.text = "Heya. How are you? You here about the ship?";
        n.options.Add(new Option("No, sorry. Something came up. Do you happen to have some gold dust?", "blacksmith_gold_1"));
        nc.nodes.Add(n);

        n = new Node();
        n.characterSpeaking = sm.people[1];
        n.id = "blacksmith_gold_1";
        n.text = "Gold dust? Yeah, sure. Some of the nobles wants their stuff gilded. Don't really see how it matters on a horseshoe, but them about that.";
        n.options.Add(new Option("Can I have some?", "blacksmith_gold_2"));
        nc.nodes.Add(n);

        n = new Node();
        n.characterSpeaking = sm.people[1];
        n.id = "blacksmith_gold_2";
        n.text = "Gold is quite expensive...";
        n.options.Add(new Option("I can pay.", "blacksmith_gold_3"));
        nc.nodes.Add(n);

        n = new Node();
        n.characterSpeaking = sm.people[1];
        n.id = "blacksmith_gold_3";
        n.text = "All right then. Your loss.";
        n.options.Add(new Option("Thank you.", "blacksmith_gold_4"));
        nc.nodes.Add(n);

        n = new Node();
        n.characterSpeaking = sm.people[1];
        n.id = "blacksmith_gold_4";
        n.text = "Care to enlighten me why you need some gold dust this badly?";
        n.OnEnter.AddListener(() => SpawnItem(Alchemy.Instance.goldDust, Alchemy.Instance.player.transform.position));
        n.options.Add(new Option("No, sorry. I'll... explain later.", "blacksmith_gold_5"));
        n.options.Add(new Option("It's... for a love potion. Not a working one. Just need to make it seem real.", "blacksmith_gold_5"));
        nc.nodes.Add(n);

        n = new Node();
        n.characterSpeaking = sm.people[1];
        n.id = "blacksmith_gold_5";
        n.text = "All right... See you then.";
        n.options.Add(new Option("Raeth la'en [May you be good].", "blacksmith_gold_6"));
        nc.nodes.Add(n);

        n = new Node();
        n.characterSpeaking = sm.people[1];
        n.id = "blacksmith_gold_6";
        n.text = "[END]";
        n.OnEnter.AddListener(() => EndConversation());

        nc.nodes.Add(n);



        nodesToSend.Add(nc);

        return nodesToSend;

	}




}






