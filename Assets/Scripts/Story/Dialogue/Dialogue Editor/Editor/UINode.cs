using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class UINode : ResizableRect {

	public int groupID = -1;

	public static Vector2 defaultNodeDimensions = new Vector2(100f, 80f);

	private Node node;
	public string id;

	public UIInlet inlet;
	public List<UIOption> options = new List<UIOption>();

	private static float optionPadding = 5f;

	public UINode(Node n, Vector2 position){
        id = n.id;
		rect = new Rect (Vector2.zero, defaultNodeDimensions).WithCenter(position);
		inlet = new UIInlet (this); 
	}

	public void OnNodeUI(DialogueEditorWindow root){
		base.OnGUI ();

        node = DialogueEditorWindow.currentGroup.nodes.Find(x => x.id == id);

		Event e = Event.current;

		//Selection
		if (rect.IsHovered ()) {
			EditorGUI.DrawRect (rect, NodeEditorStyles.nodeHovered);
		} else {
			EditorGUI.DrawRect (rect, NodeEditorStyles.nodeNormal);
		}

		if (selected) {
			Util.DrawOutlineRect (rect, Color.white.WithAlpha (0f), GUI.skin.settings.selectionColor, 2f);
		}

		if (rect.IsRightClicked ()) {
			GenericMenu gm = new GenericMenu ();
			gm.AddItem (new GUIContent ("Remove Node"), false, () => root.RemoveNode(this));
			gm.ShowAsContext ();
			e.Use ();
		}

		//Option inlet
		inlet.rect = rect.WithX(rect.x - (20 + optionPadding)).WithWidth(20).WithHeight(20);
		inlet.OnGUI ();

		//Node data
		Rect charLabel = rect.WithY (rect.yMin - 15f).WithHeight (15f);
		node.characterSpeaking.name = EditorGUI.TextField (charLabel, 
			node.characterSpeaking.name == "" ? "No Person" : node.characterSpeaking.name, 
			EditorStyles.boldLabel
		);

		Rect textArea = rect.WithPadding(10f);
		GUI.Box (textArea, "");
//		GUI.BeginScrollView(textarea
		node.text = EditorGUI.TextArea (textArea, node.text, EditorStyles.wordWrappedLabel);

		//Options
		Rect optionRect = rect;
		optionRect.height = 20f;
		optionRect.y = rect.yMax + optionPadding;
		foreach (UIOption o in options) {
			//Draw options
			o.OptionGUI(optionRect);
			optionRect.y = optionRect.yMax + optionPadding;
		}

		if (Util.FlatButton (
			optionRect.WithX (optionRect.x + (optionRect.width - optionRect.height)),
			   "+",
			   NodeEditorStyles.nodeNormal,
			   EditorStyles.whiteBoldLabel.WithCenteredAlignment ()
		   )) {
			AddOption ();
		}
	}

	public void MoveOptionUp(int idx){
		Option temp = node.options [idx - 1];
		node.options [idx - 1] = node.options[idx];
		node.options[idx] = temp;

		//swap connectedNodeIdx on UIOptions
	//	options[idx].parentNodeIdx -= 1;
	//	options [idx - 1].parentNodeIdx += 1;

		//swap them
		UIOption temp2 = options [idx - 1];
		options [idx - 1] = options[idx];
		options [idx] = temp2;
	}

	public void MoveOptionDown(int idx){
		Option temp = node.options [idx + 1];
		node.options [idx + 1] = node.options[idx];
		node.options[idx] = temp;

		//swap connectedNodeIdx on UIOptions
	//	options [idx].parentNodeIdx += 1;
	//	options [idx + 1].parentNodeIdx -= 1;

		//swap them
		UIOption temp2 = options [idx + 1];
		options [idx + 1] = options[idx];
		options[idx] = temp2;
	}

	public void RemoveOption(UIOption o){
        //TODO: How to handle the options connected node? Remove node or let it stay?
        Debug.Log(node.options.Count);
		node.options.Remove (o.option);
        Debug.Log(node.options.Count);
        options.Remove (o);
	}

	void AddOption(){
		Option o = new Option ();
		node.options.Add (o);
		options.Add (new UIOption (o, this));
	}

	public Node GetNode(){
		return DialogueEditorWindow.currentGroup.nodes.Find(x=>x.id == id);
	}
    
}
