using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Diagnostics;

public class FadeOut : MonoBehaviour
{
    public CanvasGroup uiElement;
    public bool running = true;

    void Start()
    {
        StartCoroutine(fader());
    }

    public void FadingOut()
    {
        StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 0));
    }

    IEnumerator fader()
    {
        FadingOut();
        yield return new WaitForSeconds(5);
        var dasd = GameObject.Find("Panel");
        running = false;
        if(dasd) dasd.SetActive(false);
    }



    public IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime = 5.0f)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            cg.alpha = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForEndOfFrame();
        }
    }
}