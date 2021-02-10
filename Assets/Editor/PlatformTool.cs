using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;

// Tagging a class with the EditorTool attribute and no target type registers a global tool. Global tools are valid for any selection, and are accessible through the top left toolbar in the editor.
[EditorTool("Platform Tool")]
class PlatformTool : EditorTool
{
    // Serialize this value to set a default value in the Inspector.
    [SerializeField]
    Texture2D m_ToolIcon;

    GUIContent m_IconContent;

    void OnEnable()
    {
        m_IconContent = new GUIContent()
        {
            image = m_ToolIcon,
            text = "Platform Tool",
            tooltip = "Platform Tool"
        };
    }

    public override GUIContent toolbarIcon
    {
        get { return m_IconContent; }
    }

    // This is called for each window that your tool is active in. Put the functionality of your tool here.
    public override void OnToolGUI(EditorWindow window)
    {

        EditorGUI.BeginChangeCheck();

        Vector3 up = Tools.handlePosition;
        var pos = Selection.transforms[0].position;
        float range = 5;

        Vector3[] verts = new Vector3[]
        {
            new Vector3(pos.x - range, pos.y - range, pos.z),
            new Vector3(pos.x - range, pos.y + range, pos.z),
            new Vector3(pos.x + range, pos.y + range, pos.z),
            new Vector3(pos.x + range, pos.y - range, pos.z)
        };

        Handles.DrawSolidRectangleWithOutline(verts, new Color(1, 1, 0.5f, 0.5f), new Color(0.2f, 0, 0, 1));

        foreach (Vector3 posCube in verts)
        {
            Handles.ScaleValueHandle(range,
                posCube,
                Quaternion.identity,
                1.0f,
                Handles.CubeHandleCap,
                1.0f);
        }

        using (new Handles.DrawingScope(Color.green))
        {
            up = Handles.Slider(up, Vector3.up);
        }


        if (EditorGUI.EndChangeCheck())
        {
            Vector3 delta = up - Tools.handlePosition;

            Undo.RecordObjects(Selection.transforms, "Move Platform");
            Debug.Log(delta);
            foreach (var transform in Selection.transforms)
                transform.position += delta;
        }
    }
}