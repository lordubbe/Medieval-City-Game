using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class DialogueEditorWindow : EditorWindow {

	public static NodeCreator currentGroup;
	private SerializedObject serializedCurrentGroup;

	private static float editSpacePercentage = 0.8f;
	private static NodeDatabase nodeDatabase;

	// State logic
	private bool draggingEditSize = false;

	private Vector2 scrollAmount = Vector2.zero;

    public int idIterator = 0;

	[MenuItem("Tools/Dialogue Editor %#d")]
	public static void Init(){
		DialogueEditorWindow window = (DialogueEditorWindow)EditorWindow.GetWindow (typeof(DialogueEditorWindow));
		window.Show ();
	}

	void OnGUI(){
		
		minSize = new Vector2 (200f, 200f);

		Rect space = new Rect(0,0,position.width, position.height);

		HandleCurrentNodeGroup ();

		InitializeNodeDatabase (space);

		Rect inspectorSpace = space.WithY (space.height * editSpacePercentage).WithHeight (space.height - (space.height * editSpacePercentage));
		if (currentGroup != null && serializedCurrentGroup != null) {
			InspectorGUI (inspectorSpace);
		}

		Rect editSpace = space.WithHeight (space.height * editSpacePercentage);
		EditGUI (editSpace);


		HandleWindowInteractions (space);
		
		Repaint ();
	}

	void EditGUI(Rect space){
		Event e = Event.current;

		EditorGUI.DrawRect(space, Color.black.WithAlpha(0.5f));
		EditorGUI.LabelField (space, "Node Graph", EditorStyles.boldLabel.WithFontColor(Color.white));

		if (currentGroup != null) {
			
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

            //Check for non-registered entry point for this group
            //if(nodeDatabase.entryPoints.Where())

            //Check for existing non-registered nodes (first time)
            List<Node> unregisteredNodes =
                currentGroup.nodes.Where(
                    node => !nodeDatabase.nodes.Any(x => x.GetNode() == node)
                ).ToList();
           
            foreach(Node n in unregisteredNodes){
                //Somehow do better auto-arranging? 
                AddUINode(n, space.center);
            }


			// Draw nodes
			foreach (UINode n in nodeDatabase.nodes) {
				if (n.groupID == currentGroup.GetInstanceID ()) { 
					n.OnNodeUI (this);
				}
			}

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

        bool isNodeSelected = false;

        List<UINode> nodesInGroup = nodeDatabase.nodes.FindAll(x => x.groupID == currentGroup.GetInstanceID());

        if (nodesInGroup.Count > 0)
        {
            for (int i = 0; i < nodesInGroup.Count; i++)
            {
                if (nodesInGroup[i].selected)
                {
                    isNodeSelected = true;
                    serializedCurrentGroup.Update();
                    SerializedProperty nodes = serializedCurrentGroup.FindProperty("nodes");
                    SerializedProperty unityEvent = nodes.GetArrayElementAtIndex(i).FindPropertyRelative("OnEnter");

                    //Draw
                    GUILayout.BeginArea(space.WithY(space.yMin + 15f).WithPadding(5f));
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField("Facial picture selection here?");
                    

                    //UnityEvent
                    EditorGUILayout.PropertyField(unityEvent);
                    serializedCurrentGroup.ApplyModifiedProperties();
                    //				EditorUtility.SetDirty (currentGroup);

                    EditorGUILayout.EndHorizontal();
                    GUILayout.EndArea();

                    break;
                }
            }
            Event e = Event.current;
            if (space.Contains(e.mousePosition) && e.type == EventType.mouseUp)
            {//space.IsClicked ()) {
                Event.current.Use();
                draggingEditSize = false;
            }
        }
        if (!isNodeSelected)
        {
            GUILayout.BeginArea(space.WithY(space.yMin + 15f).WithPadding(5f));
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Insert ID for NodeGroup here.");
            currentGroup.id = EditorGUILayout.TextField(currentGroup.id == "" ? "Insert thing here" : currentGroup.id);
            EditorGUILayout.EndHorizontal();
            GUILayout.EndArea();
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
		serializedCurrentGroup = new SerializedObject(currentGroup);
        idIterator = nodeDatabase.nodes.Count(x => x.groupID == currentGroup.GetInstanceID());
    }

    void InitializeNodeDatabase(Rect space){
		if (nodeDatabase == null) {
			//Auto-load database
			string[] databases = AssetDatabase.FindAssets ("NodeDatabase");

			if (databases.Length == 0) {
				//No existing database could be found. 

				//Create it automagically?

				//For now, just allow drag n' dropping it
				Rect objField = new Rect (Vector2.zero, new Vector2 (100f, 18f)).WithCenter (space.center);
				nodeDatabase = (NodeDatabase)EditorGUI.ObjectField (objField, nodeDatabase, typeof(NodeDatabase), false);
				Rect label = objField.WithWidth (200f).WithHorizontalCenter (objField.center.x);
				label.y -= 18f;
				EditorGUI.LabelField (label, "Please select a NodeDatabase object.");
			} else { 
				foreach (string s in databases) {
					string path = AssetDatabase.GUIDToAssetPath (s);
					if (path.EndsWith (".asset")) {
						NodeDatabase database = AssetDatabase.LoadAssetAtPath<NodeDatabase> (path);
						nodeDatabase = database;
					}
				}
			}
			return;
		}
	}

	void AddNode(Vector2 position){
		Node n = new Node ();
		n.characterSpeaking = new Person ();
        n.id = currentGroup.id + "_" + idIterator;
        idIterator++;

		Debug.Log ("adding node");
        currentGroup.nodes.Add (n);
		AddUINode (n, position);
	}

	public void RemoveNode(UINode node){
		currentGroup.nodes.Remove (node.GetNode());
		nodeDatabase.nodes.Remove (node);
	}

	void AddUINode(Node n, Vector2 position){
		int id = currentGroup.GetInstanceID();
		UINode node = new UINode (n, position);
		node.groupID = id;
		//Add the UINode to the databass
		nodeDatabase.AddNode(node, nodeDatabase);
	}

	public static NodeDatabase GetStaticDatabase(){
		return nodeDatabase;
	}
    
    public static UINode GetUINode(Node n)
    {
        return nodeDatabase.nodes.Find(x => x.id == n.id);
    }

    public static Node GetNode(UINode n)
    {
        return currentGroup.nodes.Find(x => x.id == n.id);
    }

}
