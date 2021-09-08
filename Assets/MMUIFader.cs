using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MMUIFader : MonoBehaviour
{
    public CanvasGroup uiElement;
    public GameObject firstMenuButton;

    void Start()
    {
        StartCoroutine(fader());
    }

    public void FadeOut()
    {
        StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 0));
    }

    IEnumerator fader()
    {
        EventSystem.current.SetSelectedGameObject(null);
        FadeOut();
        yield return new WaitForSeconds(5);
        var dasd = GameObject.Find("Panel");
        EventSystem.current.SetSelectedGameObject(firstMenuButton);
        dasd.SetActive(false);
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