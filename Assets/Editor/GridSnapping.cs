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
    public static SpriteRenderer renderer;
    public static BoxCollider2D collider;
    public static Transform transform;
    public static float gridSize = 1;
    public static float resizeGridSize = 1;
    public static bool snappingEnabled = false;


    public static string x = "1";
    public static string y = "1";

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
            GameObject obj = Selection.gameObjects[0];
            transform = obj.transform;
            renderer = obj.GetComponent<SpriteRenderer>();
            collider = obj.GetComponent<BoxCollider2D>();
            if (renderer)
            {
                lastSize = renderer.size;
                lastPosition = transform.position;
            }
        }
    }

    private void OnGUI()
    {
    }

    private static void OnScene(SceneView sceneview)
    {

        Rect rect = new Rect(10, 10, 150, 100);
        if (Menu.GetChecked(OTHER_MENU))
        {
            rect = new Rect(10, 100, 150, 100);
        } 
        GUILayout.BeginArea(rect);
        if (snappingEnabled)
        {
            if (GUILayout.Button("Disable Snapping"))
            {
                snappingEnabled = false;
            }
        } else
        {
            if (GUILayout.Button("Enable Snapping"))
            {
                snappingEnabled = true;
            }
        }

        GUILayout.Label("Position:");
        x = GUILayout.TextField(x, 30);

        GUILayout.Label("Size:");
        y = GUILayout.TextField(y, 30);


        GUILayout.EndArea();

        if (x.Length == 0)
        {
            x = "0.1";
        }
        if (y.Length == 0)
        {
            y = "0.1";
        }

        x = Regex.Replace(x, "[^0-9.]", "");
        y = Regex.Replace(y, "[^0-9.]", "");

        float posGrid = float.Parse(x);
        float sizeGrid = float.Parse(y);


        if (renderer && snappingEnabled)
        {
            Vector3 newPosition = transform.position;
            Vector2 newSize = renderer.size;
            Vector2 sizeDelta = newSize - lastSize;
            Vector3 positionDelta = newPosition - lastPosition;
            //transform.position = new Vector3(Mathf.Round(transform.position.x / gridSize) * gridSize, Mathf.Round((transform.position.y - (sizeDelta.y - positionDelta.y)) / gridSize) * gridSize, Mathf.Round(transform.position.z / gridSize) * gridSize);
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
                collider.size = renderer.size;
            }

            lastPosition = transform.position;
            lastSize = renderer.size;
        }

    }
}
