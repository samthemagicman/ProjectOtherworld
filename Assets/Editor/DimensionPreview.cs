using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#pragma warning disable

public class DimensionPreview : EditorWindow
{
    private const string MENU_NAME = "Custom Tools/Dimension Preview";

    public static bool IsEnabled
    {
        get { return Menu.GetChecked(DimensionPreview.MENU_NAME); }
    }

    [MenuItem(DimensionPreview.MENU_NAME)]
    public static void OnDimensionPreviewButton()
    {
        if (IsEnabled)
        {
            SceneView.duringSceneGui -= OnScene;
            Menu.SetChecked(DimensionPreview.MENU_NAME, false);
        } else
        {
            SceneView.duringSceneGui += OnScene;
            Menu.SetChecked(DimensionPreview.MENU_NAME, true);
        }
    }
    [InitializeOnLoadMethod]
    static void SetupCallback()
    {
        EditorApplication.playmodeStateChanged -= PlayModeChanged;
        EditorApplication.playmodeStateChanged += PlayModeChanged;
    }
    static void PlayModeChanged()
    {
        if (!EditorApplication.isPlaying)
        {
            OnDimensionPreviewButton();
        }
    }

    static void ShowAllDimensionObjects(bool show)
    {
        DimensionFilter[] dimensions = Resources.FindObjectsOfTypeAll(typeof(DimensionFilter)) as DimensionFilter[];
        foreach (DimensionFilter dim in dimensions)
        {
            if (show)
            {
                SceneVisibilityManager.instance.Show(dim.gameObject, true);
            }
            else
            {
                SceneVisibilityManager.instance.Hide(dim.gameObject, true);
            }
        }
    }

    static void ShowDimension1(bool show)
    {
        DimensionFilter[] dimensions = Resources.FindObjectsOfTypeAll(typeof(DimensionFilter)) as DimensionFilter[];
        foreach (DimensionFilter dim in dimensions)
        {
            if (show && dim.dimension == DimensionFilter.Dimension.One)
            {
                SceneVisibilityManager.instance.Show(dim.gameObject, true);
            } else
            {
                SceneVisibilityManager.instance.Hide(dim.gameObject, true);
            }
        }
    }

    static void ShowDimension2(bool show)
    {
        DimensionFilter[] dimensions = Resources.FindObjectsOfTypeAll(typeof(DimensionFilter)) as DimensionFilter[];
        foreach (DimensionFilter dim in dimensions)
        {
            if (show && dim.dimension == DimensionFilter.Dimension.Two)
            {
                SceneVisibilityManager.instance.Show(dim.gameObject, true);
            }
            else
            {
                SceneVisibilityManager.instance.Hide(dim.gameObject, true);
            }
        }
    }


    private static void OnScene(SceneView sceneview)
    {
        GUILayout.BeginArea(new Rect(10, 10, 100, 100));
        if (GUILayout.Button("All Dimensions"))
            ShowAllDimensionObjects(true);
        else if (GUILayout.Button("Dimension 1"))
        {
            ShowDimension1(true);
        }
        else if (GUILayout.Button("Dimension 2"))
        {
            ShowDimension2(true);
        }
        GUILayout.EndArea();

    }
}
