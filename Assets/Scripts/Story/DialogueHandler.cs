using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script handles taking a dialogue node group etc. from the conversation manager and throws it on screen.
/// </summary>
public class DialogueHandler : MonoBehaviour {

    public StoryManager sm;
    public GameObject optionPrefab;
    public List<GameObject> displayedOptions = new List<GameObject>();

    public Canvas dialogueCanvas;

    public Node node;

    public GridLayoutGroup optionList;
    public Text text;
    public Text charText;


	// Use this for initialization
	void Start () {
        
	}
	
	

    public void StartDialogue(Person p, Node n)
    {
        DisplayNode(n);
    }

    public void ClearDialogue()
    {
        text.text = "";
        charText.text = "";
        for (int i = displayedOptions.Count-1; i >= 0; i--)
        {
            GameObject gg = displayedOptions[i];
            displayedOptions.Remove(displayedOptions[i]);
            Destroy(gg);
        }
    }

    public void DisplayNode(string id)
    {
        DisplayNode(sm.convos.FindNode(id));
    }

    public void DisplayNode(Node n)
    {
        ClearDialogue();

        text.text = n.text;
        if(n.characterSpeaking != null)
        {
            charText.text = n.characterSpeaking.name;
        }
        foreach(Option o in n.options)
        {
            /// IF option flags fulfill Check
            if (!CheckAllConditions(o))
            {
                continue;
            }

            GameObject g = Instantiate(optionPrefab, optionList.transform) as GameObject;
            displayedOptions.Add(g);
            Button b = g.GetComponent<Button>();
            b.onClick.AddListener(() => DisplayNode(o.linkToNextNode));
            b.GetComponentInChildren<Text>().text = o.text;
        }

    }

    bool CheckAllConditions(Option o)
    {
        foreach (FlagCondition fc in o.conditions)
        {
            if (!CheckFlag(fc.f, fc.b))
            {
                return false;
            }
        }
        return true;
    }

    bool CheckFlag(flag f, bool b)
    {
        print(sm.storyflags.Count);
        print(f + " " + b);
        
        if(sm.storyflags[f] == b)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
