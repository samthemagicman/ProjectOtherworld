using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderMover : MonoBehaviour
{
    private Slider mySlider;
    private GameObject thisSlider;
    public float sliderChange;
    private float maxSliderValue;
    private float minSliderValue;
    private float sliderRange;
    private const float SLIDERSTEP = 200.0f; //used to determine how fine the value
    private const string SLIDERMOVE = "SliderHorizontal";

    //initialize values
    private void Awake()
    {
        mySlider = GetComponent<ED_Slider>();
        thisSlider = gameObject; // used to determine when the slider has focus
        maxSliderValue = mySlider.maxValue;
        minSliderValue = mySlider.minValue;
        sliderRange = maxSliderValue - minSliderValue;
        mySlider.value = 5.0f;
    }

    private void Update()
    {
        //if slider has focus
        if(thisSlider == EventSystem.current.currentSelectedGameObject)
        {
            sliderChange = Input.GetAxis(axisName: SLIDERMOVE) * sliderRange / SLIDERSTEP;
            float sliderValue = mySlider.value;
            float tempValue = sliderValue + sliderChange;
            if(tempValue <= maxSliderValue && tempValue >= minSliderValue)
            {
                print("this is the temp: " + tempValue);
                sliderValue = tempValue;
            }
            mySlider.value = sliderValue;
        }
    }
}
