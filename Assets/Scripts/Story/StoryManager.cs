﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum flag { startedgame, foundthirdfire, dead, liketheinnkeeper }
public enum reputationFactions { University, Commoners, Nobility } 


public struct Bond {
    public Person p;
	public int bondValue;
	public List<string> bondTexts;
};

public class StoryManager : MonoBehaviour {

	public StoryLoader sloader;
	public Dictionary<flag, bool> storyflags = new Dictionary<flag, bool>();
	public List<Story> stories = new List<Story>();

	public Dictionary<reputationFactions,int> reputations = new Dictionary<reputationFactions, int>();
	public Dictionary<string,int> qualities = new Dictionary<string,int>();
	public List<Bond> bonds = new List<Bond>();

    public ConversationManager convos;

	void Start(){

		//sloader.LoadStories();

		stories = GetComponentsInChildren<Story>().ToList();

        ///// DUMMY
     //   flag f = flag.liketheinnkeeper;
        storyflags.Add(flag.liketheinnkeeper, false);
        //

	}
	void Update(){
		if(Input.GetKey(KeyCode.P)){
			stories[0].StartStory(true);
		}
	}


	public void StartStory(Story s)
	{
		s.isActive = true;
		s.ChangeState(s.states[0]); //potentially change!
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

	public int GetQuality(string q)
	{
		return qualities[q];
	}

	public void SetQuality(string q, int val)
	{
		qualities[q] = val;
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



