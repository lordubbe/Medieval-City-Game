using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DialogueEditorWindow : EditorWindow {

	private static NodeCreator currentGroup;

	private static float editSpacePercentage = 0.8f;
	private static NodeDatabase nodeDatabase;

	// State logic
	private bool draggingEditSize = false;


	[MenuItem("Tools/Dialogue Editor %#d")]
	public static void Init(){
		DialogueEditorWindow window = (DialogueEditorWindow)EditorWindow.GetWindow (typeof(DialogueEditorWindow));
		window.Show ();
	}

	void OnGUI(){
		
		minSize = new Vector2 (200f, 200f);

		Rect space = new Rect(0,0,position.width, position.height);

		HandleCurrentNodeGroup ();

		Rect editSpace = space.WithHeight (space.height);
		EditGUI (editSpace);

		HandleWindowInteractions (space);

		Repaint ();
	}

	void EditGUI(Rect space){
		Event e = Event.current;

		EditorGUI.DrawRect(space, Color.black.WithAlpha(0.5f));
		EditorGUI.LabelField (space, "Node Graph", EditorStyles.boldLabel.WithFontColor(Color.white));

		if (currentGroup != null) {

			if (nodeDatabase == null) {
				Rect objField = new Rect (Vector2.zero, new Vector2 (100f, 18f)).WithCenter(space.center);
				nodeDatabase = (NodeDatabase) EditorGUI.ObjectField (objField, nodeDatabase, typeof(NodeDatabase), false);
				Rect label = objField.WithWidth(200f).WithHorizontalCenter(objField.center.x);
				label.y -= 18f;
				EditorGUI.LabelField (label, "Please select a NodeDatabase object.");
				return;
			}
			
			//Deselect button
			if(Selection.activeGameObject == null || 
				(Selection.activeGameObject != null && 
					Selection.activeGameObject.GetComponent<NodeCreator>() == null
				)
			){
				Color prevColor = GUI.color;
				GUI.color = Color.red.WithAlpha(0.5f);
				Rect unselectButton = space.WithX (space.xMax - 16f).WithHeight (15f);
				if (GUI.Button (unselectButton, "x", EditorStyles.miniButton)) {
					currentGroup = null;
				}
				GUI.color = prevColor;
			}
				
			// Draw nodes
			foreach (UINode n in nodeDatabase.nodes) {
				if (n.groupID == currentGroup.GetInstanceID ()) { 
					n.OnNodeUI ();
				}
			}
//			if (nodeDatabase.nodes.(currentGroup.GetInstanceID())) {

//				foreach (Node n in currentGroup.nodes) {
//					nodeDatabase.nodes [currentGroup.GetInstanceID ()].Find (x => x.node == n).OnGUI (); 
//				}

//				foreach (UINode n in nodeDatabase.nodes[currentGroup.GetInstanceID()]) {
//					if (n.selected) {
//						Rect inspectorSpace = space.WithY (space.height * editSpacePercentage);
//						InspectorGUI (inspectorSpace); //TODO: let the Node draw the inspector?
//					}
//
//					//TODO: Check if the node has a corresponding UINode in the NodeDatabase
//					n.OnNodeUI(); 
//
//				}
//			}

			//Right-click menu
			if (space.IsRightClicked ()) {
				GenericMenu menu = new GenericMenu ();
				menu.AddItem (new GUIContent("Add Node"), false, ()=>AddNode(e.mousePosition)); 
				//				menu.AddSeparator ("");
				//				menu.AddItem (new GUIContent ("Close Node group"), false, () => currentGroup = null);
				menu.ShowAsContext ();
				e.Use ();
			}

		} else {
			EditorGUI.HelpBox (space.WithPadding (15f), "Please select a GameObject with a 'NodeCreator' component", MessageType.Error);

			if (Selection.activeGameObject != null) {
				if (GUI.Button (space.WithY (space.yMax - 15f), "Add 'NodeCreator' component to selected GameObject")) {
					Selection.activeGameObject.AddComponent<NodeCreator> ();
				}
			}
		}
	}

	void InspectorGUI(Rect space){
		GUI.Box (space, "Inspector");
		if (space.IsClicked ()) {
			Event.current.Use ();
		}
	}

	void HandleCurrentNodeGroup(){
		if (Selection.activeGameObject != null) {
			GameObject selection = Selection.activeGameObject;
			NodeCreator c = selection.GetComponent<NodeCreator> ();

			if (c != null && currentGroup != c) {
				SetCurrentGroup (c);
			}
		}
	}

	void HandleWindowInteractions(Rect space){
		Event e = Event.current;

		// EDIT SPACE
		Rect editSpaceHandle = space.WithHeight (6f).WithVerticalCenter (space.height * editSpacePercentage);
		EditorGUIUtility.AddCursorRect (editSpaceHandle, MouseCursor.ResizeVertical);

		if (editSpaceHandle.Contains (e.mousePosition)) {
			EditorGUI.DrawRect(editSpaceHandle, Color.white.WithAlpha(0.25f));

			if (e.type == EventType.mouseDown) {
				draggingEditSize = true;
			}
		}

		if (e.type == EventType.mouseUp) {
			draggingEditSize = false;
		}

		if (draggingEditSize) {
			float newPercentage = e.mousePosition.y / space.height; 
			editSpacePercentage = Mathf.Clamp (newPercentage, 0.05f, 0.95f); 
		}
	}

	void SetCurrentGroup(NodeCreator c){
		currentGroup = c;
		int id = c.GetInstanceID ();

//		if (!nodeDatabase.nodes.ContainsKey (id)) {
//			//First time we selected this NodeCreator!
//			nodeDatabase.nodes.Add(id, new List<UINode> ());
//
//			//Add existing nodes to the database
////			foreach (Node n in c.nodes) {
////				AddUINode (n);
////			}
//		}
	}

	void AddNode(Vector2 position){
		Node n = new Node ();
		currentGroup.nodes.Add (n);
		AddUINode (n, position);
	}

	void AddUINode(Node n, Vector2 position){
		int id = currentGroup.GetInstanceID();
		UINode node = new UINode (n, position);
		node.groupID = id;
		//Add the UINode to the databass
		nodeDatabase.AddNode(node, nodeDatabase);
	}
}
