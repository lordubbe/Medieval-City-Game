using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class UIInlet : UIConnection {

	private UINode parent;

	public override void AddConnection (UIConnection c)
	{
		if (c is UIOutlet) {
            UIOutlet outlet = c as UIOutlet;

			NodeDatabase db = DialogueEditorWindow.GetStaticDatabase ();
			if (parent != db.nodes [outlet.GetParent ().parentNodeIdx]) {
                outlet.GetParent().option.linkToNextNode = parent.GetNode();
				base.AddConnection (c);
				outlet.AddConnection (this);
			}
		}
	}

	public override void RemoveConnection ()
	{
		base.RemoveConnection ();
		NodeDatabase db = DialogueEditorWindow.GetStaticDatabase ();

		//This is hacky as fuck, but it is nice for the workflow
		UIOutlet connection = parent.options.Where(o => db.nodes[o.outlet.connectedNodeIdx].inlet == this).First().outlet;
		connection.RemoveConnection ();
	}

	public UIInlet(UINode parent){
		this.parent = parent;
	}

	public UINode GetParent(){
		return parent;
	}
}
