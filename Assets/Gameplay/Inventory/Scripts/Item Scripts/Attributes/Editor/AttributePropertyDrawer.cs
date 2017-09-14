//using UnityEngine;
//using UnityEditor;
//
//[CustomPropertyDrawer(typeof(Attribute))]
//public class AttributePropertyDrawer : PropertyDrawer {
//
//	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
//	{
////		base.OnGUI (position, property, label);
//		EditorGUI.BeginProperty(position, label, property);
//
////		int indent = EditorGUI.indentLevel;
////		EditorGUI.indentLevel = 0;
//		SerializedProperty t = property.FindPropertyRelative("type");
//		t.intValue = (int) EditorGUI.EnumPopup (position, (AttributeType)t.intValue);
//
////		EditorGUI.indentLevel = indent;
//		
//		EditorGUI.EndProperty ();
//	}
//
//}
