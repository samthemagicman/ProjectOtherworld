using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class InputHandler : MonoBehaviour
{

    public FadeOut fader;
    public MenuNavi menuNavi;
    public EventSystem events;

    public enum eInputState
    {
        MouseKeyboard,
        Controller
    };

    private eInputState m_State = eInputState.MouseKeyboard;

    void OnGUI()
    {
        if (!fader.running)
        {
            switch (m_State)
            {
                case eInputState.MouseKeyboard:
                    if (isControllerInput())
                    {
                        m_State = eInputState.Controller;
                        if (menuNavi.mainMenuActive == true)
                        {
                            events.SetSelectedGameObject(menuNavi.firstMenuButton);
                            print(menuNavi.firstMenuButton);
                            print("first menu button on");
                        } else if (menuNavi.optionsMenuActive == true)
                        {
                            events.SetSelectedGameObject(menuNavi.optionsFirstButton);
                            print("opti menu button on");
                        } else if (menuNavi.displayMenuActive == true)
                        {
                            events.SetSelectedGameObject(menuNavi.displayFirstButton);
                            print("fit menu button on");
                        } else if (menuNavi.audioMenuActive == true)
                        {
                            events.SetSelectedGameObject(menuNavi.audioFirstButton);
                            print("fiasdrst mena button on");
                        } else if (menuNavi.advancedMenuActive == true)
                        {
                            events.SetSelectedGameObject(menuNavi.advancedFirstButton);
                            print("gay enu button on");
                        }
                        Debug.Log("JoyStick being used");
                    }
                    break;
                case eInputState.Controller:
                    if (isMouseKeyboard())
                    {
                        m_State = eInputState.MouseKeyboard; 
                        EventSystem.current.SetSelectedGameObject(null);
                        Debug.Log("Mouse & Keyboard being used");
                    }
                    break;
            }
        }
    }

    public eInputState GetInputState()
    {
        return m_State;
    }
 
    private bool isMouseKeyboard()
    {
        // mouse & keyboard buttons
        if (Event.current.isKey ||
            Event.current.isMouse)
        {
            return true;
        }
        // mouse movement
        if (Input.GetAxis("Mouse X") != 0.0f ||
            Input.GetAxis("Mouse Y") != 0.0f)
        {
            return true;
        }
        return false;
    }

    private bool isControllerInput()
    {
        // joystick buttons
        if (Input.GetKey(KeyCode.Joystick1Button0) ||
           Input.GetKey(KeyCode.Joystick1Button1) ||
           Input.GetKey(KeyCode.Joystick1Button2) ||
           Input.GetKey(KeyCode.Joystick1Button3) ||
           Input.GetKey(KeyCode.Joystick1Button4) ||
           Input.GetKey(KeyCode.Joystick1Button5) ||
           Input.GetKey(KeyCode.Joystick1Button6) ||
           Input.GetKey(KeyCode.Joystick1Button7) ||
           Input.GetKey(KeyCode.Joystick1Button8) ||
           Input.GetKey(KeyCode.Joystick1Button9) ||
           Input.GetKey(KeyCode.Joystick1Button10) ||
           Input.GetKey(KeyCode.Joystick1Button11) ||
           Input.GetKey(KeyCode.Joystick1Button12) ||
           Input.GetKey(KeyCode.Joystick1Button13) ||
           Input.GetKey(KeyCode.Joystick1Button14) ||
           Input.GetKey(KeyCode.Joystick1Button15) ||
           Input.GetKey(KeyCode.Joystick1Button16) ||
           Input.GetKey(KeyCode.Joystick1Button17) ||
           Input.GetKey(KeyCode.Joystick1Button18) ||
           Input.GetKey(KeyCode.Joystick1Button19) ||
           Input.GetAxis("Horizontal") != 0.0f     ||
           Input.GetAxis("Vertical") != 0.0f)
        {
            return true;
        }
        return false;
    }
}