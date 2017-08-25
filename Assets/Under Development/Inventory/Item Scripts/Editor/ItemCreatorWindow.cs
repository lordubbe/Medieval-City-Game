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

	private bool itemIsDirty;


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
		if (itemIsDirty) {
			GUI.color = Color.red;
			if (GUI.Button (saveItemRect.WithHorizontalPadding (2f), "Save Item")) {
				SaveItem ();
			}
			GUI.color = Color.white;
		} else {
			EditorGUI.LabelField (saveItemRect.WithHorizontalPadding (2f), "Item is up to date!");
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

			//Only draw if currentItem.runtimeRepresentation has been selected
			if (currentItem.runtimeRepresentation != null) {

				//If a texture doesn't already exist, fetch the standard AssetPreview texture
				if (currentItem.iconTexture != null) {
					EditorGUI.DrawTextureTransparent(itemPreviewSpace, (Texture)currentItem.iconTexture);
				} else {
					currentItem.iconTexture = AssetPreview.GetAssetPreview (currentItem.runtimeRepresentation);
					int maxCount = 50;
					while (currentItem.iconTexture == null && maxCount > 0) {
						EditorGUI.HelpBox (itemPreviewSpace, "Loading preview texture...", MessageType.Info);
						maxCount--;
					}
					if (currentItem.iconTexture != null) {
						currentItem.iconTexture.filterMode = FilterMode.Bilinear;
						currentItem.iconTexture.alphaIsTransparency = true;
					} else {
						EditorGUI.HelpBox (itemPreviewSpace, "Couldn't get preview texture :(", MessageType.Error);
					}
				}

				//draw item space grid
				for (int x = 0; x < currentItem.width; x++) {
					for (int y = 0; y < currentItem.height; y++) {
						Rect square = new Rect (itemPreviewSpace.x + x * gridSquareSize, itemPreviewSpace.y + y * gridSquareSize, gridSquareSize, gridSquareSize);
						Util.DrawOutlineRect(square.WithInsidePadding(2f), Color.black.WithAlpha(0f), Color.white.WithAlpha(0.45f), 1f);
					}
				}

				//ICON SETTINGS
				Rect itemPreviewSettingsSpace = itemPreviewSpace;
				itemPreviewSettingsSpace.x = itemPreviewSpace.xMax + 5;
				itemPreviewSettingsSpace.xMax = space.xMax - 15;
				itemPreviewSettingsSpace.height = showIconSettings ? 100 : 20f;

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
//					Rect spriteButton = rotationRect;
//					spriteButton.y = rotationRect.yMax + 25;

//					if (GUI.Button (spriteButton, "Generate Sprite")) {
//						GenerateSpriteAndTexture ();
//					}
						
					//Load the snapshot environment if it's not already loaded
					if (iconCamEnvironment == null) {
						string[] paths = AssetDatabase.FindAssets ("Item Icon Snapshot Environment");
						string path = AssetDatabase.GUIDToAssetPath (paths [0]);
						iconCamEnvironment = (GameObject)AssetDatabase.LoadAssetAtPath (path, typeof(GameObject));
					} else {
						//spawn it if it's not already spawned
						if (iconCamEnvInstance == null) {
							//Check if it's already present in the scene (from previous session, etc.)
							GameObject e = GameObject.Find("Item Icon Snapshot Environment");
							//link it if it already exists
							if (e != null) {
								DestroyImmediate (e);
							} else {
								//Instantiate a new version if it doesn't already exist
								iconCamEnvInstance = Instantiate (iconCamEnvironment, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
								iconCamEnvInstance.name = "Item Icon Snapshot Environment";
							}
						}

						//Render to the render texture
						ItemIconEditorEnvironment envScript = iconCamEnvInstance.GetComponent<ItemIconEditorEnvironment>();

						if (!envScript.hasSpawnedModel) {
							GameObject itemModel = (GameObject)Instantiate (currentItem.runtimeRepresentation, envScript.itemParent.transform.position, Quaternion.identity, envScript.itemParent);
							//set layer to the one rendered by the icon environment
							itemModel.layer = LayerMask.NameToLayer ("Item Setup");
							foreach (Transform child in itemModel.transform) {
								child.gameObject.layer = LayerMask.NameToLayer ("Item Setup");
							}
							
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

							//Check if icon has been updated
							Vector3 newPos = envScript.camera.transform.forward * currentItem.iconSettings.itemDistance;
							if (envScript.camera.orthographic != currentItem.iconSettings.orthographicCamera ||
							envScript.camera.orthographicSize != currentItem.iconSettings.orthographicScale ||
							envScript.itemParent.position != envScript.camera.transform.position + (newPos + currentItem.iconSettings.itemOffset) ||
							envScript.itemParent.eulerAngles != currentItem.iconSettings.itemRotation) 
							{
								SetItemDirty (); //TODO: Only set dirty when it's because values have been changed. NOT when the item was just changed while the icon settings was opened.
								envScript.camera.orthographic = currentItem.iconSettings.orthographicCamera;
								envScript.camera.orthographicSize = currentItem.iconSettings.orthographicScale;
								envScript.itemParent.position = envScript.camera.transform.position + (newPos + currentItem.iconSettings.itemOffset);
								envScript.itemParent.eulerAngles = currentItem.iconSettings.itemRotation;
							}

							//Update the render texture (only when item is marked as dirty)
							if (Event.current.type == EventType.repaint && itemIsDirty) {
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
								currentItem.iconTexture.name = (currentItem.name+"_icon_texture");
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
				itemSettingsSpace.height = showItemSettings ? 435 : 20f;

				if (SectionUI ("Item Settings", itemSettingsSpace, ref showItemSettings)) {

					Rect autoSaveInfo = itemSettingsSpace;
					autoSaveInfo.width /= 2;
					autoSaveInfo.height = 16;
					autoSaveInfo.x += autoSaveInfo.width;
					autoSaveInfo.y += 4f;
					EditorGUI.LabelField (autoSaveInfo, "(autosaves)");

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
						Inventory currentItemInv = currentItem as Inventory;
						isContainerBox.yMax = itemSettingsSpace.yMax - 2;
						GUI.Box (isContainerBox, "");

						Rect downGradeButton = isContainerBox;
						downGradeButton.xMin = isContainerBox.xMax - 110f;
						downGradeButton.yMax = isContainerBox.y + 18;
						downGradeButton.y += 2f;

						if (GUI.Button (downGradeButton, "Remove Inventory")) {
							InventoryToItem ((Inventory)currentItem);
						}

						//Inventory Scale
						Rect invDimFields = itemDimFields;
						invDimFields.width /= 2;
						invDimFields.y = downGradeButton.yMax + 5;
						EditorGUI.LabelField (invDimFields, "Textures should be square.");
						invDimFields.x += invDimFields.width - 2;

						Rect invDimPart = invDimFields;
						invDimPart.width /= 2;
						EditorGUIUtility.labelWidth = 40f;
						currentItemInv.inventoryWidth = (int)Mathf.Clamp(EditorGUI.IntField (invDimPart, "Width", currentItemInv.inventoryWidth), 1, int.MaxValue);
						invDimPart.x += invDimPart.width;
						currentItemInv.inventoryHeight = (int)EditorGUI.IntField (invDimPart, "Height", currentItemInv.inventoryWidth);
						EditorGUIUtility.labelWidth = prevLabelWidth;

						//Inventory Background Image Picker
						Rect bgImg = invDimFields;
						bgImg.width /= 2;
						bgImg.x += bgImg.width;
						bgImg.y = invDimPart.yMax + 5;
						bgImg.height = bgImg.width;

						currentItemInv.inventoryBackgroundImage = (Texture2D) EditorGUI.ObjectField (bgImg, currentItemInv.inventoryBackgroundImage, typeof(Texture2D), false);

						//Show preview
						Rect invPrev = invDimFields;
						invPrev.y = invDimFields.yMax + 5;
						invPrev.x -= invPrev.width - 5;
						invPrev.xMax = bgImg.x - 5;
						invPrev.height = invPrev.width;

						if (currentItemInv.inventoryBackgroundImage != null) {
							//draw texture preview
							GUI.DrawTexture (invPrev, currentItemInv.inventoryBackgroundImage);

							//Create sprite if it doesn't already exist
							if (currentItemInv.inventoryBackgroundSprite == null || currentItemInv.inventoryBackgroundImage != currentItemInv.inventoryBackgroundSprite.texture) {
								Sprite bg = Sprite.Create (currentItemInv.inventoryBackgroundImage, new Rect (0, 0, currentItemInv.inventoryBackgroundImage.width, currentItemInv.inventoryBackgroundImage.height), new Vector2 (0, 0));
								bg.name = (currentItemInv.name + "_InvBG_Sprite");
								currentItemInv.inventoryBackgroundSprite = bg;

								string currentItemPath = AssetDatabase.GetAssetPath (currentItemInv);
								string[] pathFolders = currentItemPath.Split (new char[]{ '/' }, System.StringSplitOptions.None);

								string currentItemPathNoAsset = currentItemPath.Substring (0, currentItemPath.Length - pathFolders [pathFolders.Length - 1].Length-1); 
								currentItemPathNoAsset += "/" + bg.name + ".asset";

								AssetDatabase.CreateAsset (bg, currentItemPathNoAsset);
								EditorUtility.SetDirty (currentItemInv);
								AssetDatabase.SaveAssets ();
								AssetDatabase.Refresh ();
							}

							//draw grid
							float gridSize = invPrev.width/currentItemInv.inventoryWidth;

							if (currentItemInv.spaces == null || (currentItemInv.spaces.Count != currentItemInv.inventoryWidth * currentItemInv.inventoryHeight)){//(currentItemInv.spaces.GetLength (0) != currentItemInv.inventoryWidth || currentItemInv.spaces.GetLength (1) != currentItemInv.inventoryHeight)) {
								int capacity = currentItemInv.inventoryWidth * currentItemInv.inventoryHeight;
								currentItemInv.spaces = new List<InventorySpace>(capacity);
								for (int i = 0; i < capacity; i++) {
									currentItemInv.spaces.Add(new InventorySpace ());
								}
								EditorUtility.SetDirty (currentItemInv);
								AssetDatabase.SaveAssets ();
								AssetDatabase.Refresh ();
							}

							for (int x = 0; x < currentItemInv.inventoryWidth; x++) {
								for (int y = 0; y < currentItemInv.inventoryHeight; y++) {
									Rect square = new Rect (invPrev.x + x * gridSize, invPrev.y + y * gridSize, gridSize, gridSize);
									Event e = Event.current;
									InventorySpace curSpace = currentItemInv.spaces [y * currentItemInv.inventoryWidth + x]; //rewrite this shitty code
									if (square.Contains (e.mousePosition)) {
										EditorGUI.DrawRect (square, Color.white.WithAlpha (0.2f));
										if (e.type == EventType.mouseDown) {
											curSpace.isActive = !curSpace.isActive;

											//TODO: Optimize so that it doesn't lag out every time you update a square
											EditorUtility.SetDirty (currentItemInv);
											AssetDatabase.SaveAssets ();
											AssetDatabase.Refresh ();
										}
									}
									if (curSpace != null) {
										if (curSpace.isActive) {
											Util.DrawOutlineRect (square.WithInsidePadding (2f), Color.black.WithAlpha (0f), Color.white.WithAlpha (0.45f), 1f);
										} else {
											Util.DrawOutlineRect (square.WithInsidePadding (2f), Color.black.WithAlpha (0f), Color.white.WithAlpha (0.2f), 1f);
											EditorGUI.DrawRect (square.WithInsidePadding (2f), Color.black.WithAlpha (0.5f));
										}
									} else {
										
										curSpace = new InventorySpace ();
									}
								}
							}


						} else {
							EditorGUI.HelpBox (invPrev, "Please select a background image for the inventory.", MessageType.Error);
						}

					} else {
						if (GUI.Button (isContainerBox, "Make Inventory")) {
							ItemToInventory (currentItem);
						}
					}
				}

			} else {

				//Drag & drop of new item
				Event e = Event.current;
				//change mouse cursor
				if (e.type == EventType.dragUpdated) {
					DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
				}

				if (itemPreviewSpace.Contains (e.mousePosition)) {
					EditorGUI.HelpBox (itemPreviewSpace, "Release left mouse button to drop the item as the graphical representation of this object!", MessageType.Info);

					if (e.type == EventType.DragPerform) {
						Object[] dragObjs = DragAndDrop.objectReferences;

						for(int i=0; i<dragObjs.Length; i++){
							if (dragObjs [i] is GameObject) {
								currentItem.runtimeRepresentation = dragObjs [i] as GameObject;
								SetItemDirty ();
								break;
							}
						}

						DragAndDrop.AcceptDrag ();
					}
				} else {
					EditorGUI.HelpBox (itemPreviewSpace, "No graphical representation selected! Drag & Drop an object into this box!", MessageType.Warning);
				}
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

	void SetItemDirty(){
		itemIsDirty = true;
	}

	void GenerateSpriteAndTexture(){
		Sprite icon = Sprite.Create (currentItem.iconTexture, new Rect (0, 0, currentItem.iconTexture.width, currentItem.iconTexture.height), new Vector2 (0.5f, 0.5f));
		icon.name = currentItem.name + "_icon";
		currentItem.icon = icon;

		SaveAsAsset (icon, currentItem, icon.name);
		AssetDatabase.AddObjectToAsset (currentItem.iconTexture, icon);
	}

	void SaveAsAsset(Object obj, Item item, string name){
		string currentItemPath = AssetDatabase.GetAssetPath (item);
		string[] pathFolders = currentItemPath.Split (new char[]{ '/' }, System.StringSplitOptions.None);

		string currentItemPathNoAsset = currentItemPath.Substring (0, currentItemPath.Length - pathFolders [pathFolders.Length - 1].Length-1); 
		currentItemPathNoAsset += "/" + name + ".asset";

		AssetDatabase.CreateAsset (obj, currentItemPathNoAsset);
		EditorUtility.SetDirty (item);
		AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh ();
	}

	void ItemToInventory (Item item){
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

		//Inventory specifics
		newItem.spaces = new List<InventorySpace>(1);
		newItem.spaces.Add(new InventorySpace ());
//		newItem.inventoryWidth = 1;
//		newItem.inventoryHeight = 1;
		newItem.availableSpace = item.width * item.height;

		string path = AssetDatabase.GetAssetPath (item);
		AssetDatabase.CreateAsset (newItem, path);
		currentItem = newItem;
	}

	void InventoryToItem(Inventory item){
		Item newItem = Item.CreateInstance<Item> ();
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
		//TODO: Save all aspects of the asset
		GenerateSpriteAndTexture();
		itemIsDirty = false;
	}

	void OnLostFocus(){
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
