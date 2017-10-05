using UnityEngine;
using System.Collections;
using UnityEditor;

public class UIEntryPoint : DraggableRect
{
	public int groupID = -1;

	public UIOutlet outlet;
	private static Color entryPointColor = new Color(0, 1, 1);

	public override void OnGUI()
	{
		base.OnGUI();

		EditorGUI.DrawRect(rect, entryPointColor);
	}

    public UIEntryPoint(int groupID){
        this.groupID = groupID;
        outlet = new UIOutlet(null);
    }
}