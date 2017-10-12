using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

public enum flag { startedgame, foundthirdfire, dead }
public enum reputationFactions { University, Commoners, Nobility } 


public struct Bond {
    public Person p;
	public int bondValue;
	public List<string> bondTexts;
};

public class StoryManager : SerializedMonoBehaviour {

	public StoryLoader sloader;
	public Dictionary<flag, bool> storyflags = new Dictionary<flag, bool>();
	[HideInInspector] public List<Story> stories = new List<Story>();

	public Dictionary<reputationFactions,int> reputations = new Dictionary<reputationFactions, int>();
	public List<Quality> allQualities = new List<Quality>();
    public List<Quality> playerQualities = new List<Quality>();
    public List<Bond> bonds = new List<Bond>();
    public List<Person> people = new List<Person>();

    public ConversationManager convos;
    public UIManager uiman;

	void Awake(){
		stories = GetComponentsInChildren<Story>().ToList();
		allQualities = GetComponentsInChildren<Quality> ().ToList ();
		playerQualities.AddRange(allQualities); //well. that's also dumb.

        foreach(Person p in people)
        {
            p.InitPerson(this);
        }
	}

    void Start()
    {
  //      stories[0].StartStory();
    }

	void Update(){
		if(Input.GetKey(KeyCode.P)){
			stories[0].StartStory();
		}

        if (Input.GetKeyDown(KeyCode.G))
        {
            playerQualities.Add(allQualities.Find(x => x.id == "madepotion"));
            playerQualities[0].SetValue(1);
        }


	}

    public void TESTTHIS()
    {
        print("hELLOOOO");
    }

	public void StartStory(Story s)
	{
        Story sref = stories.Find(x => x == s);
        sref.isActive = true;
        Debug.Log("Story " + sref.storyname + " started");
        sref.ChangeState(sref.startState); //potentially change!
        if(uiman != null)
        {
            uiman.DisplayUpdate(UpdateType.StoryStart, sref.storyname);
        }
	}

	public void UpdateStory(Story s, StoryState newState)
	{
		s.ChangeState(newState);

    }

	public void SetFlag(flag f, bool b)
	{
		storyflags[f] = b;
	}

	public bool GetFlag(flag f)
	{
		return storyflags[f];
	}

	public void SetFlag(Story s, string ss, bool b)
	{
		s.storyflags[ss] = b;
	}

	public bool GetFlag(Story s, string ss)
	{
		return s.storyflags[ss];
	}


	public void SetReputation(reputationFactions r, int val)
	{
		reputations[r] = val;
	}

	public int GetReputation(reputationFactions r)
	{
		return reputations[r];
	}

	public Quality GetQuality(string q)
	{
		return playerQualities.Find(x=>x.id==q);
	}

	public void SetQuality(string q, Quality val)
	{
		playerQualities[playerQualities.FindIndex(x=>x.id==q)] = val;
	}

	public void SetQuality(string q, int val)
	{
		playerQualities[playerQualities.FindIndex(x=>x.id==q)].SetValue(val);
	}

	public void SetBond(Person p, int newBond)
	{
		Bond toChange = bonds.Find(x=>x.p == p);
		toChange.bondValue = newBond;
	}

	public int GetBondValue(Person p)
    {
		return bonds.Find(x=>x.p == p).bondValue;
	}

	public string GetBondText(Person p)
    {
		return bonds.Find(x=>x.p == p).bondTexts[bonds.Find(x=>x.p == p).bondValue];
	}
		
}



