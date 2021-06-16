using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.EditorTools;
using System.Runtime.InteropServices;
using System;

//http://www.luispedrofonseca.com/unity-custom-rect-editor/

//[CustomEditor(typeof(RectExample))]
[EditorTool("Level Builder")]
public class LevelBuilder : EditorTool
{
    public static CustomCursor.WindowsCursor currentCursor = CustomCursor.WindowsCursor.none;
    void OnEnable()
    {
        EditorApplication.update += Update;
    }

    private void OnDisable()
    {
        EditorApplication.update -= Update;
    }

    static void Update()
    {
        if (currentCursor != CustomCursor.WindowsCursor.none) CustomCursor.ChangeCursor(currentCursor);
    }


    public override void OnToolGUI(EditorWindow window)
    {
        if (target == null) return;
        GameObject targetGameObject = ((GameObject)target);
        SpriteRenderer rectExample = ((GameObject)target).GetComponent<SpriteRenderer>();
        if (!rectExample) return;
        var rect = RectUtils.ResizeRect(
            new RectUtils.ObjectDetails(targetGameObject.transform.position, rectExample.size),
            rectExample.gameObject
        );

        foreach (GameObject obj in Selection.gameObjects)
        {
            Undo.RecordObject(obj.transform, "move/resize");
            SpriteRenderer rend = obj.GetComponent<SpriteRenderer>();
            if (rend)
            {
                Undo.RecordObject(rend, "move/resize");
            }
        }

        Vector2 previousPosition = targetGameObject.transform.position;
        targetGameObject.transform.position = new Vector3(rect.position.x, rect.position.y, targetGameObject.transform.position.z);
        Vector2 positionDelta = ((Vector2)targetGameObject.transform.position) - previousPosition;
        //if (targetGameObject.transform.position.x % 0.5f != 0 || targetGameObject.transform.position.y % 0.5f != 0)
        

        Vector2 prev = rectExample.size;
        rectExample.size = rect.size;
        Vector2 sizeDelta = rect.size - prev;

        if (rect.positionDelta.magnitude > 0)
        {
            GameObject[] selected = Selection.gameObjects;
            foreach (GameObject obj in selected)
            {
                if (obj == targetGameObject) continue;
                if (obj.transform.position.x % 0.5f != 0 || obj.transform.position.y % 0.5f != 0)
                {
                    obj.transform.position = new Vector3( Mathf.Floor(obj.transform.position.x / 0.5f) * 0.5f, Mathf.Floor(obj.transform.position.y / 0.5f) * 0.5f, 0);
                }
                obj.transform.position += new Vector3(positionDelta.x, positionDelta.y, 0);
                obj.GetComponent<SpriteRenderer>().size += sizeDelta;
                SnapToGrid(obj);
            }
        }

        SnapToGrid(targetGameObject);

    }

    void SnapToGrid(GameObject obj)
    {
        SpriteRenderer rectExample = obj.GetComponent<SpriteRenderer>();
        rectExample.size = new Vector2(Mathf.Round(rectExample.size.x), Mathf.Round(rectExample.size.y));
        if (rectExample.size.x % 2 == 0) // even
        {
            if (obj.transform.position.x % 1f != 0)
            {
                obj.transform.position = new Vector3(Mathf.Floor(obj.transform.position.x), obj.transform.position.y, obj.transform.position.z);
            }
        }
        else //odd
        {
            if ((obj.transform.position.x / 0.5f) % 2 != 1)
            {
                obj.transform.position = new Vector3(Mathf.Round(obj.transform.position.x) + 0.5f, obj.transform.position.y, obj.transform.position.z);
            }
        }

        if (rectExample.size.y % 2 == 0) // even
        {
            if (obj.transform.position.y % 1f != 0)
            {
                obj.transform.position = new Vector3(obj.transform.position.x, Mathf.Floor(obj.transform.position.y), obj.transform.position.z);
            }
        }
        else //odd
        {
            if ((obj.transform.position.y / 0.5f) % 2 != 1)
            {
                obj.transform.position = new Vector3(obj.transform.position.x, Mathf.Round(obj.transform.position.y) + 0.5f, obj.transform.position.z);
            }
        }
    }
}



