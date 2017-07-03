using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ItemCreatorWindow : EditorWindow {

	[MenuItem ("Tools/Item Creator")]
	public static void ShowWindow(){
		EditorWindow.GetWindow (typeof(ItemCreatorWindow));
	}
}
