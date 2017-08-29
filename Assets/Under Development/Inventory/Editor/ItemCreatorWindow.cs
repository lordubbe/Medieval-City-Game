using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class ItemCreatorWindow : EditorWindow, IHasCustomMenu {

	[MenuItem ("Tools/Item Editor %i")]
	public static void ShowWindow(){
		EditorWindow.GetWindow (typeof(ItemCreatorWindow));
	}
		
	private float headerHeight = 15f;
	private Vector2 minSizeVector = new Vector2 (315f, 315f);
	private Vector2 previewBoxPercentage = new Vector2(0.5f, 0.5f);
	private float totalOffset = 0f;

	//system modes
	private bool showIconSettings = false;
	private bool showItemSettings = true;
	private bool showInventorySettings = false;

	public GameObject iconCamEnvironment;
	private GameObject iconCamEnvInstance;
	private ItemIconEditorEnvironment envScript;

	private float gridSquareSize = 100f;
	private float previewScale = 1f;

	public GameObject currentItemObj;
	public Item currentItem;

	private bool itemIsDirty;

	#region window lock functionality
	// Lock window
	[System.NonSerialized]
	GUIStyle lockButtonStyle;

	[System.NonSerialized]
	bool locked = false;
	void ShowButton(Rect position) {
		if (lockButtonStyle == null)
			lockButtonStyle = "IN LockButton";
		locked = GUI.Toggle(position, locked, GUIContent.none, lockButtonStyle);
	}
	void IHasCustomMenu.AddItemsToMenu(GenericMenu menu) {
		menu.AddItem(new GUIContent("Lock"), locked, () => {
			locked = !locked;
		});
	}
	#endregion

	void OnGUI(){
		//limit the size of the window
		minSize = minSizeVector;

		Rect space = new Rect (0, 0, position.width, position.height);

		WindowLockedStateSelection ();

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

	void WindowLockedStateSelection(){
		// Change currentItem to be the recent selected (if window is not locked)
		if (!locked) {
			if (Selection.activeGameObject != null) {
				Item i = Selection.activeGameObject.GetComponent<Item> ();
				if (i != null) { // Make sure selected object is actually an Item
					// If it's an item, we want to edit the PREFAB and not the INSTANCE if it's selected in the scene!
					GameObject j = PrefabUtility.GetPrefabParent (Selection.activeObject) as GameObject;
					if (j != null) {
						currentItem = j.GetComponent<Item> ();
						currentItemObj = currentItem.gameObject;
					} else {
						currentItem = i;
						currentItemObj = i.gameObject;
					}
				}
			}
		}
	}

	void HeaderAreaUI(Rect space){
		Util.HeaderLabel (space, "Item Editor Window", GUIStyles.WindowHeaderStyle, Color.black);
	}

	void ItemEditorUI(Rect space){
				
		//TOOLBAR
		Rect toolbarSpace = space;
		toolbarSpace.yMin += 2f;
		toolbarSpace.height = 15;
		int buttonIdx = GUI.Toolbar (toolbarSpace.WithHorizontalPadding(2f),-1, new string[]{ "New Item", "Save Item", "Load Item"}, EditorStyles.miniButton);

		switch (buttonIdx) {
		case 0: // New Item
			CreateNewItem ();
			break;
		case 1: // Save Item
			if (currentItem != null) {
				SaveItem ();
			} else {
				EditorUtility.DisplayDialog ("U WOT M8", "You can't save an Item that is not loaded, doofus!", ":'(");
			}
			break;
		case 2: // Load Item
			LoadItem ();
			break;
		default:
			break;
		}

		//EDITOR
		Rect editSpace = space;
		editSpace.yMin = toolbarSpace.yMax;
		editSpace = editSpace.WithInsidePadding (2f);
		GUI.Box (editSpace, "");

		if (currentItem != null) {
			
			//Adjust grid size based on window size
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

			//Item preview rect
			Rect itemPreviewSpace = new Rect(editSpace.x + 10f, editSpace.y + 25f, currentItem.width * gridSquareSize, currentItem.height*gridSquareSize);

			//Only draw if currentItem.runtimeRepresentation has been selected
			if (currentItem.runtimeRepresentation != null) {
				
				//Name header
				Rect itemNameSpace = editSpace;
				itemNameSpace.y += 5;
				Util.HeaderLabel (itemNameSpace, "Now editing '"+currentItem.name+"'", GUIStyles.ItemNameStyle, Color.grey);

				//TODO: Automatically render and draw item icon, even if it doesn't exist
				//If a texture doesn't already exist, fetch the standard AssetPreview texture
				if (currentItem.iconTexture != null) {
					EditorGUI.DrawTextureTransparent(itemPreviewSpace, (Texture)currentItem.iconTexture);
					//TODO: Include into texture and do automatic slicing
					GUI.DrawTexture (itemPreviewSpace, (Texture)currentItem.iconBorder.texture);//, ScaleMode.ScaleToFit);

					//Select prefab by clicking in the icon
					Event e = Event.current;
					if (itemPreviewSpace.Contains (e.mousePosition) && e.type == EventType.mouseDown) {
						EditorGUIUtility.PingObject (currentItem.gameObject);
						Selection.activeObject = currentItem.gameObject;
					}

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
				Rect iconSettingsSpace = itemPreviewSpace;
				iconSettingsSpace.x = itemPreviewSpace.xMax + 5;
				iconSettingsSpace.xMax = space.xMax - 15;
				iconSettingsSpace.height = showIconSettings ? 130f : 15f;

				EditorGUI.DrawRect (iconSettingsSpace.WithHorizontalPadding(-2), Color.white.WithAlpha(0.5f));
				showIconSettings = EditorGUI.Foldout (iconSettingsSpace.WithHeight(15f), showIconSettings, "Icon Settings");
				if(showIconSettings){
					totalOffset += 120f;

					HandleIconEnvironment ();

					//Scale slider
					Rect cameraModeMenu = iconSettingsSpace.WithInsidePadding (2f);
					cameraModeMenu.y += 15f;
					cameraModeMenu.height = 15f;

					Rect cameraDistanceSlider = cameraModeMenu;
					cameraDistanceSlider.y += 18f;

					currentItem.iconSettings.orthographicCamera = GUI.Toolbar(cameraModeMenu, currentItem.iconSettings.orthographicCamera ? 1 : 0, new string[] {"Perspective", "Orthographic"}) == 1 ? true : false;

					EditorGUIUtility.labelWidth = 50f;
					if (currentItem.iconSettings.orthographicCamera) {
						currentItem.iconSettings.orthographicScale = EditorGUI.Slider (cameraDistanceSlider, "Distance", currentItem.iconSettings.orthographicScale, 0.1f, 10f);
					} else {
						currentItem.iconSettings.itemDistance = EditorGUI.Slider (cameraDistanceSlider, "Distance", currentItem.iconSettings.itemDistance, 0.1f, 10f);
					}
				
					//Offset editing
					Rect offsetRect = cameraDistanceSlider;
					offsetRect.y = cameraDistanceSlider.yMax + 5f;
					currentItem.iconSettings.itemOffset = EditorGUI.Vector3Field (offsetRect, "Item Offset", currentItem.iconSettings.itemOffset);

					//Rotation editing
					Rect rotationRect = offsetRect;
					rotationRect.y = offsetRect.yMax + 20f;
					currentItem.iconSettings.itemRotation = EditorGUI.Vector3Field (rotationRect, "Item Rotation", currentItem.iconSettings.itemRotation);
				}else {
					if (iconCamEnvInstance != null) {
						DestroyImmediate (iconCamEnvInstance);
					}
//					totalOffset += 20f;
				}


				//ITEM SETTINGS
				Rect itemSettingsSpace = iconSettingsSpace;
				itemSettingsSpace.y = iconSettingsSpace.yMax + 5f;
				itemSettingsSpace.height = showItemSettings ? 280f : 15f;

				EditorGUI.DrawRect (itemSettingsSpace.WithHorizontalPadding(-2), Color.white.WithAlpha(0.5f));
				showItemSettings = EditorGUI.Foldout (itemSettingsSpace.WithHeight(15f), showItemSettings, "Item Settings");
				if(showItemSettings){
					//Name field
					Rect nameField = itemSettingsSpace;
					nameField.height = 15f;
					nameField.y = itemSettingsSpace.y + 18f;
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

					EditorGUIUtility.labelWidth = 40f;
					currentItem.width = Mathf.Clamp(EditorGUI.IntField (itemDimPart, "Width", currentItem.width), 1, 20);
					itemDimPart.x += itemDimPart.width;
					currentItem.height = Mathf.Clamp(EditorGUI.IntField (itemDimPart, "Height", currentItem.height), 1, 20);
					EditorGUIUtility.labelWidth = prevLabelWidth;


					// Attributes

					Rect attributesField = itemDimFields;
					attributesField.y = itemDimFields.yMax + 5f;
//					attributesField.height = attributesField.width/2;

					string[] names = System.Enum.GetNames (typeof(AttributeType));
					int attributesPerRow = 2;

					// Handle automatic decision of attributesPerRow based on window width
					if (attributesField.width < 130) {
						attributesPerRow = 1;
					} else if (attributesField.width > 235) {
						attributesPerRow = 3;
					} else if (attributesField.width > 300) {

					}
					int rows = Mathf.CeilToInt((float)names.Length / (float)attributesPerRow);

					for (int i = 0; i < rows; i ++) {
						List<string> att = new List<string> ();
						for (int j = 0; j < attributesPerRow; j++) {
							int idx = i * attributesPerRow + j;
							if (idx < names.Length) {
								att.Add(names[idx]);
							}
						}
						Rect attributeButton = attributesField;
						attributeButton.width /= attributesPerRow;
						if (currentItem.attributes != null) {
							for (int b = 0; b < att.Count; b++) {
								AttributeType attributeType = (AttributeType)System.Enum.Parse (typeof(AttributeType), att [b]);
								List<Attribute> attributes = currentItem.attributes.Where (a => a.type == attributeType).ToList ();
								bool hasAttribute = attributes.Count > 0;
								if (GUI.Toggle (attributeButton, hasAttribute, att [b], EditorStyles.toolbarButton)) {
									if (!hasAttribute) {
										currentItem.attributes.Add (new Attribute (attributeType));
									}
								} else {
									if (hasAttribute) {
										currentItem.attributes.Remove (currentItem.attributes.First<Attribute> (a => a.type == attributeType));
									}
								}
								attributeButton.x += attributeButton.width;
							}
						}
						attributesField.y += 18f;
					}
				}


				//INVENTORY SETTINGS
				Inventory currentItemInv = currentItem.GetComponent<Inventory>();

				Rect inventorySettingsSpace = itemSettingsSpace;
				inventorySettingsSpace.y = itemSettingsSpace.yMax + 5f;
				inventorySettingsSpace.height = showInventorySettings ? 150f : 15f;

				if (currentItemInv == null) {
					inventorySettingsSpace.height = 15f;
					if (GUI.Button (inventorySettingsSpace, "Add Inventory", EditorStyles.miniButton)) {
						ItemToInventory (currentItem);
					}
				} else {
					EditorGUI.DrawRect (inventorySettingsSpace.WithHorizontalPadding (-2), Color.white.WithAlpha (0.5f));
					showInventorySettings = EditorGUI.Foldout (inventorySettingsSpace.WithHeight (15f), showInventorySettings, "InventorySettings");

					if (showInventorySettings) {

						//Inventory Scale
						Rect invDimFields = inventorySettingsSpace;
						invDimFields.height = 15f;
						invDimFields.y = inventorySettingsSpace.WithHeight(15f).yMax + 5;

						Rect invDimPart = invDimFields;
						invDimPart.width /= 2;

						float prevLabelWidth = EditorGUIUtility.labelWidth;
						EditorGUIUtility.labelWidth = 40f;
						currentItemInv.inventoryWidth = (int)Mathf.Clamp (EditorGUI.IntField (invDimPart, "Width", currentItemInv.inventoryWidth), 1, int.MaxValue);
						invDimPart.x += invDimPart.width;
						currentItemInv.inventoryHeight = (int)EditorGUI.IntField (invDimPart, "Height", currentItemInv.inventoryWidth);

						Rect textureInfo = invDimFields;
						textureInfo.y = invDimFields.yMax;
						textureInfo.height = 15f;
						EditorGUI.LabelField (textureInfo, "Textures should be square.", EditorStyles.centeredGreyMiniLabel);

						Rect bgImg = textureInfo;
						bgImg.y = textureInfo.yMax + 5f;
						EditorGUIUtility.labelWidth = 80f;
						currentItemInv.inventoryBackground = (Sprite)EditorGUI.ObjectField (bgImg, "Background", currentItemInv.inventoryBackground, typeof(Sprite), false);
						EditorGUIUtility.labelWidth = prevLabelWidth;

						//Show preview
						Rect invPrev = bgImg;
						invPrev.y = bgImg.yMax + 5;
						invPrev.height = invPrev.width;

						if (currentItemInv.inventoryBackground != null) {
							//draw texture preview
							GUI.DrawTexture (invPrev, currentItemInv.inventoryBackground.texture);

							//draw grid
							float gridSize = invPrev.width / currentItemInv.inventoryWidth;

							if (currentItemInv.spaces == null || (currentItemInv.spaces.Count != currentItemInv.inventoryWidth * currentItemInv.inventoryHeight)) {//(currentItemInv.spaces.GetLength (0) != currentItemInv.inventoryWidth || currentItemInv.spaces.GetLength (1) != currentItemInv.inventoryHeight)) {
								int capacity = currentItemInv.inventoryWidth * currentItemInv.inventoryHeight;
								currentItemInv.spaces = new List<InventorySpace> (capacity);
								for (int i = 0; i < capacity; i++) {
									currentItemInv.spaces.Add (new InventorySpace ());
								}
							}

							//TODO: Drag-draw isActive
							for (int x = 0; x < currentItemInv.inventoryWidth; x++) {
								for (int y = 0; y < currentItemInv.inventoryHeight; y++) {
									Rect square = new Rect (invPrev.x + x * gridSize, invPrev.y + y * gridSize, gridSize, gridSize);
									Event e = Event.current;
									InventorySpace curSpace = currentItemInv.spaces [y * currentItemInv.inventoryWidth + x]; //rewrite this shitty code

									if (square.Contains (e.mousePosition)) {
										EditorGUI.DrawRect (square, Color.white.WithAlpha (0.2f));
										if (e.type == EventType.mouseDown) {
											curSpace.isActive = !curSpace.isActive;
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
					}
				}
			} else {
				Rect boxSpace = editSpace;

				//Drag & drop of new item
				Event e = Event.current;
				//change mouse cursor
				if (e.type == EventType.dragUpdated) {
					DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
				}

				if (boxSpace.Contains (e.mousePosition) && DragAndDrop.objectReferences.Length > 0) {
					EditorGUI.HelpBox (boxSpace, "Release left mouse button to drop the item as the graphical representation of this object!", MessageType.Info);

					if (e.type == EventType.DragPerform) {
						Object[] dragObjs = DragAndDrop.objectReferences;

						for(int i=0; i<dragObjs.Length; i++){
							if (dragObjs [i] is GameObject) {
								currentItem.runtimeRepresentation = Instantiate(dragObjs [i], currentItem.gameObject.transform) as GameObject;
								currentItem.runtimeRepresentation.name = "Graphics";
								SetItemDirty ();
								break;
							}
						}
						DragAndDrop.AcceptDrag ();
					}
				} else {
					EditorGUI.HelpBox (boxSpace, "No graphical representation selected! Drag & Drop an object into this box!", MessageType.Warning);
				}
			}

		} else {
			EditorGUI.HelpBox (editSpace.WithInsidePadding(5f), "No Item selected! Create a new one, or open an existing one before continuing.", MessageType.Error);
		}

	}

	void CheckItemDirty(){
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
	}

	void SetItemDirty(){
		itemIsDirty = true;
	}

	void UpdateIconTexture(){
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

	void GenerateSpriteAndTexture(){
		Sprite icon = Sprite.Create (currentItem.iconTexture, new Rect (0, 0, currentItem.iconTexture.width, currentItem.iconTexture.height), new Vector2 (0.5f, 0.5f));
		icon.name = currentItem.name + "_icon";
		currentItem.icon = icon;

		SaveAsAsset (icon, currentItem, icon.name);
		AssetDatabase.AddObjectToAsset (currentItem.iconTexture, icon);

		// Add to prefab
		// Create an instance of the prefab
		GameObject prefabInstance = PrefabUtility.InstantiatePrefab(currentItem.gameObject) as GameObject;

		// Check to see if the prefab already has an 'Icon' object
		Transform existingIcon = prefabInstance.transform.Find("Icon");
		if (existingIcon != null) {
			DestroyImmediate (existingIcon.gameObject);
		}

		// Icon
		GameObject icn = new GameObject("Icon");
		icn.transform.parent = prefabInstance.transform;
		icn.transform.localPosition = Vector3.zero;
		UnityEngine.UI.Image img = icn.AddComponent<UnityEngine.UI.Image> ();
		img.sprite = currentItem.icon;
		img.raycastTarget = false;

		// Icon Border
		GameObject border = new GameObject("Icon_border");
		border.transform.parent = icn.transform;
		border.transform.localPosition = Vector3.zero;
		img = border.AddComponent<UnityEngine.UI.Image> ();
		img.sprite = currentItem.iconBorder;
		img.raycastTarget = false;

		// Apply to the prefab
		PrefabUtility.ReplacePrefab(prefabInstance, PrefabUtility.GetPrefabParent(prefabInstance), ReplacePrefabOptions.ConnectToPrefab);

		// Then destroy the prefab instance
		DestroyImmediate(prefabInstance);

		SetItemDirty ();
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

	void HandleIconEnvironment(){
		//First make sure that the current item has IconSettings
		if (currentItem.iconSettings == null) {
			currentItem.iconSettings = new IconSettings (Vector3.zero, Vector3.zero, true, 5f);
		}

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
			if (envScript == null) {
				envScript = iconCamEnvInstance.GetComponent<ItemIconEditorEnvironment> ();
			}

			// Initialize the snapshot environment (
			envScript.Init (currentItem);

			CheckItemDirty ();

			//Update the render texture (only when item is marked as dirty)
			if (Event.current.type == EventType.repaint && itemIsDirty) {
				UpdateIconTexture ();
			}
		}
	}

	void ItemToInventory (Item item){
		//Create inventory and copy all settings
		Inventory inventory = item.gameObject.AddComponent<Inventory>();
		inventory.spaces = new List<InventorySpace>(1);
		inventory.spaces.Add(new InventorySpace ());
		inventory.connectedItem = item;
		inventory.availableSpace = inventory.inventoryWidth * inventory.inventoryHeight;

	}
//
//	void InventoryToItem(Inventory item){
//		Item newItem = Item.CreateInstance<Item> ();
//		newItem.width = item.width;
//		newItem.height = item.height;
//		newItem.iconSettings = item.iconSettings;
//		newItem.iconTexture = item.iconTexture;
//		newItem.icon = item.icon;
//		newItem.flavorText = item.flavorText;
//		newItem.name = item.name;
//		newItem.runtimeRepresentation = item.runtimeRepresentation;
//
//		string path = AssetDatabase.GetAssetPath (item);
//		AssetDatabase.CreateAsset (newItem, path);
//		currentItem = newItem;
//	}

	void LoadItem(){
		EditorUtility.DisplayDialog ("Whoops...", "This feature is not implemented yet. Instead, select a GameObject with an Item component in the project view", "xD");
	}

	void CreateNewItem(){ //TODO: Create prefab instead of ScriptableObject
		
		string path = EditorUtility.SaveFilePanelInProject ("Create a new Item", "New Item", "prefab", "Select the desired destination folder.");
//
		//Make sure a path was selected
		if (path != "") {

			string[] folderNames = path.Split(new char[]{'/'}, System.StringSplitOptions.None); // Get the chosen path
			string itemName = folderNames[folderNames.Length-1].Substring(0, folderNames[folderNames.Length-1].Length-7); // Find the entered name (last part of path -7 for '.prefab')

			if (!AssetDatabase.IsValidFolder (path.Substring (0, path.Length - 7))) { // If top-level item folder doesn't already exist, then create it
				string folderPath = path.Substring (0, path.Length - folderNames [folderNames.Length - 1].Length-1); Debug.Log (folderPath);
				string newFolderGUID = AssetDatabase.CreateFolder (folderPath, itemName);
				path = AssetDatabase.GUIDToAssetPath (newFolderGUID) + "/" + folderNames[folderNames.Length-1];
			}


			currentItemObj = PrefabUtility.CreatePrefab (path, new GameObject (itemName));
			currentItem = currentItemObj.AddComponent<Item>();
			currentItem.name = itemName;

//			Item newItem = Item.CreateInstance<Item> ();
//
//			//make new folder if it doesn't exist
//			string[] folderNames = path.Split(new char[]{'/'}, System.StringSplitOptions.None);
//			string itemName = folderNames[folderNames.Length-1].Substring(0, folderNames[folderNames.Length-1].Length-6);
//			if (!AssetDatabase.IsValidFolder (path.Substring (0, path.Length - 6))) {
//				string folderPath = path.Substring (0, path.Length - folderNames [folderNames.Length - 1].Length-1); Debug.Log (folderPath);
//				string newFolderGUID = AssetDatabase.CreateFolder (folderPath, itemName);
//				path = AssetDatabase.GUIDToAssetPath (newFolderGUID) + "/" + folderNames[folderNames.Length-1];
//			}
//			AssetDatabase.CreateAsset (newItem, path);
//			newItem.name = AssetDatabase.LoadAssetAtPath (path,typeof(Item)).name;
//
//			currentItem = newItem;
//
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
