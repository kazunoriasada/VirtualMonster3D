using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

[Serializable]
public class MAST_Interface : EditorWindow
{
#if (UNITY_EDITOR)
    
    // ---------------------------------------------------------------------------
    // Add menu named "Open MAST Palette" to the Window menu
    // ---------------------------------------------------------------------------
    [MenuItem("Tools/MAST/Open MAST Window", false, 16)]
    private static void ShowWindow()
    {
        // Get existing open window or if none, make a new one:
        EditorWindow.GetWindow(typeof(MAST_Interface)).Show();
    }
    
// ---------------------------------------------------------------------------
#region Variable Declaration
// ---------------------------------------------------------------------------
    
    // Initialize Hotkeys Class if needed and return HotKeysClass
    [SerializeField] private MAST_Hotkeys HotKeysClass;
    private MAST_Hotkeys HotKeys
    {
        get
        {
            if(HotKeysClass == null)
                HotKeysClass = new MAST_Hotkeys();
            return HotKeysClass;
        }
    }
    
    [SerializeField] private MAST_Settings_Interface Settings;
    [SerializeField] private MAST_Tools_Interface Tools;
    
    // ------------------------------
    // Image Variables
    // ------------------------------
    [SerializeField] private Texture2D iconGridToggle;
    [SerializeField] private Texture2D iconGridUp;
    [SerializeField] private Texture2D iconGridDown;
    [SerializeField] private Texture2D[] iconDrawTool;
    [SerializeField] private Texture2D iconRotate;
    [SerializeField] private Texture2D iconFlip;
    [SerializeField] private Texture2D iconAxisX;
    [SerializeField] private Texture2D iconAxisY;
    [SerializeField] private Texture2D iconAxisZ;
    [SerializeField] private Texture2D iconLoadFromFolder;
    [SerializeField] private Texture2D iconSettings;
    [SerializeField] private Texture2D iconTools;
    
    // ------------------------------
    // GUIStyle Variables
    // ------------------------------
    [SerializeField] public GUISkin guiSkin;
    
    // ------------------------------
    // Palette Scrollview Variables
    // ------------------------------
    [SerializeField] const int scrollBarWidth = 19;  // Subtracted from scroll area width or height when calculating visible area
    [SerializeField] private Vector2 scrollPos = new Vector2();  // Current scroll position
    
    // ------------------------------
    // Palette Variables
    // ------------------------------
    [SerializeField] public float toolBarIconSize;
    [SerializeField] private GUIContent[] paletteItemContent;
    [SerializeField] private string[] paletteItemName;
    [SerializeField] private int paletteColumnCount = 3;
    [SerializeField] private bool showPaletteSettings;
    
    // ------------------------------
    // Draw Tool Variables
    // ------------------------------
    [SerializeField] private bool drawing = false;
    [SerializeField] private bool painting = false;
    [SerializeField] private bool erasing = false;
    
    [SerializeField] private bool editorPlaying;
    
#endregion
    
    // ---------------------------------------------------------------------------
    // Perform Initializations
    // ---------------------------------------------------------------------------
    void Awake() // This runs on the first time open
    {
        //Debug.Log("Interface - Awake");
    }
    
    void OnFocus()
    {
        //Debug.Log("Interface - On Focus");
        
        //SceneView.duringSceneGui -= this.OnScene; // Don't add twice
        //SceneView.duringSceneGui += this.OnScene;
    }
    
    // ---------------------------------------------------------------------------
    // MAST Window is Enabled
    // ---------------------------------------------------------------------------
    private void OnEnable()
    {
        //Debug.Log("Interface - On Enable");
        
        // Load icon textures
        LoadImages();
        
        // Load custom gui styles
        guiSkin = MAST_Asset_Loader.GetGUISkin();
        
        // Initialize Preference Manager
        MAST_Settings.Initialize();
        
        // Set up deletegates so that OnScene is called automatically
        SceneView.duringSceneGui -= this.OnScene;
        SceneView.duringSceneGui += this.OnScene;
        
        // Set up deletegates to capture when scene is loaded
        EditorSceneManager.sceneLoaded -= this.OnSceneLoaded;
        EditorSceneManager.sceneLoaded += this.OnSceneLoaded;
        
        // Set scene to be updated by mousemovement
        wantsMouseMove = true;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("Interface - On Scene Loaded");
        
        editorPlaying = true;
        
        CleanUpInterface();
    }
    
