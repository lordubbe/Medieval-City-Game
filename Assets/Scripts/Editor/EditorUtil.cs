using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class EditorUtil {
    
    public static void HeaderLabel(Rect space, string label, GUIStyle style, Color color)
    {
        style.normal.textColor = color;
        EditorGUI.LabelField(space, label, style);
    }

    public static void DropShadowHeaderLabel(Rect space, string label, GUIStyle style, Color color)
    {
        //Shadow
        Vector2 prevOff = style.contentOffset;
        style.contentOffset = new Vector2(2f, 2f);
        style.normal.textColor = Color.black.WithAlpha(0.5f);
        EditorGUI.LabelField(space, label, style);

        //Label
        style.contentOffset = prevOff;
        style.normal.textColor = color;
        EditorGUI.LabelField(space, label, style);

    }

    public static void DrawOutlineRect(Rect r, Color fillColor, Color strokeColor, float strokewidth)
    {
        for (int i = 0; i < strokewidth; i++)
        {
            r.x -= 1;
            r.y -= 1;
            r.xMax += 2;
            r.yMax += 2;
            Handles.DrawSolidRectangleWithOutline(r, fillColor, strokeColor);
        }
    }

    #region Editor Util
    public static bool ResizableRegion(Rect region, MouseCursor resizeType, out float modifies, ref bool interacting)
    {
        Event e = Event.current;
        modifies = 0f;
        EditorGUIUtility.AddCursorRect(region, resizeType);

        if (region.IsHovered())
        {
            EditorGUI.DrawRect(region, Color.white.WithAlpha(0.25f));
        }

        if (region.IsClicked())
        {
            interacting = true;
            e.Use();
        }
        if (e.type == EventType.mouseUp)
        {
            interacting = false;
        }

        if (interacting)
        {
            switch (resizeType)
            {
                case MouseCursor.ResizeVertical:
                    modifies = e.mousePosition.y;
                    break;
                case MouseCursor.ResizeHorizontal:
                    modifies = e.mousePosition.x;
                    break;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool FlatButton(Rect rect, string label, Color color, GUIStyle style)
    {
        EditorGUI.DrawRect(rect, color);
        EditorGUI.LabelField(rect, label, style);

        if (rect.IsClicked())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion



}


#region Better Rect classes

public abstract class InteractableRect
{
    public Rect rect;

    public virtual void OnGUI()
    {
        Event e = Event.current;

        HandleInteractionEvents(e);
    }

    void HandleInteractionEvents(Event e)
    {
        if (rect.IsClicked())
        {
            OnInteract(e);
        }

        if (rect.IsRightClicked())
        {
            OnRightClick(e);
        }

        if (e.type == EventType.mouseUp)
        {

            if (rect.Contains(e.mousePosition))
            {
                OnMouseUpOverRect(e);
            }

            OnStopInteract(e);
        }

    }

    protected virtual void OnInteract(Event e) { }

    protected virtual void OnRightClick(Event e) { }

    protected virtual void OnStopInteract(Event e) { }

    protected virtual void OnMouseUpOverRect(Event e) { }
}

public abstract class DraggableRect : InteractableRect, ISelectableUIElement
{
    public bool selected;

    private bool dragging;
    private Vector2 clickOffset = Vector2.zero;

    public override void OnGUI()
    {
        base.OnGUI();

        Event e = Event.current;

        if (dragging)
        {
            Vector2 newPos =
                e.mousePosition -
                new Vector2(rect.width * clickOffset.x, rect.height * clickOffset.y);
            rect.position = newPos;
        }
    }

    protected override void OnInteract(Event e)
    {
        base.OnInteract(e);

        dragging = true;
        OnSelect();
        clickOffset = Rect.PointToNormalized(rect, e.mousePosition);
    }

    protected override void OnStopInteract(Event e)
    {
        base.OnStopInteract(e);

        dragging = false;

        if (selected && !rect.Contains(e.mousePosition))
        {
            OnDeselect();
            GUI.FocusControl("");
        }
    }

    public void OnSelect()
    {
        selected = true;
    }
    public void OnDeselect()
    {
        selected = false;
    }

    public virtual void OnInspectorGUI(Rect space)
    {

    }
}

public abstract class ResizableRect : DraggableRect
{

    public float resizingHandleSize = 5f;
    public float minSize = 35f;

    bool resizingTop, resizingBottom, resizingLeft, resizingRight;

    public override void OnGUI()
    {
        base.OnGUI();
        HandleResizing();
    }

    void HandleResizing()
    {
        float resizingHandleSize = 5f;

        Rect top = rect.WithY(rect.yMin - (resizingHandleSize + 1)).WithHeight(resizingHandleSize);
        float newYMin = 0f;
        if (EditorUtil.ResizableRegion(top, MouseCursor.ResizeVertical, out newYMin, ref resizingTop))
        {
            rect.yMin = newYMin < rect.yMax - minSize ? newYMin : rect.yMax - minSize;
        }

        Rect bottom = rect.WithY(rect.yMax + 1f).WithHeight(resizingHandleSize);
        float newYMax = 0f;
        if (EditorUtil.ResizableRegion(bottom, MouseCursor.ResizeVertical, out newYMax, ref resizingBottom))
        {
            rect.yMax = newYMax > rect.yMin + minSize ? newYMax : rect.yMin + minSize;
        }

        Rect left = rect.WithX(rect.xMin - (resizingHandleSize + 1)).WithWidth(resizingHandleSize);
        float newXMin = 0f;
        if (EditorUtil.ResizableRegion(left, MouseCursor.ResizeHorizontal, out newXMin, ref resizingLeft))
        {
            rect.xMin = newXMin < rect.xMax - minSize ? newXMin : rect.xMax - minSize;
        }

        Rect right = rect.WithX(rect.xMax + 1f).WithWidth(resizingHandleSize);
        float newXMax = 0f;
        if (EditorUtil.ResizableRegion(right, MouseCursor.ResizeHorizontal, out newXMax, ref resizingRight))
        {
            rect.xMax = newXMax > rect.xMin + minSize ? newXMax : rect.xMin + minSize;
        }
    }
}

public interface ISelectableUIElement
{

    void OnSelect();
    void OnDeselect();

    void OnInspectorGUI(Rect space);
}

#endregion
