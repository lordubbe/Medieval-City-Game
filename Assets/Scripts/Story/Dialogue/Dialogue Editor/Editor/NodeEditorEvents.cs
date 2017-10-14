using UnityEngine;

public delegate void NodeConnectionEvent(Option o);
public class NodeEditorEvents {

	public static UIConnection activeConnection;

	public static NodeConnectionEvent OnOptionSeekConnect;
//	public static NodeConnectionEvent OnOption	
}
