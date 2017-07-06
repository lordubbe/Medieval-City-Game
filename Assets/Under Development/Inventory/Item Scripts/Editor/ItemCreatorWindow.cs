using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ItemCreatorWindow : EditorWindow {

	[MenuItem ("Tools/Item Editor")]
	public static void ShowWindow(){
		EditorWindow.GetWindow (typeof(ItemCreatorWindow));
	}

	private float headerHeight = 15f;
	private Vector2 previewBoxPercentage = new Vector2(0.5f, 0.5f);
	private float totalOffset = 0f;

	//system modes
	private bool showIconSettings = false;
	private bool showItemSettings = false;
	public GameObject iconCamEnvironment;
	private GameObject iconCamEnvInstance;

	private float gridSquareSize = 100f;
	private float previewScale = 1f;

	public Item currentItem;



	void OnGUI(){
		//limit the size of the window
		minSize = new Vector2(315f, 315f);

		Rect space = new Rect (0, 0, position.width, position.height);
//		gridSquareSize = space.width / 6.5f;
		

		Rect headerSpace = space;
		headerSpace.y += 5;
		headerSpace.height = headerHeight;
		HeaderAreaUI (headerSpace);

		Rect editorSpace = space;
		editorSpace.y = headerSpace.yMax;
		editorSpace.yMax = space.yMax;
		ItemEditorUI (editorSpace.WithInsidePadding(10f));

		Repaint ();
	}

	void HeaderAreaUI(Rect space){
		Util.DropShadowHeaderLabel (space, "Item Editor Window", GUIStyles.WindowHeaderStyle, Color.white);
	}

	void ItemEditorUI(Rect space){
		GUI.Box (space, "");

		//TOOLBAR
		Rect toolbarSpace = space;
		toolbarSpace.yMin += 2f;
		toolbarSpace.height = 15;

		//new item
		Rect newItemRect = toolbarSpace;
		newItemRect.width /= 3;
		if (GUI.Button (newItemRect.WithHorizontalPadding(2f), "New Item")) {
			CreateNewItem ();
		}
		//save item
		Rect saveItemRect = newItemRect;
		saveItemRect.x = newItemRect.xMax;
		if (GUI.Button (saveItemRect.WithHorizontalPadding(2f), "Save Item (autosaves)")) {
			SaveItem ();
		}

		//load item
		Rect loadItemRect = saveItemRect;
		loadItemRect.x = saveItemRect.xMax;
		currentItem = (Item)EditorGUI.ObjectField (loadItemRect.WithHorizontalPadding(2f), currentItem, typeof(Item), false);


		//EDITOR
		Rect editSpace = space;
		editSpace.yMin = toolbarSpace.yMax;
		editSpace = editSpace.WithInsidePadding (2f);
		GUI.Box (editSpace, "");

		if (currentItem != null) {
			//Adjust grid size
			switch (currentItem.width) {
			case 1:
				gridSquareSize = 150f;
				break;
			case 2:
				gridSquareSize = (editSpace.width/2)/2;
				break;
			case 3: 
				gridSquareSize = (editSpace.width/2)/3;
				break;
			default:
				gridSquareSize = (editSpace.width / 2) / currentItem.width;
				break;
			}
			//Item preview
			Rect itemPreviewSpace = new Rect(editSpace.x + 10f, editSpace.y + 25f, currentItem.width * gridSquareSize, currentItem.height*gridSquareSize);

			//Name header
			Rect itemNameSpace = editSpace;
			Util.DropShadowHeaderLabel (itemNameSpace, "Now editing '"+currentItem.name+"'", GUIStyles.ItemNameStyle, Color.white);

			if (currentItem.runtimeRepresentation != null) {
				if (currentItem.iconTexture != null) {
//					GUI.DrawTexture (itemPreviewSpace, (Texture)currentItem.icon);
					EditorGUI.DrawTextureTransparent(itemPreviewSpace, (Texture)currentItem.iconTexture);
				} else {
					currentItem.iconTexture = AssetPreview.GetAssetPreview (currentItem.runtimeRepresentation);
					currentItem.iconTexture.filterMode = FilterMode.Bilinear;
					currentItem.iconTexture.alphaIsTransparency = true;
				}

				//draw grid
				for (int x = 0; x < currentItem.width; x++) {
					for (int y = 0; y < currentItem.height; y++) {
						Rect square = new Rect (itemPreviewSpace.x + x * gridSquareSize, itemPreviewSpace.y + y * gridSquareSize, gridSquareSize, gridSquareSize);
						//EditorGUI.DrawRect (square.WithInsidePadding (1f), Color.white.WithAlpha (0.2f));
						Util.DrawOutlineRect(square.WithInsidePadding(2f), Color.black.WithAlpha(0f), Color.white.WithAlpha(0.45f), 1f);
					}
				}

				//ICON SETTINGS
				Rect itemPreviewSettingsSpace = itemPreviewSpace;
				itemPreviewSettingsSpace.x = itemPreviewSpace.xMax + 5;
				itemPreviewSettingsSpace.xMax = space.xMax - 15;
				itemPreviewSettingsSpace.height = showIconSettings ? 145 : 20f;

				if (SectionUI ("Icon Settings", itemPreviewSettingsSpace, ref showIconSettings)) {
					totalOffset += 120f;

					//First make sure that the current item has IconSettings
					if (currentItem.iconSettings == null) {
						currentItem.iconSettings = new IconSettings (Vector3.zero, Vector3.zero, true, 5f);
					}

					//Scale slider
					Rect scaleSliderRect = itemPreviewSettingsSpace.WithInsidePadding (2f);
					scaleSliderRect.y += 18f;
					scaleSliderRect.height = 15f;

					//Offset editing
					Rect offsetRect = scaleSliderRect;
					offsetRect.y = scaleSliderRect.yMax;
					currentItem.iconSettings.itemOffset = EditorGUI.Vector3Field (offsetRect, "Item Offset", currentItem.iconSettings.itemOffset);

					//Rotation editing
					Rect rotationRect = offsetRect;
					rotationRect.y = offsetRect.yMax + 15f;
					currentItem.iconSettings.itemRotation = EditorGUI.Vector3Field (rotationRect, "Item Rotation", currentItem.iconSettings.itemRotation);

					//Generate Sprite button
					Rect spriteButton = rotationRect;
					spriteButton.y = rotationRect.yMax + 45;
					if (GUI.Button (spriteButton, "Generate Sprite")) {
						Sprite icon = Sprite.Create (currentItem.iconTexture, new Rect (0, 0, currentItem.iconTexture.width, currentItem.iconTexture.height), new Vector2 (0.5f, 0.5f));
						icon.name = currentItem.name + "_icon";
						currentItem.icon = icon;

						string currentItemPath = AssetDatabase.GetAssetPath (currentItem);
						string[] pathFolders = currentItemPath.Split (new char[]{ '/' }, System.StringSplitOptions.None);

						string currentItemPathNoAsset = currentItemPath.Substring (0, currentItemPath.Length - pathFolders [pathFolders.Length - 1].Length-1); 
						currentItemPathNoAsset += "/" + icon.name + ".asset";

						AssetDatabase.CreateAsset (icon, currentItemPathNoAsset);
					}


					//Load the snapshot environment if it's not already loaded
					if (iconCamEnvironment == null) {
						string[] paths = AssetDatabase.FindAssets ("Item Icon Snapshot Environment");
						string path = AssetDatabase.GUIDToAssetPath (paths [0]);
						iconCamEnvironment = (GameObject)AssetDatabase.LoadAssetAtPath (path, typeof(GameObject));
					} else {
						//spawn it if it's not already spawned
						if (iconCamEnvInstance == null) {
							iconCamEnvInstance = Instantiate (iconCamEnvironment, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
							iconCamEnvInstance.name = "Item Icon Snapshot Environment";
						}

						//Render to the render texture
						ItemIconEditorEnvironment envScript = iconCamEnvInstance.GetComponent<ItemIconEditorEnvironment>();

						Rect blackBG = rotationRect;
						blackBG.y = rotationRect.yMax + 20f;
						envScript.BGactive = EditorGUI.ToggleLeft (blackBG, "Black Background", envScript.BGactive);

						if (!envScript.hasSpawnedModel) {
							GameObject itemModel = (GameObject)Instantiate (currentItem.runtimeRepresentation, envScript.itemParent.transform.position, Quaternion.identity, envScript.itemParent);
							itemModel.layer = LayerMask.NameToLayer ("Item Setup");
							envScript.hasSpawnedModel = true;
						}
						if (envScript != null) {

							//Camera options (saved in IconSettings in currentItem)
							Rect orthoBool = scaleSliderRect;
							orthoBool.width = scaleSliderRect.width / 2 - 20f;
							orthoBool.y = scaleSliderRect.y;
							EditorGUIUtility.labelWidth = 50f;

							if (currentItem.iconSettings.orthographicCamera) {
								currentItem.iconSettings.orthographicCamera = EditorGUI.ToggleLeft (orthoBool, "Orthographic Camera", currentItem.iconSettings.orthographicCamera);
								Rect orthoSize = orthoBool;
								orthoSize.x = orthoBool.xMax;
								orthoSize.xMax = offsetRect.xMax;
								currentItem.iconSettings.orthographicScale = EditorGUI.Slider (orthoSize, "Scale", currentItem.iconSettings.orthographicScale, 0.1f, 50f);
							} else {

								Rect persp = orthoBool;
								persp.x = orthoBool.xMax;
								persp.xMax = offsetRect.xMax;
								currentItem.iconSettings.itemDistance = EditorGUI.Slider (persp, "Distance", currentItem.iconSettings.itemDistance, 0.1f, 20f);
								if (!EditorGUI.ToggleLeft (orthoBool, "Perspective Camera", true)) {
									currentItem.iconSettings.orthographicCamera = true;
								}
							}

							//Update the camera in the Icon Design Environment
							if (!currentItem.iconSettings.orthographicCamera) {
								Vector3 camToItem = envScript.itemParent.transform.position - envScript.camera.transform.position;
								Vector3 newPos = envScript.camera.transform.position + (camToItem.normalized * currentItem.iconSettings.itemDistance);
								envScript.itemParent.transform.position = newPos;
							}
							envScript.camera.orthographic = currentItem.iconSettings.orthographicCamera;
							envScript.camera.orthographicSize = currentItem.iconSettings.orthographicScale;
							envScript.itemParent.position += currentItem.iconSettings.itemOffset;
							envScript.itemParent.eulerAngles = currentItem.iconSettings.itemRotation;


							//Update the render texture
							if (Event.current.type == EventType.repaint) {
								RenderTexture renderTex = RenderTexture.active;

								if (envScript.camera.targetTexture.name != (currentItem.name + " (" + currentItem.width + "x" + currentItem.height + ")")) {
									RenderTexture r = new RenderTexture (256 * currentItem.width, 256 * currentItem.height, 32, RenderTextureFormat.ARGB32);
									r.Create ();
									r.name = currentItem.name + " (" + currentItem.width + "x" + currentItem.height + ")";
									envScript.camera.targetTexture = r;
									RenderTexture.active = r;
								} else {
									RenderTexture.active = envScript.camera.targetTexture;
								}

								Texture2D tex = new Texture2D (envScript.camera.targetTexture.width, envScript.camera.targetTexture.height);
								tex.ReadPixels (new Rect (0, 0, tex.width, tex.height), 0, 0);
								tex.Apply ();
								currentItem.iconTexture = tex;
								currentItem.iconTexture.name = "HOV!";
								RenderTexture.active = renderTex;
							}
						}



					}
				}else {
					if (iconCamEnvInstance != null) {
						DestroyImmediate (iconCamEnvInstance);
					}
					totalOffset += 20f;
				}


				//ITEM SETTINGS
				Rect itemSettingsSpace = itemPreviewSettingsSpace;
				itemSettingsSpace.y = itemPreviewSettingsSpace.yMax + 5f;
				itemSettingsSpace.height = showItemSettings ? 320 : 20f;

				if (SectionUI ("Item Settings", itemSettingsSpace, ref showItemSettings)) {
					//Name field
					Rect nameField = itemSettingsSpace;
					nameField.height = 15f;
					nameField.y = itemSettingsSpace.y + 22f;
					nameField = nameField.WithHorizontalPadding (2f);

					float prevLabelWidth = EditorGUIUtility.labelWidth;
					EditorGUIUtility.labelWidth = 50f;
					currentItem.name = EditorGUI.TextField (nameField, "Name", currentItem.name);
					EditorGUIUtility.labelWidth = prevLabelWidth;

					//Flavor Text
					Rect flavTextLabel = nameField;
					flavTextLabel.y = nameField.yMax + 5;
					EditorGUI.LabelField (flavTextLabel, "Flavor Text");

					Rect flavTextField = flavTextLabel;
					flavTextField.height = 50f;
					flavTextField.y = flavTextLabel.yMax;
					currentItem.flavorText = EditorGUI.TextArea (flavTextField, currentItem.flavorText);

					//Dimensions
					Rect itemDimFields = flavTextField;
					itemDimFields.y = flavTextField.yMax + 5;
					itemDimFields.height = 15f;

					Rect itemDimPart = itemDimFields;
					itemDimPart.width /= 2;

					EditorGUIUtility.labelWidth = 50f;
					currentItem.width = EditorGUI.IntField (itemDimPart, "Width", currentItem.width);
					itemDimPart.x += itemDimPart.width;
					currentItem.height = EditorGUI.IntField (itemDimPart, "Height", currentItem.height);
					EditorGUIUtility.labelWidth = prevLabelWidth;

					Rect isContainerBox = itemDimFields;
					isContainerBox.y = itemDimFields.yMax + 5;

					//Inventory
					if (currentItem is Inventory) {
						isContainerBox.yMax = itemSettingsSpace.yMax - 2;
						GUI.Box (isContainerBox, "Inventory Settings");
					} else {
						if (GUI.Button (isContainerBox, "Add Inventory")) {
							UpgradeToInventory (currentItem);
						}
					}
				}

			} else {
				EditorGUI.HelpBox (itemPreviewSpace, "No graphical representation selected! Oh noes!", MessageType.Warning);
			}

		} else {
			EditorGUI.HelpBox (editSpace.WithInsidePadding(5f), "No Item selected! Create a new one, or open an existing one before continuing.", MessageType.Error);
		}

	}

	bool SectionUI(string sectionName, Rect space, ref bool showBool){
		GUI.Box (space, ""); 

		//Header
		Rect iconSettingsLabel = space.WithInsidePadding (2f);
		iconSettingsLabel.height = 18f;
		Util.DropShadowHeaderLabel (iconSettingsLabel, sectionName, GUIStyles.SecondHeaderStyle, Color.white*0.9f);

		//Icon Settings expand button
		Rect iconSettingsExpand = iconSettingsLabel;
		iconSettingsExpand.height = 15f;
		iconSettingsExpand.xMin = iconSettingsLabel.xMax - 18f;

		showBool.GetType ();

		if (GUI.Button (iconSettingsExpand, showBool ? "-" : "+")) {
			showBool = !showBool;
		}
			
		return showBool;
	}

	void UpgradeToInventory (Item item){
		//Create inventory and copy all settings
		Inventory newItem = Inventory.CreateInstance<Inventory> ();
		newItem.width = item.width;
		newItem.height = item.height;
		newItem.iconSettings = item.iconSettings;
		newItem.iconTexture = item.iconTexture;
		newItem.icon = item.icon;
		newItem.flavorText = item.flavorText;
		newItem.name = item.name;
		newItem.runtimeRepresentation = item.runtimeRepresentation;

		string path = AssetDatabase.GetAssetPath (item);
		AssetDatabase.CreateAsset (newItem, path);
		currentItem = newItem;
	}

	void CreateNewItem(){

		string path = EditorUtility.SaveFilePanelInProject ("Create a new Item", "New Item", "asset", "Don't forget to like and subscribe for more Minecraft tutorials!");

		//Make sure a path was selected
		if (path != "") {
			Item newItem = Item.CreateInstance<Item> ();

			//make new folder if it doesn't exist
			string[] folderNames = path.Split(new char[]{'/'}, System.StringSplitOptions.None);
//			foreach (string s in folderNames) {
//				Debug.Log (s);
//			}
			string itemName = folderNames[folderNames.Length-1].Substring(0, folderNames[folderNames.Length-1].Length-6);
			if (!AssetDatabase.IsValidFolder (path.Substring (0, path.Length - 6))) {
				string folderPath = path.Substring (0, path.Length - folderNames [folderNames.Length - 1].Length-1); Debug.Log (folderPath);
				string newFolderGUID = AssetDatabase.CreateFolder (folderPath, itemName);
				path = AssetDatabase.GUIDToAssetPath (newFolderGUID) + "/" + folderNames[folderNames.Length-1];
			}
			AssetDatabase.CreateAsset (newItem, path);
			newItem.name = AssetDatabase.LoadAssetAtPath (path,typeof(Item)).name;

			currentItem = newItem;

		}
	}

	void SaveItem(){
		Debug.LogWarning("OH SHIT! THIS FEATURE IS NOT IMPLEMENTED YET! ARRRGGGHHHH!!!!!");
	}

	void OnDestroy(){
		if (iconCamEnvInstance != null) {
			DestroyImmediate (iconCamEnvInstance);
		}
	}
}
	
public class GUIStyles{
	
	public static GUIStyle WindowHeaderStyle = NewGUIStyle(TextAnchor.MiddleCenter, 16, FontStyle.Bold);
	public static GUIStyle SecondHeaderStyle = NewGUIStyle(TextAnchor.UpperLeft, 14, FontStyle.Normal);
	public static GUIStyle ItemNameStyle = NewGUIStyle(TextAnchor.UpperCenter, 14, FontStyle.Normal);

	static GUIStyle NewGUIStyle(TextAnchor alignment, int fontSize, FontStyle fontStyle){
		GUIStyle newStyle = new GUIStyle ();
		newStyle.alignment = alignment;
		newStyle.fontSize = fontSize;
		newStyle.fontStyle = fontStyle;

		return newStyle;
	}

	static GUIStyle NewGUIStyle(TextAnchor alignment, int fontSize, FontStyle fontStyle, bool wordWrap){
		GUIStyle newStyle = new GUIStyle ();
		newStyle.alignment = alignment;
		newStyle.fontSize = fontSize;
		newStyle.fontStyle = fontStyle;
		newStyle.wordWrap = wordWrap;

		return newStyle;
	}

}

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

}

public static class ExtensionMethods{

	public static Color WithAlpha(this Color color, float alpha){
		color.a = alpha;
		return color;
	}

	public static Rect WithInsidePadding(this Rect rect, float padding){
		rect.x += padding;
		rect.xMax -= padding * 2;
		rect.y += padding;
		rect.yMax -= padding * 2;

		return rect;
	}

	public static Rect WithHorizontalPadding(this Rect rect, float padding){
		rect.x += padding;
		rect.xMax -= padding * 2;

		return rect;
	}
}