    private void CleanUpInterface()
    {
        // Delete placement grid
        MAST_Grid_Manager.DestroyGrid();
        
        // Deselect palette item and delete visualizer
        MAST_Settings.gui.palette.selectedItemIndex = -1;
        MAST_Placement_Visualizer.RemoveVisualizer();
        
        // Deselect draw tool and change placement mode to none
        MAST_Settings.gui.toolbar.selectedDrawToolIndex = -1;
        MAST_Placement_Interface.ChangePlacementMode(MAST_Placement_Interface.PlacementMode.None);
        
        // Cancel any drawing or painting
        drawing = false;
        painting = false;
        erasing = false;
        MAST_Placement_PaintArea.DeletePaintArea();
        
    }
    
    // ---------------------------------------------------------------------------
    // Perform Maintenance when MAST Window is Disabled
    // ---------------------------------------------------------------------------
    [RuntimeInitializeOnLoadMethod]
    private void OnDisable()
    {
        //Debug.Log("Interface - On Disable");
        
        MAST_Settings.Save_Settings();
        SceneView.duringSceneGui -= this.OnScene;
        
        CleanUpInterface();
    }
    
    // ---------------------------------------------------------------------------
    // Perform Cleanup when MAST Window is Closed
    // ---------------------------------------------------------------------------
    private void OnDestroy()
    {
        //Debug.Log("Interface - On Destroy");
        
        SceneView.duringSceneGui -= this.OnScene;
        
        CleanUpInterface();
    }
    
    // ---------------------------------------------------------------------------
    // Runs every frame
    // ---------------------------------------------------------------------------
    private void Update()
    {
        // -----------------------------------------------------------------------
        // Detect when the scene is first stopped
        // -----------------------------------------------------------------------
        
        // If editor is not running the application
        // This method is used when not using MaximizeOnPlay, since editor window never closes
        if (!EditorApplication.isPlaying)
        {
            // If editor playing variable was set to true
            if (editorPlaying)
            {
                // Destroy any remaining grids or visualizers
                MAST_Grid_Manager.DestroyGrid();
                MAST_Placement_Visualizer.RemoveVisualizer();
                
                // Set editor playing to false so this only runs once
                editorPlaying = false;
            }
        }
        
        // If Settings window is open and changes to UI value ocurred,
        // update the grid and repaint all views incase needed
        if (Settings != null)
        {
            if (Settings.PreferencesChanged ())
            {
                MAST_Grid_Manager.UpdateGridSettings();
                UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
            }
        }
    }
    
// ---------------------------------------------------------------------------
#region Load Images
// ---------------------------------------------------------------------------
    private void LoadImages()
    {
        iconGridToggle = MAST_Asset_Loader.GetImage("Grid_Toggle.png");
        iconGridUp = MAST_Asset_Loader.GetImage("Grid_Up.png");
        iconGridDown = MAST_Asset_Loader.GetImage("Grid_Down.png");
        
        iconDrawTool = new Texture2D[5];
        iconDrawTool[0] = MAST_Asset_Loader.GetImage("Pencil.png");
        iconDrawTool[1] = MAST_Asset_Loader.GetImage("Paint_Roller.png");
        iconDrawTool[2] = MAST_Asset_Loader.GetImage("Paint_Bucket.png");
        iconDrawTool[3] = MAST_Asset_Loader.GetImage("Randomizer.png");
        iconDrawTool[4] = MAST_Asset_Loader.GetImage("Eraser.png");
        
        iconRotate = MAST_Asset_Loader.GetImage("Rotate.png");
        iconFlip = MAST_Asset_Loader.GetImage("Flip.png");
        iconAxisX = MAST_Asset_Loader.GetImage("Axis_X.png");
        iconAxisY = MAST_Asset_Loader.GetImage("Axis_Y.png");
        iconAxisZ = MAST_Asset_Loader.GetImage("Axis_Z.png");
        iconLoadFromFolder = MAST_Asset_Loader.GetImage("Load_From_Folder.png");
        iconSettings = MAST_Asset_Loader.GetImage("Settings.png");
        iconTools = MAST_Asset_Loader.GetImage("Tools.png");
    }
#endregion
    
// ---------------------------------------------------------------------------
#region SceneView
// ---------------------------------------------------------------------------
    private void OnScene(SceneView sceneView)
    {
        // Handle SceneView GUI
        SceneviewGUI(sceneView);
        
        // Handle view focus
        CheckMouseEnterLeaveSceneView();
        
        // Process HotKeys and repaint all views if any changes were made
        if (HotKeys.ProcessHotkeys())
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        
        // If draw tool was deselected by a hotkey
        if (MAST_Settings.gui.toolbar.selectedDrawToolIndex != 1)
            // Stop any drawing
            if (drawing)
                drawing = false;
        
        // If paint area tool was deselected by a hotkey
        if (MAST_Settings.gui.toolbar.selectedDrawToolIndex != 2)
            // Stop any painting
            if (painting)
            {
                MAST_Placement_PaintArea.DeletePaintArea();
                painting = false;
            }
        
        // If erase tool was deselected by a hotkey
        if (MAST_Settings.gui.toolbar.selectedDrawToolIndex != 4)
            // Stop any erasing
            if (erasing)
                erasing = false;
        
        // If a palette item is selected or erase tool is selected, handle object placement
        if (MAST_Settings.gui.palette.selectedItemIndex > -1 ||
            MAST_Settings.gui.toolbar.selectedDrawToolIndex == 4)
            ObjectPlacement();
    }
    
