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
        DrawPropertiesExcluding(serializedObject, new string[] { "cameraType" });


        t.cameraType = (CameraPositionTrigger.CameraType)EditorGUILayout.EnumPopup("Camera Type", t.cameraType);
        if (t.cameraType == CameraPositionTrigger.CameraType.Dynamic && t.staticObjectIndicator == null)
        {
            t.staticObjectIndicator = new GameObject("StaticCameraIndicator", typeof(SpriteRenderer));
            t.staticObjectIndicator.transform.SetParent(t.transform);
        }
        else if (t.cameraType == CameraPositionTrigger.CameraType.Static && t.staticObjectIndicator != null)
        {
            DestroyImmediate(t.staticObjectIndicator);
        }


        serializedObject.ApplyModifiedProperties();
    }
}
