﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public enum flag { startedgame, foundthirdfire, dead }

public enum reputationFactions { University, Commoners, Nobility } 

public struct Bond {
	public Character character;
	public int bondValue;
	public List<string> bondTexts;
};

public class StoryManager : MonoBehaviour {


	public Dictionary<flag, bool> storyflags = new Dictionary<flag, bool>();
	public List<Story> stories = new List<Story>();

	public Dictionary<reputationFactions,int> reputations = new Dictionary<reputationFactions, int>();
	public Dictionary<string,int> qualities = new Dictionary<string,int>();
	public List<Bond> bonds = new List<Bond>();



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

	public void SetBond(Character c, int newBond)
	{
		Bond toChange = bonds.Find(x=>x.character = c);
		toChange.bondValue = newBond;
	}

	public int GetBondValue(Character c){
		return bonds.Find(x=>x.character = c).bondValue;
	}

	public string GetBondText(Character c){
		return bonds.Find(x=>x.character = c).bondTexts[bonds.Find(x=>x.character = c).bondValue];
	}
		
}