    // Handle SceneView GUI
    private void SceneviewGUI(SceneView sceneView)
    {
        
    }
    
    // Handle events when mouse point enter or leaves the SceneView
    void CheckMouseEnterLeaveSceneView()
    {
        // If mouse enters SceneView window, show visualizer
        if (Event.current.type == EventType.MouseEnterWindow)
            MAST_Placement_Visualizer.SetVisualizerVisibility(true);
        
        // If mouse leaves SceneView window
        else if (Event.current.type == EventType.MouseLeaveWindow)
        {
            // Hide visualizer
            MAST_Placement_Visualizer.SetVisualizerVisibility(false);
            
            // Stop any drawing
            if (drawing)
                drawing = false;
            
            // Stop any painting
            if (painting)
            {
                MAST_Placement_PaintArea.DeletePaintArea();
                painting = false;
            }
            
            // Stop any erasing
            if (erasing)
                erasing = false;
        }
    }
    
    // Handle object placement
    private void ObjectPlacement()
    {
        // Get mouse events for object placement when in the Scene View
        Event currentEvent = Event.current;
        
        // Change position of visualizer
        MAST_Placement_Visualizer.UpdateVisualizerPosition();
        
        switch (MAST_Settings.gui.toolbar.selectedDrawToolIndex)
        {
            // Draw single tool
            case 0:
            // Randomizer tool
            case 3:
                // If left mouse button was clicked
                if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0)
                {
                    // Keep mouseclick from selecting other objects
                    GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);
                    Event.current.Use();
                    
                    // Place selected prefab on grid
                    MAST_Placement_Place.PlacePrefabInScene();
                }
                break;
                
