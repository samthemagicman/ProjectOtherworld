using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

[CustomEditor(typeof(CameraPositionTrigger))]
public class CameraPositionTriggerInspector : Editor
{
    public CameraPositionTrigger.CameraType cameraTypeSelection;
    public override void OnInspectorGUI()
    {
        CameraPositionTrigger t = (CameraPositionTrigger)target;
        DrawPropertiesExcluding(serializedObject, new string[] { "cameraType", "dynamicObjectIndicator" });


        t.cameraType = (CameraPositionTrigger.CameraType)EditorGUILayout.EnumPopup("Camera Type", t.cameraType);
        if (t.cameraType == CameraPositionTrigger.CameraType.Dynamic && t.dynamicObjectIndicator == null)
        {
            t.dynamicObjectIndicator = CreateStaticObjectIndicator(t.gameObject);
            //t.staticObjectIndicator = new GameObject("StaticCameraIndicator", typeof(SpriteRenderer));
            //t.staticObjectIndicator.transform.SetParent(t.transform);
        }
        else if (t.cameraType == CameraPositionTrigger.CameraType.Static && t.dynamicObjectIndicator != null)
        {
            DestroyImmediate(t.dynamicObjectIndicator);
        }


        serializedObject.ApplyModifiedProperties();
    }

    GameObject CreateStaticObjectIndicator(GameObject target)
    {
        GameObject indicator;
        indicator = new GameObject("DynamicCameraArea", typeof(SpriteRenderer));
        indicator.transform.SetParent(target.transform);
        indicator.transform.position = target.transform.position;

        SpriteRenderer renderer = indicator.GetComponent<SpriteRenderer>();

        Camera cam = target.GetComponent<Camera>();
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        renderer.size = new Vector3(width, height);
        return indicator;
    }
}
