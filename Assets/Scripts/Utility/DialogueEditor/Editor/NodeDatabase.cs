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

	public static void CreateAsset(){
		NodeDatabase nodeDatabase = ScriptableObject.CreateInstance<NodeDatabase> ();
		nodeDatabase.nodes = new List<UINode> ();
		AssetDatabase.CreateAsset (nodeDatabase, "Assets/Scripts/Utility/DialogueEditor/NodeDatabase.asset");
	}
}
