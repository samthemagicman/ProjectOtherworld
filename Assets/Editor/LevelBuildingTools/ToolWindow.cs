using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ToolWindow : EditorWindow
{
    string myString = "Hello World";
    bool showingDimensions = false;
    bool myBool = true;
    float myFloat = 1.23f;
    bool onSceneGuiActivated = false;

    bool selectedObjectDimension;

    Color dimension1Color = new Color(1, 0, 0, 0.1f);
    Color dimension2Color = new Color(0, 0, 1, 0.1f);

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/Level Building Tool")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(ToolWindow));
    }


    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);

        EditorGUILayout.DropdownButton(new GUIContent("Test", "Test), FocusType.Passive);
        EditorGUILayout.LabelField("Object Dimension");
        EditorGUILayout.BeginHorizontal();
        selectedObjectDimension = EditorGUILayout.Toggle("Dimension 1", selectedObjectDimension);
        selectedObjectDimension = !EditorGUILayout.Toggle("Dimension 2", !selectedObjectDimension);
        selectedObjectDimension = !EditorGUILayout.Toggle("None", !selectedObjectDimension);
        EditorGUILayout.EndHorizontal();


        showingDimensions = EditorGUILayout.BeginToggleGroup("Enable Dimension Settings", showingDimensions);
        dimension1Color = EditorGUILayout.ColorField("Dimension 1", dimension1Color);
        dimension2Color = EditorGUILayout.ColorField("Dimension 2", dimension2Color);

        EditorGUILayout.EndToggleGroup();
    }
    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {

        SceneView.duringSceneGui -= OnSceneGUI;
    }

    void OnSceneGUI(SceneView sceneView) 
    {
        if (showingDimensions)
        {

            DimensionFilter[] dimensions = Resources.FindObjectsOfTypeAll(typeof(DimensionFilter)) as DimensionFilter[];
            foreach (DimensionFilter dim in dimensions)
            {
                SpriteRenderer renderer = dim.gameObject.GetComponent<SpriteRenderer>();
                if (!renderer) continue;
                Vector2 position = dim.transform.position;
                Vector3[] rectangleCorners =
                {
                    new Vector3(position.x - renderer.size.x/2, position.y - renderer.size.y/2, 0),   // Bottom Left
                    new Vector3(position.x + renderer.size.x/2, position.y - renderer.size.y/2, 0),   // Bottom Right
                    new Vector3(position.x + renderer.size.x/2, position.y + renderer.size.y/2, 0),   // Top Right
                    new Vector3(position.x - renderer.size.x/2, position.y + renderer.size.y/2, 0)    // Top Left
                };
                if (dim.dimension == DimensionFilter.Dimension.One)
                {
                    Handles.DrawSolidRectangleWithOutline(rectangleCorners, dimension1Color, Color.white);
                } else
                {
                    Handles.DrawSolidRectangleWithOutline(rectangleCorners, dimension2Color, Color.white);
                }
            }
            HandleUtility.Repaint();
        }
    }

}