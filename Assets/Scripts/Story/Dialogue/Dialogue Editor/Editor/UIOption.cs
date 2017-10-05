using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class UIOption {

	private static float optionPadding = 5f;

	public int parentNodeIdx = -1;
	public Option option;

	public UIOutlet outlet;

	public void OptionGUI(Rect space){
		Event e = Event.current;

		EditorGUI.DrawRect (space, NodeEditorStyles.nodeNormal);
		option.text = EditorGUI.TextArea (
			space.WithX(space.x + 5), 
			option.text, 
			EditorStyles.miniLabel.WithWordWrap()
		);

		outlet.rect = space.WithX (space.xMax + optionPadding).WithWidth(space.height);
		outlet.OnGUI ();

		NodeDatabase nodeDatabase = DialogueEditorWindow.GetStaticDatabase ();

		int idx = nodeDatabase.nodes[parentNodeIdx].options.IndexOf (this);
		int parentOptionCount = nodeDatabase.nodes[parentNodeIdx].options.Count;

		if(space.IsRightClicked()){
			GenericMenu menu = new GenericMenu();

			if (idx > 0) {
				menu.AddItem (new GUIContent ("Move up"), false, ()=> nodeDatabase.nodes[parentNodeIdx].MoveOptionUp(idx));
			}
			if (idx < parentOptionCount - 1) {
				menu.AddItem (new GUIContent ("Move down"), false, ()=> nodeDatabase.nodes[parentNodeIdx].MoveOptionDown(idx));
			}
			menu.AddSeparator ("");
			menu.AddItem(new GUIContent("Remove Option"), false, ()=> nodeDatabase.nodes[parentNodeIdx].RemoveOption(this));
			menu.ShowAsContext ();
			e.Use ();
		}
	}

	public UIOption(Option o, UINode parent){
		NodeDatabase nodeDatabase = DialogueEditorWindow.GetStaticDatabase ();

		this.parentNodeIdx = nodeDatabase.nodes.IndexOf(parent);
		this.option = o;
		outlet = new UIOutlet (this);
	}
}
