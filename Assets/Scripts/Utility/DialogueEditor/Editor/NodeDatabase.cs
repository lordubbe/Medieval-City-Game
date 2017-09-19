using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public delegate void NodeSelectionEvent(UINode node);
[CreateAssetMenu(), System.Serializable]
public class NodeDatabase : ScriptableObject{ //Should be a ScriptableObject or something that saves

	public List<UINode> nodes = new List<UINode>();

	public static NodeSelectionEvent OnNodeSelected;
	public static NodeSelectionEvent OnNodeDeselected;

	public void AddNode(UINode n, NodeDatabase d){
		nodes.Add (n);
		EditorUtility.SetDirty (this);
	}
}