            // Draw continuous tool
            case 1:
                // If not already drawing and the left mouse button pressed
                if (!drawing)
                    if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0)
                    {
                        // Start drawing
                        drawing = true;
                        
                        // Keep mouseclick from selecting other objects
                        GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);
                        Event.current.Use();
                    }
                
                // If drawing and left mouse button not released
                if (drawing)
                {
                    // Place selected prefab on grid
                    MAST_Placement_Place.PlacePrefabInScene();
                    
                    // If left mouse button released
                    if (currentEvent.type == EventType.MouseUp && currentEvent.button == 0)
                        drawing = false;
                }
                break;
                
            // Paint area tool
            case 2:
                // If not already painting and the left mouse button pressed
                if (!painting)
                    if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0)
                    {
                        // Start drawing
                        painting = true;
                        
                        // Start paint area at current mouse location
                        MAST_Placement_PaintArea.StartPaintArea();
                        
                        // Keep mouseclick from selecting other objects
                        GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);
                        Event.current.Use();
                    }
                
                // If drawing and left mouse button not released
                if (painting)
                {
                    // Update the paint area as the mouse moves
                    MAST_Placement_PaintArea.UpdatePaintArea();
                    
                    // If left mouse button released
                    if (currentEvent.type == EventType.MouseUp && currentEvent.button == 0)
                    {
                        MAST_Placement_PaintArea.CompletePaintArea();
                        painting = false;
                    }
                }
                break;
            
            // Erase tool
            case 4:
                // If not already erasing and the left mouse button pressed
                if (!erasing)
                    if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0)
                    {
                        // Start drawing
                        erasing = true;
                        
                        // Keep mouseclick from selecting other objects
                        GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);
                        Event.current.Use();
                    }
                
                // If erasing and left mouse button not released
                if (erasing)
                {
                    // Place selected prefab on grid
                    MAST_Placement_Interface.ErasePrefab();
                    
                    // If left mouse button released
                    if (currentEvent.type == EventType.MouseUp && currentEvent.button == 0)
                        erasing = false;
                }
                break;
        }
        
    }
#endregion
    
// ---------------------------------------------------------------------------
#region Custom Editor Window Interface
// ---------------------------------------------------------------------------
    void OnGUI()
    {
        // Load custom skin
        GUI.skin = guiSkin;
        
        GUILayout.BeginHorizontal("MAST Toolbar BG");  // Begin entire window horizontal layout
        
        // If toolbar is on the left
        if (MAST_Settings.gui.toolbar.position == MAST_GUI_ScriptableObject.ToolbarPos.Left)
        {
            DisplayToolbarGUI();
            DisplayPaletteGUI();
        }
        
        // If toolbar is on the right
        else
        {
            DisplayPaletteGUI();
            DisplayToolbarGUI();
        }
        
        GUILayout.EndHorizontal(); // End of entire window horizontal layout
        
        // ----------------------------------------------
        // Redraw MAST window if mouse is moved
        // ----------------------------------------------
        if (Event.current.type == EventType.MouseMove)
            Repaint();
        
        // ----------------------------------------------
        // Process Hotkeys
        // ----------------------------------------------
        if (HotKeys.ProcessHotkeys())
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
    }
#endregion
    
