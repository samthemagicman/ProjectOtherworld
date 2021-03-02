using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

#pragma warning disable

public class GridSnapping : EditorWindow
{
    private const string MENU_NAME = "Custom Tools/GridSnapping";
    private const string OTHER_MENU = "Custom Tools/Dimension Preview";

    public static Vector2 lastSize;
    public static Vector3 lastPosition;
    public static float gridSize = 1;
    public static float resizeGridSize = 1;
    public static bool snappingEnabled = true;
    public static EditorInfo selectionInfo;
    public static SpriteRenderer renderer;
    public static BoxCollider2D collider;
    public static Transform transform;
    public static GameObject selection;
    public static DimensionFilter dfilter;
    private static bool autoGetFromSelection;

    public static string posSnap = "1";
    public static string sizeSnap = "2";
    public static string colSizeOffset = "0"; 
    private static bool noWindow = true;
    

    public static bool IsEnabled
    {
        get { return Menu.GetChecked(MENU_NAME); }
    }

    [MenuItem(MENU_NAME)]
    public static void OnDimensionPreviewButton()
    {
        if (IsEnabled)
        {
            SceneView.duringSceneGui -= OnScene;
            Menu.SetChecked(MENU_NAME, false);
        } else
        {
            SceneView.duringSceneGui += OnScene;
            Menu.SetChecked(MENU_NAME, true);
            OnSelectionChange();

        }
    }
    [InitializeOnLoadMethod]
    static void SetupCallback()
    {
        EditorApplication.playmodeStateChanged -= PlayModeChanged;
        EditorApplication.playmodeStateChanged += PlayModeChanged;
        Selection.selectionChanged += OnSelectionChange;
        
    }
    static void PlayModeChanged()
    {
        if (!EditorApplication.isPlaying)
        {
            OnDimensionPreviewButton();
        }
    }

    static void OnSelectionChange()
    {
        if (Selection.gameObjects.Length > 0)
        {
            selection = Selection.gameObjects[0];
            transform = selection.transform;
            renderer = selection.GetComponent<SpriteRenderer>();
            collider = selection.GetComponent<BoxCollider2D>();
            selectionInfo = selection.GetComponent<EditorInfo>();
            dfilter = selection.GetComponent<DimensionFilter>();
            if (autoGetFromSelection)
            {
                GetFromSelection();
            }
            if (renderer)
            {
                lastSize = renderer.size;
                lastPosition = transform.position;
            }
        }
    }


    private static void OnScene(SceneView sceneview)
    {
        Rect rect = new Rect(10, 10, 150, 300);
        if (Menu.GetChecked(OTHER_MENU))
        {
            rect = new Rect(10, 100, 150, 350);
        }
        GUILayout.BeginArea(rect);
        snappingEnabled = GUILayout.Toggle(snappingEnabled, "Tile Snapping");

        GUILayout.Label("Position:");
        posSnap = GUILayout.TextField(posSnap, 30);

        GUILayout.Label("Size:");
        sizeSnap = GUILayout.TextField(sizeSnap, 30);

        GUILayout.Label("Collider Size Offset");
        colSizeOffset = GUILayout.TextField(colSizeOffset, 30);

        autoGetFromSelection = GUILayout.Toggle(autoGetFromSelection, "Auto get from selection");
        if (selectionInfo)
        {
            if (GUILayout.Button("Get from selection") || autoGetFromSelection)
            {
                GetFromSelection();
            }
        }
        // GUILayout.Toggle(show tools)

        // lock z and rotation
        //push into next layer
        //pull into last layer
        // fix z and rotation
        //add/remove colider
        if (collider)
        {
            if (GUILayout.Button("Remove Collider"))
            {
                DestroyImmediate(collider);
            }
        }else if (GUILayout.Button("Add Collider"))
        {
            selection.AddComponent<BoxCollider2D>();
        }
        //add/remove dimension filter
        GUILayout.Label("Dimension Controls");
        GUILayout.Label($"Dimension: {(dfilter ? dfilter.dimension.ToString() : "Both")}" );
        if (dfilter)
        {
            if (GUILayout.Button("Remove DFilter"))
            {
                DestroyImmediate(dfilter);
            }
        }
        else if (GUILayout.Button("Add DFilter"))
        {
            selection.AddComponent<DimensionFilter>();
            OnSelectionChange();
        }
        //swap dimension filter 
        if (dfilter)
        {
            if(GUILayout.Button("Swap Dimension"))
            {
                if(dfilter.dimension == DimensionFilter.Dimension.One)
                {
                    dfilter.dimension = DimensionFilter.Dimension.Two;
                }else if(dfilter.dimension == DimensionFilter.Dimension.Two)
                {
                    dfilter.dimension = DimensionFilter.Dimension.One;
                    //need to refresh dimension view in editor
                }
            }
        }
        GUILayout.EndArea();
        noWindow = false;

        
        if (posSnap.Length == 0)
        {
            posSnap = "0.1";
        }
        if (sizeSnap.Length == 0)
        {
            sizeSnap = "0.1";
        }
        if (colSizeOffset.Length == 0)
        {
            colSizeOffset = "0.1";
        }


        posSnap = Regex.Replace(posSnap, "[^0-9.]", "");
        sizeSnap = Regex.Replace(sizeSnap, "[^0-9.]", "");
        colSizeOffset = Regex.Replace(colSizeOffset, "[^0-9.]", "");
        float posGrid = float.Parse(posSnap);
        float sizeGrid = float.Parse(sizeSnap);
        float colliderOffset = float.Parse(colSizeOffset);


        if (renderer && snappingEnabled)
        {
            Vector3 newPosition = transform.position;
            Vector2 newSize = renderer.size;
            Vector2 sizeDelta = newSize - lastSize;
            Vector3 positionDelta = newPosition - lastPosition;
            
            renderer.size = new Vector2(Mathf.Round(renderer.size.x / sizeGrid) * sizeGrid, Mathf.Round(renderer.size.y / sizeGrid) * sizeGrid);
            newSize = renderer.size;
            Vector2 sizeDeltaAfterChange = newSize - lastSize;
            if (sizeDelta.magnitude > 0)
            {
                transform.position = new Vector3(lastPosition.x + sizeDeltaAfterChange.x / 2, lastPosition.y - sizeDeltaAfterChange.y / 2, transform.position.z);
            }
            else if (positionDelta.magnitude > 0)
            {
                transform.position = new Vector3(Mathf.Round(transform.position.x / posGrid) * posGrid, Mathf.Round(transform.position.y / posGrid) * posGrid, Mathf.Round(transform.position.z / posGrid) * posGrid);
            }
            if (collider)
            {
                collider.size = new Vector2(renderer.size.x - colliderOffset, renderer.size.y - colliderOffset); 
            }
            lastPosition = transform.position;
            lastSize = renderer.size;
        }

    }
    private static void GetFromSelection()
    {
        if (selectionInfo)
        {
            posSnap = selectionInfo.posGridSnapping.ToString();
            sizeSnap = selectionInfo.sizeGridSnapping.ToString();
            colSizeOffset = selectionInfo.colliderSizeOffset.ToString();
        }
        else
        {
            posSnap = "1";
            sizeSnap = "2";
            colSizeOffset = "0";
        }
       
    }
}
