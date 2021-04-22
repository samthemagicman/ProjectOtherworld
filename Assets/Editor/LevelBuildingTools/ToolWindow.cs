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

    DimensionFilter.Dimension selectedObjectDimension;

    Color dimension1Color = new Color(1, 0, 0, 0.1f);
    Color dimension2Color = new Color(0, 0, 1, 0.1f);
    Color outlineColor = new Color(0, 1, 0, 1f);

    GameObject lastTarget = null;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/Level Building Tool")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(ToolWindow));
    }
    public void OnInspectorUpdate()
    {
        // This will only get called 10 times per second.
        Repaint();
    }


    void OnGUI()
    {
        GameObject currentSelection = Selection.activeGameObject;
        bool targetChanged = currentSelection != lastTarget;
        DimensionFilter dimensionFilter = null;
        if (currentSelection != null)
            dimensionFilter = currentSelection.GetComponent<DimensionFilter>();

        if (dimensionFilter != null && targetChanged)
        {
            selectedObjectDimension = dimensionFilter.dimension;
        }

        GUILayout.Label("Base Settings", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();

        if (dimensionFilter == null) GUI.enabled = false;
        if (GUILayout.Button("None")) DestroyImmediate(dimensionFilter);
        GUI.enabled = true;

        if (dimensionFilter != null && selectedObjectDimension == DimensionFilter.Dimension.One) GUI.enabled = false;
        if (GUILayout.Button("Dimension 1"))
        {
            if (dimensionFilter == null)
            {
                dimensionFilter = currentSelection.AddComponent<DimensionFilter>();
            }
            dimensionFilter.dimension = DimensionFilter.Dimension.One;
            selectedObjectDimension = DimensionFilter.Dimension.One;
        }
        GUI.enabled = true;

        if (dimensionFilter != null && selectedObjectDimension == DimensionFilter.Dimension.Two) GUI.enabled = false;
        if (GUILayout.Button("Dimension 2"))
        {
            if (dimensionFilter == null)
            {
                dimensionFilter = currentSelection.AddComponent<DimensionFilter>();
            }
            dimensionFilter.dimension = DimensionFilter.Dimension.Two;
            selectedObjectDimension = DimensionFilter.Dimension.Two;
        }
        GUI.enabled = true;

        EditorGUILayout.EndHorizontal();

        GUI.enabled = true;

        showingDimensions = EditorGUILayout.BeginToggleGroup("Enable Dimension Settings", showingDimensions);
        dimension1Color = EditorGUILayout.ColorField("Dimension 1", dimension1Color);
        dimension2Color = EditorGUILayout.ColorField("Dimension 2", dimension2Color);
        outlineColor = EditorGUILayout.ColorField("Outline Color", outlineColor);
        EditorGUILayout.EndToggleGroup();


        lastTarget = Selection.activeGameObject;
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
                    Handles.DrawSolidRectangleWithOutline(rectangleCorners, dimension1Color, outlineColor);
                } else
                {
                    Handles.DrawSolidRectangleWithOutline(rectangleCorners, dimension2Color, outlineColor);
                }
            }
            HandleUtility.Repaint();
        }
    }

}