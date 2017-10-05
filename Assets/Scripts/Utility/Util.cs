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

	#region Editor Utility
	public static bool ResizableRegion(Rect region, MouseCursor resizeType, out float modifies, ref bool interacting){
		Event e = Event.current;
		modifies = 0f;
		EditorGUIUtility.AddCursorRect (region, resizeType);

		if (region.IsHovered ()) {
			EditorGUI.DrawRect (region, Color.white.WithAlpha (0.25f));
		}

		if (region.IsClicked ()) {
			interacting = true;
			e.Use ();
		}
		if (e.type == EventType.mouseUp) {
			interacting = false;
		}

		if (interacting) {
			switch (resizeType) {
			case MouseCursor.ResizeVertical:
				modifies = e.mousePosition.y;
				break;
			case MouseCursor.ResizeHorizontal:
				modifies = e.mousePosition.x;
				break;
			}
			return true;
		} else {
			return false;
		}
	}

	public static bool FlatButton(Rect rect, string label, Color color, GUIStyle style){
		EditorGUI.DrawRect (rect, color);
		EditorGUI.LabelField (rect, label, style);

		if (rect.IsClicked ()) {
			return true;
		} else {
			return false;
		}
	}

	#endregion

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

	#region Color 

	public static Color WithAlpha(this Color color, float alpha){
		color.a = alpha;
		return color;
	}

	#endregion

	#region Vector3

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

	#endregion

	#region Rect

	public static Rect WithHorizontalPadding(this Rect rect, float padding){
		rect.x += padding;
		rect.xMax -= padding * 2;

		return rect;
	}

	public static Rect WithVerticalPadding(this Rect rect, float padding){
		rect.y += padding;
		rect.yMax -= padding * 2;

		return rect;
	}

    public static Rect WithPadding(this Rect rect, float padding){
		rect.x += padding;
		rect.xMax -= padding * 2;
		rect.y += padding;
		rect.yMax -= padding * 2;
		
		return rect;
	}

	public static Rect WithX(this Rect rect, float x){
		rect.xMin = x;
		return rect;
	}

	public static Rect WithY(this Rect rect, float y){
		rect.yMin = y;
		return rect;
	}

	public static Rect WithWidth(this Rect rect, float width){
		rect.width = width;
		return rect;
	}

	public static Rect WithHeight(this Rect rect, float height){
		rect.height = height;
		return rect;
	}

	public static Rect WithCenter(this Rect rect, Vector2 position){
		rect.x = position.x - rect.width / 2;
		rect.y = position.y - rect.height / 2;
		return rect;
	}

	public static Rect WithHorizontalCenter(this Rect rect, float x){
		rect.x = x - rect.width / 2;
		return rect;
	}

	public static Rect WithVerticalCenter(this Rect rect, float y){
		rect.y = y - rect.height / 2;
		return rect;
	}
		
	#endregion

	#region GUIStyle

	public static GUIStyle WithFontColor(this GUIStyle style, Color color){
		GUIStyle newStyle = new GUIStyle (style);
		newStyle.normal.textColor = color;
		return newStyle;
	}

	public static GUIStyle WithWordWrap(this GUIStyle style){
		GUIStyle newStyle = new GUIStyle (style);
		newStyle.wordWrap = true;
		return newStyle;
	}

	public static GUIStyle WithCenteredAlignment(this GUIStyle style){
		GUIStyle newStyle = new GUIStyle (style);
		newStyle.alignment = TextAnchor.MiddleCenter;
		return newStyle;
	}

	#endregion

	#region Event

	public static bool IsClicked(this Rect rect){
		Event e = Event.current;
		return rect.Contains (e.mousePosition) && e.type == EventType.mouseDown && e.button == 0;
	}

	public static bool IsRightClicked(this Rect rect){
		Event e = Event.current;
		return rect.Contains (e.mousePosition) && e.type == EventType.mouseDown && e.button == 1;
	}

	public static bool IsHovered(this Rect rect){
		Event e = Event.current;
		return rect.Contains (e.mousePosition);
	}

	#endregion
}

