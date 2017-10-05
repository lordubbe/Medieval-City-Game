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
            Debug.Log(parent.id);
            Debug.Log(outlet.GetParent().parentid);
			if (parent.id != outlet.GetParent ().parentid) {
                outlet.GetParent().option.linkToNextNode = parent.GetNode().id;
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
