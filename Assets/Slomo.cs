using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slomo : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButton("Slomo"))
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0.3f, 0.1f);
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }
        else
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, 0.12f);
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }
    }
}
