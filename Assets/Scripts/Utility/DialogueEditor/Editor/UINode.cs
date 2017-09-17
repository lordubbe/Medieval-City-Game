using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class UINode : ResizableRect {

	public int groupID = -1;

	public static Vector2 defaultNodeDimensions = new Vector2(100f, 80f);
	public Node node;
	public Rect optionInlet;

	private static float optionPadding = 5f;

	private bool addingNode;
	private Option addingNodeFromOption;


	public UINode(Node n, Vector2 position){
		node = n;
		rect = new Rect (Vector2.zero, defaultNodeDimensions).WithCenter(position);
	}

	public void OnNodeUI(){
		base.OnGUI ();

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

		//Option inlet
		optionInlet = rect.WithX(rect.x - (20 + optionPadding)).WithWidth(20).WithHeight(20);

		if (Util.FlatButton (optionInlet, ">", NodeEditorStyles.nodeNormal, EditorStyles.whiteBoldLabel.WithCenteredAlignment ())) {
			//Maybe clear connection?
		}

		//Node data
		Rect charLabel = rect.WithY (rect.yMin - 15f).WithHeight (15f);
		EditorGUI.TextField (charLabel, 
			node.characterSpeaking == null ? "No Person" : node.characterSpeaking.name, 
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
		foreach (Option o in node.options) {
			//Draw options
			OptionUI(optionRect, o);
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

	void OptionUI(Rect space, Option o){
		Event e = Event.current;

		EditorGUI.DrawRect (space, NodeEditorStyles.nodeNormal);
		o.text = EditorGUI.TextArea (
			space.WithX(space.x + 5), 
			o.text, 
			EditorStyles.miniLabel.WithWordWrap()
		);

		Rect addConnectionButton = space.WithX (space.xMax + optionPadding).WithWidth(space.height);

		if (Util.FlatButton (addConnectionButton, ">", NodeEditorStyles.nodeNormal, EditorStyles.whiteBoldLabel.WithCenteredAlignment())) {
			Debug.Log ("Adding connection...");
			addingNode = true;
			addingNodeFromOption = o;
		}

		if (addingNode && addingNodeFromOption == o) {
			Vector2 c = addConnectionButton.center;
			Handles.DrawLine ( 
				new Vector3 (c.x, c.y), 
				new Vector2 (e.mousePosition.x, e.mousePosition.y)
			);
			
			if (e.type == EventType.mouseUp) {
				//TODO: Add new node with connection to this option
				addingNode = false;
				addingNodeFromOption = null;
			}
		}

		if(space.IsRightClicked()){
			GenericMenu menu = new GenericMenu();

			int idx = node.options.IndexOf (o);

			if (idx > 0) {
				menu.AddItem (new GUIContent ("Move up"), false, ()=>MoveOptionUp(idx));
			}
			if (idx < node.options.Count - 1) {
				menu.AddItem (new GUIContent ("Move down"), false, ()=>MoveOptionDown(idx));
			}
			menu.AddSeparator ("");
			menu.AddItem(new GUIContent("Remove Option"), false, ()=>RemoveOption(o));
			menu.ShowAsContext ();
			e.Use ();
		}

	}

	void MoveOptionUp(int idx){
		Option temp = node.options [idx - 1];
		node.options [idx - 1] = node.options[idx];
		node.options[idx] = temp;
	}

	void MoveOptionDown(int idx){
		Option temp = node.options [idx + 1];
		node.options [idx + 1] = node.options[idx];
		node.options[idx] = temp;
	}

	void RemoveOption(Option o){
		//TODO: How to handle the options connected node? Remove node or let it stay?
		node.options.Remove (o);
	}

	void AddOption(){
		node.options.Add (new Option ());
	}
}
