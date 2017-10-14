/*#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Elements))]
public class ElementDrawer : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){

		EditorGUI.BeginProperty (position, label, property);
		position = EditorGUI.PrefixLabel (position, GUIUtility.GetControlID (FocusType.Passive), label);

		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		Rect fieldRect = new Rect (position.x, position.y, position.width, position.height);

		EditorGUI.PropertyField (fieldRect, property.FindPropertyRelative ("sin"), GUIContent.none);
		EditorGUI.PropertyField (fieldRect, property.FindPropertyRelative ("change"), GUIContent.none);
		EditorGUI.PropertyField (fieldRect, property.FindPropertyRelative ("force"), GUIContent.none);
		EditorGUI.PropertyField (fieldRect, property.FindPropertyRelative ("secrets"), GUIContent.none);
		EditorGUI.PropertyField (fieldRect, property.FindPropertyRelative ("beauty"), GUIContent.none);

		EditorGUI.indentLevel = indent;

		EditorGUI.EndProperty ();


	}
EndProperty

}
#endif
*/