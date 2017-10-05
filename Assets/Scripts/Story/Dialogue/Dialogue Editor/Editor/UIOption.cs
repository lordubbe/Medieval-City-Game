using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class UIOption {

	private static float optionPadding = 5f;

	public string parentid;
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

		int idx = nodeDatabase.nodes.Find(x=>x.id==parentid).options.IndexOf (this); 
		int parentOptionCount = nodeDatabase.nodes.Find(x => x.id == parentid).options.Count; 

		if(space.IsRightClicked()){
			GenericMenu menu = new GenericMenu();

			if (idx > 0) {
				menu.AddItem (new GUIContent ("Move up"), false, ()=> nodeDatabase.nodes.Find(x => x.id == parentid).MoveOptionUp(idx));
			}
			if (idx < parentOptionCount - 1) {
				menu.AddItem (new GUIContent ("Move down"), false, ()=> nodeDatabase.nodes.Find(x => x.id == parentid).MoveOptionDown(idx));
			}
			menu.AddSeparator ("");
			menu.AddItem(new GUIContent("Remove Option"), false, ()=> nodeDatabase.nodes.Find(x => x.id == parentid).RemoveOption(this));
			menu.ShowAsContext ();
			e.Use ();
		}
	}

	public UIOption(Option o, UINode parent){
		NodeDatabase nodeDatabase = DialogueEditorWindow.GetStaticDatabase ();

		this.parentid = parent.id;
        this.option = o;
		outlet = new UIOutlet (this);
	}
}