public class RectUtils
{
    static Color capCol = Color.green;
    static Color fillCol = Color.yellow;
    public class ObjectDetails
    {
        public Vector2 position;
        public Vector2 size;
        public Vector2 positionDelta;
        public ObjectDetails(Vector2 position, Vector2 size)
        {
            this.position = position;
            this.size = size;
        }
        public ObjectDetails(Vector2 position, Vector2 size, Vector2 positionDelta)
        {
            this.position = position;
            this.size = size;
            this.positionDelta = positionDelta;
        }
    }

    public static int MouseIsNearObjectEdge(Vector2 position, Vector2 size)
    {
        Vector3 mousePosition = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
        float padding = 0.2f;
        var check1 = Mathf.Abs((position.x + size.x / 2) - mousePosition.x) < padding || Mathf.Abs((position.x - size.x / 2) - mousePosition.x) < padding;
        var check2 = Mathf.Abs((position.y + size.y / 2) - mousePosition.y) < padding || Mathf.Abs((position.y - size.y / 2) - mousePosition.y) < padding;
        if (check1) return 1;
        if (check2) return 2;
        return 0;
    }

    public static Vector3 GridSnap(Vector3 vector3, float gridSize=1)
    {
        return new Vector3(Mathf.Round(vector3.x / 1) * 1, Mathf.Round(vector3.y / 1) * 1);
    }

    public static Vector2 GridSnap(Vector2 vector3, float gridSize = 1)
    {
        return new Vector2(  Mathf.Round(vector3.x / 1) * 1, Mathf.Round(vector3.y / 1) * 1);
    }

