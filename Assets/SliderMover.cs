using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderMover : MonoBehaviour
{
    private Slider mySlider;
    private GameObject thisSlider;
    private float sliderChange;
    private float maxSliderValue;
    private float minSliderValue;
    private float sliderRange;
    private const float SLIDERSTEP = 200.0f; //used to determine how fine the value
    private const string SLIDERMOVE = "SliderHorizontal";
    private static float sliderDeadZone = 0.1f;
    public static float selectionDelay = .3f;
    public static float selectionDeadZone = 0.5f;
    public static bool inputPaused;
    public static float endTime;

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
        if(endTime - Time.time <= 0) //this prevents overshoot/over reading analog input when moving up and down.
        {
            inputPaused = false;
        }
        if(thisSlider == EventSystem.current.currentSelectedGameObject)
        {
            //This is for changing Slider Value
            float horiMove = Input.GetAxis(axisName: SLIDERMOVE);
            if(horiMove > sliderDeadZone || horiMove < -sliderDeadZone) //deadzone prevents accidental slider changes when moving up and down
            {
                sliderChange = horiMove * sliderRange / SLIDERSTEP;
                float sliderValue = mySlider.value;
                float tempValue = sliderValue + sliderChange;
                if (tempValue <= maxSliderValue && tempValue >= minSliderValue)
                {
                    sliderValue = tempValue;
                }
                mySlider.value = sliderValue;
            }
            //this is for changing which slider is selected
            //honestly an easier way to do this probably is just to make a public array and manually put in the selectable values.
            float vertMove = Input.GetAxis("Vertical");
            if ((vertMove > selectionDeadZone || vertMove < -selectionDeadZone) && !inputPaused) //deadzone prevents accidental vertical inputs when adjusting slider
            {
                inputPaused = true;
                endTime = Time.time + selectionDelay;
                int childIndex = 0;
                Transform sliderList = transform.parent.parent;
                foreach (Transform child in sliderList)
                {
                    if (child == transform.parent)
                    {
                        if (vertMove > 0 && childIndex > 0)
                        {
                            GameObject nextItem = sliderList.GetChild(childIndex - 1).GetComponentInChildren<ED_Slider>().gameObject; // each slider also contains a text object that we are filtering out with get component
                            EventSystem.current.SetSelectedGameObject(nextItem);;
                            break;
                        }
                        else if (vertMove < 0 && childIndex < sliderList.childCount-1)
                        {
                            GameObject nextItem = sliderList.GetChild(childIndex + 1).GetComponentInChildren<ED_Slider>().gameObject;
                            EventSystem.current.SetSelectedGameObject(nextItem);
                            break;
                        }else if(childIndex >= sliderList.childCount-1)//its a lil hardcoded, but this grabs the back button when at the end of the list of sliders by grabing its parent.
                        {
                            EventSystem.current.SetSelectedGameObject(sliderList.parent.GetComponentInChildren<Button>().gameObject); // not quite sure why, but moving up from the button to select the sliders already works.
                            break;
                        }
                        Debug.Log("wasnt able to select anything");
                        break;
                    }
                    childIndex++;
                }
            }
        }
        
    }
}
