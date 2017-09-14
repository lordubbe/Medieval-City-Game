using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class Util{
#if UNITY_EDITOR
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
#endif


    public static int coordsToIndex(Inventory inv, int x, int y)
    {
        return y * inv.inventoryWidth + x;
    }

    public static Vector2 indexToCoords(Inventory inv, int idx)
    {
        Vector2 coords = Vector2.zero;
        coords.x = idx % inv.inventoryWidth;
        coords.y = (idx - coords.x) / inv.inventoryWidth;

        return coords;
    }

    public static Vector3 PosTo2D(Vector3 pos, Canvas c, bool world)
    {
        
        if (world)
        {
            Vector3 pos3d;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(c.transform as RectTransform, pos, c.worldCamera, out pos3d);
            //return c.transform.TransformPoint(pos3d);
            return Camera.main.ScreenToWorldPoint(pos3d);
        }
        else
        {
            Vector2 pos2d;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(c.transform as RectTransform, pos, c.worldCamera, out pos2d);
            return c.transform.TransformPoint(pos2d);
 
        }


    }

    public static Vector3 PosTo2DRect(RectTransform rt, Vector3 pos, Canvas c)
    {
        Vector3 globalMousePos;
        //Debug.Log(c.name + " " + rt.name);
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(c.transform as RectTransform, pos, c.worldCamera, out globalMousePos))
        {
          //  Debug.Log(rt);
            
           // rt.position = globalMousePos;
            rt.rotation = ((RectTransform)c.transform).rotation;
        }
        return globalMousePos;
    }

    public static Vector3 PosTo2DRect(Transform rt, Vector3 pos, Canvas c)
    {
        Vector3 globalMousePos;
        //Debug.Log(c.name + " " + rt.name);
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(c.transform as RectTransform, pos, c.worldCamera, out globalMousePos))
        {
            //  Debug.Log(rt);

            // rt.position = globalMousePos;
            rt.rotation = ((RectTransform)c.transform).rotation;
        }
        return globalMousePos;
    }






}


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

    public static Vector3 WithZ(this Vector3 v, float newZ)
    {
        return new Vector3(v.x, v.y, newZ);
    }

    public static Rect WithInsidePadding(this Rect rect, float padding){
		rect.x += padding;
		rect.xMax -= padding * 2;
		rect.y += padding;
		rect.yMax -= padding * 2;
		
		return rect;
	}

	public static Rect WithHeight(this Rect rect, float height){
		rect.height = height;
		return rect;
	}
}