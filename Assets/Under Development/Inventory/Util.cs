using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

public class Util{

	public static void HeaderLabel(Rect space, string label, GUIStyle style, Color color){
		style.normal.textColor = color;
		EditorGUI.LabelField (space, label, style);
	}

	public static void DropShadowHeaderLabel(Rect space, string label, GUIStyle style, Color color){
		//Shadow
		Vector2 prevOff = style.contentOffset;
		style.contentOffset = new Vector2 (2f, 2f);
		style.normal.textColor = Color.black.WithAlpha(0.5f);
		EditorGUI.LabelField (space, label, style);

		//Label
		style.contentOffset = prevOff;
		style.normal.textColor = color;
		EditorGUI.LabelField (space, label, style);

	}

	public static void DrawOutlineRect(Rect r, Color fillColor, Color strokeColor, float strokewidth)
	{
		for (int i = 0; i < strokewidth; i++) {
			r.x -= 1;
			r.y -= 1;
			r.xMax += 2;
			r.yMax += 2;
			Handles.DrawSolidRectangleWithOutline (r, fillColor, strokeColor);
		}
	}

	public static int coordsToIndex(Inventory inv, int x, int y){
		return y * inv.inventoryWidth + x;
	}

	public static Vector2 indexToCoords(Inventory inv, int idx){
		Vector2 coords = Vector2.zero;
		coords.x = idx % inv.inventoryWidth;
		coords.y = (idx - coords.x) / inv.inventoryWidth;

		return coords;
	}

}
#endif

public static class ExtensionMethods{

	public static Color WithAlpha(this Color color, float alpha){
		color.a = alpha;
		return color;
	}

	public static Rect WithHorizontalPadding(this Rect rect, float padding){
		rect.x += padding;
		rect.xMax -= padding * 2;

		return rect;
	}

	public static Vector3 WithMagnitude(this Vector3 v, float magnitude){
		return v.normalized * magnitude;
	}

	public static Vector3 WithY(this Vector3 v, float newY){
		return new Vector3 (v.x, newY, v.z); 
	}

	public static Rect WithInsidePadding(this Rect rect, float padding){
		rect.x += padding;
		rect.xMax -= padding * 2;
		rect.y += padding;
		rect.yMax -= padding * 2;
		
		return rect;
	}
}