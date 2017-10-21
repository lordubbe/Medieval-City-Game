using UnityEngine;
using UnityEditor;

[System.Serializable]
public class UIConnection : InteractableRect {
	
	private bool addingConnection;

	protected override void OnMouseUpOverRect (Event e)
	{
        Debug.Log("up mouse over react!");
		base.OnMouseUpOverRect (e);
		if (NodeEditorEvents.activeConnection != null) {
			AddConnection (NodeEditorEvents.activeConnection);
//				NodeEditorEvents.activeConnection.connection = this;
//				connection = NodeEditorEvents.activeConnection;
//				occupied = true; 
		}
	}

	public override void OnGUI ()
	{
		base.OnGUI ();

		Event e = Event.current;
        EditorUtil.FlatButton (rect, ">", NodeEditorStyles.nodeNormal, EditorStyles.whiteBoldLabel.WithCenteredAlignment ());

		if (addingConnection) {
			Handles.DrawLine ( 
				rect.center,
				e.mousePosition
			);
		}

		//Draw connection (if any)
//		if (connection != null) {
//			Handles.DrawLine ( 
//				rect.center,
//				connection.rect.center
//			);
//		}
	}

	protected override void OnInteract (Event e)
	{
		base.OnInteract (e);
        
		NodeEditorEvents.activeConnection = this;
		addingConnection = true;
	}

	protected override void OnStopInteract (Event e)
	{
		base.OnStopInteract (e);

		addingConnection = false;
	}

	protected override void OnRightClick (Event e)
	{
		base.OnRightClick (e);
        
		GenericMenu gm = new GenericMenu ();
		gm.AddItem (new GUIContent ("Remove Connection"), false, 
			() => {
				RemoveConnection(); 
			}
		);
		gm.ShowAsContext ();
		e.Use ();
	}

	public virtual void RemoveConnection(){
//		connection = null;
	}

	public virtual void AddConnection(UIConnection c){
//		connection = c;
	}
}
