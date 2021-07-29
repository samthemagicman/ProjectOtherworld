using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slomo : MonoBehaviour
{
    public float speed = 0.3f;
    void Update()
    {
        if (Input.GetButton("PreviewDimension") && !Input.GetButtonDown("PreviewDimension") && !Input.GetButtonUp("PreviewDimension"))
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, speed, 0.1f);
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }
        else
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, 0.12f);
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }
    }
}
