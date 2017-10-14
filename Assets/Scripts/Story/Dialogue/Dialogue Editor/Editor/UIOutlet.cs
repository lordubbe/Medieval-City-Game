using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class UIOutlet : UIConnection {

	private UIOption parent;
	public int connectedNodeIdx = -1;

	public override void AddConnection (UIConnection c)
	{
		if (c is UIInlet) {
			UIInlet inlet = c as UIInlet;

			//Don't connect a Node to itself
			if (inlet.GetParent() != DialogueEditorWindow.GetStaticDatabase().nodes.Find(x=>x.id == parent.parentid)) {
				base.AddConnection (c);
				connectedNodeIdx = DialogueEditorWindow.GetStaticDatabase().nodes.IndexOf(inlet.GetParent()); 
			}
		}
	}

	public override void OnGUI ()
	{
		//Draw connection (if any)
		if (connectedNodeIdx != -1) {
			Handles.DrawLine ( 
				rect.center,
				DialogueEditorWindow.GetStaticDatabase().nodes[connectedNodeIdx].inlet.rect.center
			);
		}

		base.OnGUI ();
	}

	public UIOutlet(UIOption parent){
		this.parent = parent;
	}

	public void SetConnection(UINode n){
		connectedNodeIdx = DialogueEditorWindow.GetStaticDatabase().nodes.IndexOf(n);
	}

	public UIOption GetParent(){
		return parent;
	}
}
