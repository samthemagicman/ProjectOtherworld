using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEditor;
[InitializeOnLoad]
public class CustomCursor
{
    void OnSceneGUI()
    {
        EditorApplication.update += Update;
    }
    static void Update()
    {
        Debug.Log("uyup");
        CustomCursor.ChangeCursor(CustomCursor.WindowsCursor.DoublePointedArrowPointingNorthAndSouth);
    }


    public enum WindowsCursor
    {
        none = 0,
        StandardArrowAndSmallHourglass = 32650,
        StandardArrow = 32512,
        Crosshair = 32515,
        Hand = 32649,
        ArrowAndQuestionMark = 32651,
        IBeam = 32513,
        //Icon = 32641, // Obsolete for applications marked version 4.0 or later. 
        SlashedCircle = 32648,
        //Size = 32640,  // Obsolete for applications marked version 4.0 or later. Use FourPointedArrowPointingNorthSouthEastAndWest
        FourPointedArrowPointingNorthSouthEastAndWest = 32646,
        DoublePointedArrowPointingNortheastAndSouthwest = 32643,
        DoublePointedArrowPointingNorthAndSouth = 32645,
        DoublePointedArrowPointingNorthwestAndSoutheast = 32642,
        DoublePointedArrowPointingWestAndEast = 32644,
        VerticalArrow = 32516,
        Hourglass = 32514
    }

    public static void ChangeCursor(WindowsCursor cursor)
    {
        SetCursor(LoadCursor(IntPtr.Zero, (int)cursor));
    }

    [DllImport("user32.dll", EntryPoint = "SetCursor")]
    public static extern IntPtr SetCursor(IntPtr hCursor);

    [DllImport("user32.dll", EntryPoint = "LoadCursor")]
    public static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);
}