// ---------------------------------------------------------------------------
#region Toolbar GUI
// ---------------------------------------------------------------------------
    private void DisplayToolbarGUI()
    {
        // Calculate toolbar icon size
        toolBarIconSize = position.height / 15.3f;
        
        GUILayout.Space(toolBarIconSize / 10);
        
        GUILayout.BeginVertical();
        
        GUILayout.Space(toolBarIconSize / 7.5f);
        
        // ----------------------------------------------
        // Grid Section
        // ----------------------------------------------
        GUILayout.BeginVertical("MAST Toolbar BG Inset");
        
        // ------------------------------------
        // Grid Up Button
        // ------------------------------------
        if (GUILayout.Button(new GUIContent(iconGridUp, "Move Grid Up"),
            "MAST Half Button", GUILayout.Width(toolBarIconSize), GUILayout.Height(toolBarIconSize / 2)))
        {
            MAST_Grid_Manager.MoveGridUp();
        }
        
        // ------------------------------------
        // Toggle Grid Button
        // ------------------------------------
        EditorGUI.BeginChangeCheck ();
        
        // OnScene Enable/Disable Randomizer Icon Button
        MAST_Grid_Manager.gridExists = GUILayout.Toggle(
            MAST_Grid_Manager.gridExists,
            new GUIContent(iconGridToggle, "Toggle Scene Grid"),
            "MAST Toggle", GUILayout.Width(toolBarIconSize), GUILayout.Height(toolBarIconSize));
        
        // If randomizer enabled value changed, process the change
        if (EditorGUI.EndChangeCheck ())
            MAST_Grid_Manager.ChangeGridVisibility();
        
        // ------------------------------------
        // Grid Down Button
        // ------------------------------------
        if (GUILayout.Button(new GUIContent(iconGridDown, "Move Grid Down"),
            "MAST Half Button", GUILayout.Width(toolBarIconSize), GUILayout.Height(toolBarIconSize / 2)))
        {
            MAST_Grid_Manager.MoveGridDown();
        }
                
        GUILayout.EndVertical();
        
        //GUILayout.Box("", "MAST Separator", GUILayout.Width(toolBarIconSize), GUILayout.Height(toolBarIconSize / 10));
        
        GUILayout.Space(toolBarIconSize / 5);
        
        // ----------------------------------------------
        // Draw Tool Section
        // ----------------------------------------------
        GUILayout.BeginVertical("MAST Toolbar BG Inset");
        
        // ------------------------------------
        // Add Draw Tool Toggle Group
        // ------------------------------------
        EditorGUI.BeginChangeCheck ();
        
        // Create drawtools SelectionGrid
        int newSelectedDrawToolIndex = GUILayout.SelectionGrid(
            MAST_Settings.gui.toolbar.selectedDrawToolIndex, 
            iconDrawTool, 1, "MAST Toggle",
            GUILayout.Width(toolBarIconSize), 
            GUILayout.Height(toolBarIconSize * 5));
        
        // If the draw tool was changed
        if (EditorGUI.EndChangeCheck ()) {
            
            // If the draw tool was clicked again, deselect it
            if (newSelectedDrawToolIndex == MAST_Settings.gui.toolbar.selectedDrawToolIndex)
            {
                MAST_Settings.gui.toolbar.selectedDrawToolIndex = -1;
                
                MAST_Placement_Interface.ChangePlacementMode(MAST_Placement_Interface.PlacementMode.None);
                
                drawing = false;
                painting = false;
                erasing = false;
            }
            
            // If a different draw tool was clicked, change to it
            else
            {
                MAST_Settings.gui.toolbar.selectedDrawToolIndex = newSelectedDrawToolIndex;
                
                if (MAST_Settings.gui.toolbar.selectedDrawToolIndex != 1)
                {
                    drawing = false;
                }
                if (MAST_Settings.gui.toolbar.selectedDrawToolIndex != 2)
                {
                    painting = false;
                }
                if (MAST_Settings.gui.toolbar.selectedDrawToolIndex != 4)
                {
                    erasing = false;
                }
                
                switch (MAST_Settings.gui.toolbar.selectedDrawToolIndex)
                {
                    // Draw Single Tool selected
                    case 0:
                        MAST_Placement_Interface.ChangePlacementMode(MAST_Placement_Interface.PlacementMode.DrawSingle);
                        break;
                    
                    // Draw Continuous Tool selected
                    case 1:
                        MAST_Placement_Interface.ChangePlacementMode(MAST_Placement_Interface.PlacementMode.DrawContinuous);
                        break;
                    
                    // Flood Fill Tool selected
                    case 2:
                        MAST_Placement_Interface.ChangePlacementMode(MAST_Placement_Interface.PlacementMode.PaintArea);
                        break;
                    
                    // Randomizer Tool selected
                    case 3:
                        MAST_Placement_Interface.ChangePlacementMode(MAST_Placement_Interface.PlacementMode.Randomize);
                        SceneView.RepaintAll();
                        break;
                    
                    // Eraser Tool selected
                    case 4:
                        MAST_Placement_Interface.ChangePlacementMode(MAST_Placement_Interface.PlacementMode.Erase);
                        SceneView.RepaintAll();
                        break;
                }
                
            }
        }
        
        GUILayout.EndVertical();
        
        GUILayout.Space(toolBarIconSize / 5);
        
        // ----------------------------------------------
        // Manipulate Section
        // ----------------------------------------------
        GUILayout.BeginVertical("MAST Toolbar BG Inset");
        
        // ------------------------------------
        // Rotate Button
        // ------------------------------------
        if (GUILayout.Button(new GUIContent(iconRotate, "Rotate Prefab/Selection"),
            "MAST Button", GUILayout.Width(toolBarIconSize), GUILayout.Height(toolBarIconSize)))
        {
            MAST_Placement_Manipulate.RotateObject();
        }
        
        // OnScene Change Rotate Axis Icon Button
        switch (MAST_Placement_Manipulate.GetCurrentRotateAxis())
        {
            case MAST_Placement_Manipulate.Axis.X:
                if (GUILayout.Button(new GUIContent(iconAxisX, "Change Rotate Axis"),
                    "MAST Half Button", GUILayout.Width(toolBarIconSize), GUILayout.Height(toolBarIconSize / 2)))
                    MAST_Placement_Manipulate.ToggleRotateAxis();
                break;
            case MAST_Placement_Manipulate.Axis.Y:
                if (GUILayout.Button(new GUIContent(iconAxisY, "Change Rotate Axis"),
                    "MAST Half Button", GUILayout.Width(toolBarIconSize), GUILayout.Height(toolBarIconSize / 2)))
                    MAST_Placement_Manipulate.ToggleRotateAxis();
                break;
            case MAST_Placement_Manipulate.Axis.Z:
                if (GUILayout.Button(new GUIContent(iconAxisZ, "Change Rotate Axis"),
                    "MAST Half Button", GUILayout.Width(toolBarIconSize), GUILayout.Height(toolBarIconSize / 2)))
                    MAST_Placement_Manipulate.ToggleRotateAxis();
                break;
        }
        
        GUILayout.Space(toolBarIconSize / 10);
        
        // ------------------------------------
        // Flip Button
        // ------------------------------------
        if (GUILayout.Button(new GUIContent(iconFlip, "Flip Prefab/Selection"), 
            "MAST Button", GUILayout.Width(toolBarIconSize), GUILayout.Height(toolBarIconSize)))
        {
            MAST_Placement_Manipulate.FlipObject();
        }
        
        // OnScene Change Flip Axis Icon Button
        switch (MAST_Placement_Manipulate.GetCurrentFlipAxis())
        {
            case MAST_Placement_Manipulate.Axis.X:
                if (GUILayout.Button(new GUIContent(iconAxisX, "Change Flip Axis"),
                    "MAST Half Button", GUILayout.Width(toolBarIconSize), GUILayout.Height(toolBarIconSize / 2)))
                    MAST_Placement_Manipulate.ToggleFlipAxis();
                break;
            case MAST_Placement_Manipulate.Axis.Y:
                if (GUILayout.Button(new GUIContent(iconAxisY, "Change Flip Axis"),
                    "MAST Half Button", GUILayout.Width(toolBarIconSize), GUILayout.Height(toolBarIconSize / 2)))
                    MAST_Placement_Manipulate.ToggleFlipAxis();
                break;
            case MAST_Placement_Manipulate.Axis.Z:
                if (GUILayout.Button(new GUIContent(iconAxisZ, "Change Flip Axis"),
                    "MAST Half Button", GUILayout.Width(toolBarIconSize), GUILayout.Height(toolBarIconSize / 2)))
                    MAST_Placement_Manipulate.ToggleFlipAxis();
                break;
        }
        
        GUILayout.EndVertical();
        
        GUILayout.Space(toolBarIconSize / 5);
        
        // ----------------------------------------------
        // Misc Section
        // ----------------------------------------------
        GUILayout.BeginVertical("MAST Toolbar BG Inset");
        
        // ------------------------------------
        // Load Palette Button
        // ------------------------------------
        if (GUILayout.Button(new GUIContent(iconLoadFromFolder, "Load prefabs from the selected project folder"),
            "MAST Button", GUILayout.Width(toolBarIconSize), GUILayout.Height(toolBarIconSize)))
        {
            string path = MAST_Asset_Loader.GetPathOfSelectedFolder();
            if (EditorUtility.DisplayDialog("Are you sure?",
                "Load prefabs from '" + MAST_Asset_Loader.GetPathOfSelectedFolder() + "'?",
                "Load Prefabs", "Cancel"))
            {
                LoadPaletteItemContent();
            }
        }
        
        // ------------------------------------
        // Open Tools Window Button
        // ------------------------------------
        if (GUILayout.Button(new GUIContent(iconTools, "Tools"),
            "MAST Button", GUILayout.Width(toolBarIconSize), GUILayout.Height(toolBarIconSize)))
        {
            // If Tools window is closed, show and initialize it
            if (Tools == null)
            {
                Tools = (MAST_Tools_Interface)EditorWindow.GetWindow(
                    typeof(MAST_Tools_Interface),
                    true, "MAST Tools");
                
                Tools.minSize = Tools.maxSize = new Vector2(330, 300);
            }
            
            // If Tools window is open, close it
            else
            {
                EditorWindow.GetWindow(typeof(MAST_Tools_Interface)).Close();
            }
        }
        
        // ------------------------------------
        // Open Settings Window Button
        // ------------------------------------
        if (GUILayout.Button(new GUIContent(iconSettings, "Settings"),
            "MAST Button", GUILayout.Width(toolBarIconSize), GUILayout.Height(toolBarIconSize)))
        {
            // If Settings window is closed, show and initialize it
            if (Settings == null)
            {
                Settings = (MAST_Settings_Interface)EditorWindow.GetWindow(
                    typeof(MAST_Settings_Interface),
                    true, "MAST Settings");
                
                Settings.minSize = Settings.maxSize = new Vector2(330, 300);
                Settings.Initialize();
            }
            
            // If Settings window is open, close it
            else
            {
                EditorWindow.GetWindow(typeof(MAST_Settings_Interface)).Close();
            }
        }
        
        GUILayout.EndVertical();
        
        GUILayout.EndVertical();
    }