    public static ObjectDetails ResizeRect(ObjectDetails rect, GameObject gameObject)
    {
        #region Mouse Changing
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        if (renderer)
        {
            int edge = MouseIsNearObjectEdge(gameObject.transform.position, renderer.size);
            if (edge == 1)
            {
                LevelBuilder.currentCursor = CustomCursor.WindowsCursor.DoublePointedArrowPointingWestAndEast;
            }
            else if (edge == 2)
            {
                LevelBuilder.currentCursor = CustomCursor.WindowsCursor.DoublePointedArrowPointingNorthAndSouth;
            }
            else
            {
                LevelBuilder.currentCursor = CustomCursor.WindowsCursor.none;
            }
        }
        #endregion

        float snap = 10;
        float capSize = HandleUtility.GetHandleSize(Vector3.zero) * 0.15f;
        Vector2 halfRectSize = new Vector2(rect.size.x * 0.5f, rect.size.y * 0.5f);

        if (gameObject.GetComponent<DimensionFilter>() == null)
        {
            Vector3[] rectangleCorners =
                {
                new Vector3(rect.position.x - halfRectSize.x, rect.position.y - halfRectSize.y, 0),   // Bottom Left
                new Vector3(rect.position.x + halfRectSize.x, rect.position.y - halfRectSize.y, 0),   // Bottom Right
                new Vector3(rect.position.x + halfRectSize.x, rect.position.y + halfRectSize.y, 0),   // Top Right
                new Vector3(rect.position.x - halfRectSize.x, rect.position.y + halfRectSize.y, 0)    // Top Left
            };

            Handles.color = fillCol;
            Handles.DrawSolidRectangleWithOutline(rectangleCorners, new Color(fillCol.r, fillCol.g, fillCol.b, 0.1f), capCol);
        }

        Vector3[] handlePoints =
            {
                new Vector3(rect.position.x - halfRectSize.x, rect.position.y, 0),   // Left
                new Vector3(rect.position.x + halfRectSize.x, rect.position.y, 0),   // Right
                new Vector3(rect.position.x, rect.position.y + halfRectSize.y, 0),   // Top
                new Vector3(rect.position.x, rect.position.y - halfRectSize.y, 0),    // Bottom 

                new Vector3(rect.position.x + halfRectSize.x, rect.position.y + halfRectSize.y, 0),   // Top Right
                new Vector3(rect.position.x - halfRectSize.x, rect.position.y + halfRectSize.y, 0),   // Top Left
                new Vector3(rect.position.x + halfRectSize.x, rect.position.y - halfRectSize.y, 0),   // Bottom Right
                new Vector3(rect.position.x - halfRectSize.x, rect.position.y - halfRectSize.y, 0)    // Bottom Left
            };

        Handles.color = capCol;

        var newSize = rect.size;
        var newPosition = rect.position;
        var leftHandle = Handles.Slider(handlePoints[0], -Vector3.right, rect.size.y / 2, Handles.RectangleHandleCap, snap).x - handlePoints[0].x; //Handles.Slider(handlePoints[0], -Vector3.right, capSize, capFunc, snap).x - handlePoints[0].x;
        var rightHandle = Handles.Slider(handlePoints[1], Vector3.right, rect.size.y / 2, Handles.RectangleHandleCap, snap).x - handlePoints[1].x;
        var topHandle = Handles.Slider(handlePoints[2], Vector3.up, rect.size.x / 2, Handles.RectangleHandleCap, snap).y - handlePoints[2].y; //Handles.Slider(handlePoints[2], Vector3.up, capSize, capFunc, snap).y - handlePoints[2].y;
        var bottomHandle = Handles.Slider(handlePoints[3], -Vector3.up, rect.size.x / 2, Handles.RectangleHandleCap, snap).y - handlePoints[3].y;//Handles.Slider(handlePoints[3], -Vector3.up, capSize, capFunc, snap).y - handlePoints[3].y;
        Vector2 middleHandle = Handles.Slider2D(rect.position, Vector3.back, Vector3.right, Vector3.up, capSize*2, Handles.RectangleHandleCap, snap) - new Vector3(rect.position.x, rect.position.y);

        Vector2 topRightHandle = Handles.Slider2D(handlePoints[4], Vector3.back, Vector3.right, Vector3.up, capSize, Handles.CubeHandleCap, snap) - handlePoints[4];
        Vector2 topLeftHandle = Handles.Slider2D(handlePoints[5], Vector3.back, Vector3.right, Vector3.up, capSize, Handles.CubeHandleCap, snap) - handlePoints[5];
        Vector2 bottomRightHandle = Handles.Slider2D(handlePoints[6], Vector3.back, Vector3.right, Vector3.up, capSize, Handles.CubeHandleCap, snap) - handlePoints[6];
        Vector2 bottomLeftHandle = Handles.Slider2D(handlePoints[7], Vector3.back, Vector3.right, Vector3.up, capSize, Handles.CubeHandleCap, snap) - handlePoints[7];




        leftHandle = Mathf.Round(leftHandle / 1) * 1;
        rightHandle = Mathf.Round(rightHandle / 1) * 1;
        topHandle = Mathf.Round(topHandle / 1) * 1;
        bottomHandle = Mathf.Round(bottomHandle / 1) * 1;
        middleHandle = new Vector2(Mathf.Round(middleHandle.x / 1) * 1, Mathf.Round(middleHandle.y / 1) * 1);

        topRightHandle = GridSnap(topRightHandle);
        topLeftHandle = GridSnap(topLeftHandle);
        bottomLeftHandle = GridSnap(bottomLeftHandle);
        bottomRightHandle = GridSnap(bottomRightHandle);

        newSize = new Vector2(
            Mathf.Max(.1f, newSize.x - leftHandle
            + topRightHandle.x
            - topLeftHandle.x
            + bottomRightHandle.x
            - bottomLeftHandle.x

            + rightHandle),
            Mathf.Max(.1f, newSize.y + topHandle
            + topRightHandle.y
            + topLeftHandle.y
            - bottomRightHandle.y
            - bottomLeftHandle.y
            - bottomHandle));

        newPosition = new Vector2(
            newPosition.x
            + leftHandle * .5f
            + rightHandle * .5f
            + middleHandle.x
            + topRightHandle.x / 2
            + topLeftHandle.x / 2
            + bottomRightHandle.x / 2
            + bottomLeftHandle.x / 2,

            newPosition.y + topHandle * .5f + bottomHandle * .5f + middleHandle.y
            + topRightHandle.y / 2
            + topLeftHandle.y / 2
            + bottomRightHandle.y / 2
            + bottomLeftHandle.y / 2
            );

        return new ObjectDetails(newPosition, newSize, ((Vector2) gameObject.transform.position) - newPosition);
    }
}