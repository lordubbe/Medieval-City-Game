using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryLoader : MonoBehaviour {

	public StoryManager sman;
    
	public List<Story> LoadStories(){

        List<Story> storiesToSend = new List<Story>();

        Dictionary<string, bool> flags = new Dictionary<string, bool>();
        flags.Add("madepotion", false); flags.Add("givenpotion", false); flags.Add("talkedtogirl", false); flags.Add("potionAccurate", false);
        Story lovepotion = new Story(sman,"Love Potion",flags);

        Quality[] quas = new Quality[] { new Quality("madepotion", "", 1) };
        StoryState stateone = new StoryState(lovepotion, "begin", "[Name] wants me to make a love potion. Not sure he really knows what he's asking for. Or how impossible it it. Or how wrong it is. Let's see if I can resolve this in a way that makes sense", quas);

        Option o = new Option("Hey, you know someone called [Name]?", sman.convos.FindNode("lovePotionInnkeeperIntro").id);
        stateone.optionsToAdd.Add("innKeeperHello", o);



        return storiesToSend;

	}
		
}