#endregion
    
// ---------------------------------------------------------------------------
#region Palette GUI
// ---------------------------------------------------------------------------
    private void DisplayPaletteGUI()
    {
        // Only draw prefab palette if it exists
        if (paletteItemContent != null && paletteItemContent.Length > 0)
            DisplayPaletteGUIPopulated();
        else
            DisplayPaletteGUIPlaceholder();
    }
    
    private void DisplayPaletteGUIPlaceholder()
    {
        GUILayout.BeginVertical("MAST Toolbar BG");
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal();
        GUILayout.Space(5f);
        EditorGUILayout.LabelField(
            "No prefabs to display!  Select your prefabs folder and click Load Prefabs",
            EditorStyles.wordWrappedLabel);
        GUILayout.Space(5f);
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
    }
    
    private void DisplayPaletteGUIPopulated()
    {
        GUILayout.BeginVertical("MAST Toolbar BG");  // Begin toolbar vertical layout
        
        GUILayout.BeginHorizontal();
        
        // ---------------------------------------------
        // Calculate Palette SelectionGrid size
        // ---------------------------------------------
        
        // Verical scroll view for palette items
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        
        // Get scrollview width and height of scrollview if is resized
        float scrollViewWidth = EditorGUIUtility.currentViewWidth - scrollBarWidth - toolBarIconSize - 20;
        
        int rowCount = Mathf.CeilToInt(paletteItemContent.Length / (float)paletteColumnCount);
        float scrollViewHeight = rowCount * ((scrollViewWidth) / paletteColumnCount);
        
        // ---------------------------------------------
        // Get palette background image
        // ---------------------------------------------
        string paletteGUISkin = null;
        
        switch (MAST_Settings.gui.palette.bgColor)
        {
            case MAST_GUI_ScriptableObject.PaleteBGColor.Dark:
                paletteGUISkin = "MAST Palette Item Dark";
                break;
            case MAST_GUI_ScriptableObject.PaleteBGColor.Gray:
                paletteGUISkin = "MAST Palette Item Gray";
                break;
            case MAST_GUI_ScriptableObject.PaleteBGColor.Light:
                paletteGUISkin = "MAST Palette Item Light";
                break;
        }
        
        EditorGUI.BeginChangeCheck ();
        
        // ---------------------------------------------
        // Draw Palette SelectionGrid
        // ---------------------------------------------
        
        int newSelectedPaletteItemIndex = GUILayout.SelectionGrid(
            MAST_Settings.gui.palette.selectedItemIndex, 
            paletteItemContent, 
            paletteColumnCount,
            paletteGUISkin,
            GUILayout.Width((float)scrollViewWidth), 
            GUILayout.Height(scrollViewHeight)
            );
        
        // If changes to UI value ocurred, update the grid
        if (EditorGUI.EndChangeCheck ()) {
            
            // If palette item was deselected by being clicked again
            if (newSelectedPaletteItemIndex == MAST_Settings.gui.palette.selectedItemIndex)
            {
                MAST_Settings.gui.palette.selectedItemIndex = -1;
                
                // If erase draw tool isn't selected, remove the visualizer
                if (MAST_Settings.gui.toolbar.selectedDrawToolIndex != 4)
                    MAST_Placement_Visualizer.RemoveVisualizer();
            }
            
            // If palette item selection has changed
            else
            {
                MAST_Settings.gui.palette.selectedItemIndex = newSelectedPaletteItemIndex;
                
                // If no draw tool is selected, then select the draw single tool
                if (MAST_Settings.gui.toolbar.selectedDrawToolIndex == -1)
                {
                    MAST_Settings.gui.toolbar.selectedDrawToolIndex = 0;
                    MAST_Placement_Interface.ChangePlacementMode(MAST_Placement_Interface.PlacementMode.DrawSingle);
                }
                
                // If erase draw tool isn't selected, change the visualizer prefab
                if (MAST_Settings.gui.toolbar.selectedDrawToolIndex != 4)
                    MAST_Placement_Interface.ChangeSelectedPrefab();
            }
        }
        
        EditorGUILayout.EndScrollView();
        
        GUILayout.EndHorizontal();
        
        // Palette Column Count Slider
        paletteColumnCount = (int)GUILayout.HorizontalSlider(paletteColumnCount, 1, 10);
        
GUILayout.Space(toolBarIconSize / 10);
        
        GUILayout.EndVertical();
    }
    
    // ---------------------------------------------------------------------------
    // Generate Palette Item Content
    // ---------------------------------------------------------------------------
    private void LoadPaletteItemContent()
    {
        //palettePrefabObjects = MAST_Asset_Loader.GetPrefabsInSelectedFolder(false);
        MAST_Settings.gui.palette.prefab = MAST_Asset_Loader.GetPrefabsInSelectedFolder(false);
        
        // Increase asset preview cache to allow more thumbnails to be generated at once
        AssetPreview.SetPreviewTextureCacheSize(500);
        
        // Initialize and make the paletteItemIcon array the same size as the paletteItem array
        paletteItemContent = new GUIContent[MAST_Settings.gui.palette.prefab.Length];
        
        MAST_Settings.gui.palette.image =
            MAST_Asset_Loader.GetThumbnailCamera()
            .GetComponent<MAST_Preview_Camera_Component>()
            .GetThumbnails(MAST_Settings.gui.palette.prefab);
        
        for (int i = 0; i < MAST_Settings.gui.palette.prefab.Length; i++)
        {
            MAST_Settings.gui.palette.image[i].alphaIsTransparency = true;
            
            paletteItemContent[i] = new GUIContent(
                MAST_Settings.gui.palette.image[i],
                MAST_Settings.gui.palette.prefab[i].name.Replace("_", "\n").Replace(" ", "\n"));
        }
    }
#endregion
    
#endif
}