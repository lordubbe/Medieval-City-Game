using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class StoryLoader : MonoBehaviour {

	public StoryManager sman;

	public List<Flowchart> flowsForStories = new List<Flowchart>();

	public void LoadStories(){

		Story newStory = new Story();
		newStory.storyname = "The First Story";
		newStory.flowcharts.Add(flowsForStories[0]);

		StoryState s1 = new StoryState();
		s1.statename = "the beginning";
		StoryState s2 = new StoryState();
		s2.statename = "the end";

		newStory.states.Add(s1); newStory.states.Add(s2);

		Dictionary<string, bool> newFlags = new Dictionary<string, bool>();
		newFlags.Add("item delivered",false);
		newFlags.Add("merchant angry",false);

		newStory.storyflags = newFlags;

		sman.stories.Add(newStory);
	}
		
}