#region Better Rect classes

public abstract class InteractableRect{
	public Rect rect;

	public virtual void OnGUI(){
		Event e = Event.current;

		HandleInteractionEvents (e);
	}

	void HandleInteractionEvents(Event e){
		if (rect.IsClicked ()) {
			OnInteract (e);
		}

		if (rect.IsRightClicked ()) {
			OnRightClick (e);
		}

		if (e.type == EventType.mouseUp) {
			
			if (rect.Contains (e.mousePosition)) {
				OnMouseUpOverRect (e);
			}

			OnStopInteract (e);
		}

	}

	protected virtual void OnInteract(Event e){	}

	protected virtual void OnRightClick(Event e){ }

	protected virtual void OnStopInteract(Event e){	}

	protected virtual void OnMouseUpOverRect(Event e){ }
}

public abstract class DraggableRect : InteractableRect, ISelectableUIElement{
	public bool selected;

	private bool dragging;
	private Vector2 clickOffset = Vector2.zero;

	public override void OnGUI(){
		base.OnGUI ();

		Event e = Event.current;

		if (dragging) {
			Vector2 newPos = 
				e.mousePosition - 
				new Vector2 (rect.width * clickOffset.x, rect.height * clickOffset.y);  
			rect.position = newPos;
		}
	}

	protected override void OnInteract(Event e){
		base.OnInteract (e);

		dragging = true;
		OnSelect ();
		clickOffset = Rect.PointToNormalized (rect, e.mousePosition);
	}

	protected override void OnStopInteract(Event e){
		base.OnStopInteract (e);

		dragging = false;

		if (selected && !rect.Contains (e.mousePosition)) {
			OnDeselect ();
			GUI.FocusControl ("");
		}
	}

	public void OnSelect(){ 
		selected = true;
	}
	public void OnDeselect(){ 
		selected = false;
	}

	public virtual void OnInspectorGUI(Rect space){

	}
}

public abstract class ResizableRect : DraggableRect {

	public float resizingHandleSize = 5f;
	public float minSize = 35f;

	bool resizingTop, resizingBottom, resizingLeft, resizingRight;

	public override void OnGUI(){
		base.OnGUI ();
		HandleResizing ();
	}

	void HandleResizing(){
		float resizingHandleSize = 5f;

		Rect top = rect.WithY(rect.yMin - (resizingHandleSize+1)).WithHeight (resizingHandleSize);
		float newYMin = 0f;
		if (Util.ResizableRegion (top, MouseCursor.ResizeVertical, out newYMin, ref resizingTop)) {
			rect.yMin = newYMin < rect.yMax - minSize ? newYMin : rect.yMax-minSize;
		}

		Rect bottom = rect.WithY (rect.yMax + 1f).WithHeight (resizingHandleSize);
		float newYMax = 0f;
		if (Util.ResizableRegion (bottom, MouseCursor.ResizeVertical, out newYMax, ref resizingBottom)) {
			rect.yMax = newYMax > rect.yMin + minSize ? newYMax : rect.yMin + minSize;
		}

		Rect left = rect.WithX (rect.xMin - (resizingHandleSize+1)).WithWidth (resizingHandleSize);
		float newXMin = 0f;
		if (Util.ResizableRegion (left, MouseCursor.ResizeHorizontal, out newXMin, ref resizingLeft)) {
			rect.xMin = newXMin < rect.xMax - minSize ? newXMin : rect.xMax - minSize;
		}

		Rect right = rect.WithX (rect.xMax + 1f).WithWidth (resizingHandleSize);
		float newXMax = 0f;
		if (Util.ResizableRegion (right, MouseCursor.ResizeHorizontal, out newXMax, ref resizingRight)) {
			rect.xMax = newXMax > rect.xMin + minSize ? newXMax : rect.xMin + minSize;
		}
	}
}

public interface ISelectableUIElement{

	void OnSelect();
	void OnDeselect();

	void OnInspectorGUI (Rect space);
}

#endregion