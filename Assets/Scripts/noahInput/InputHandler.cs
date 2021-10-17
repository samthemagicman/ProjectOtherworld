using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class InputHandler : MonoBehaviour
{

    public FadeOut fader;

    public bool isController;
    public bool isKeyboard;

    public enum eInputState
    {
        MouseKeyboard,
        Controller
    };

    private eInputState m_State = eInputState.MouseKeyboard;

    void OnGUI()
    {
        switch (m_State)
        {
            case eInputState.MouseKeyboard:
                if (isControllerInput())
                {
                    m_State = eInputState.Controller;
                    isController = true;
                    isKeyboard = false;
                    //print("Controller: " + isController);
                }
                break;
            case eInputState.Controller:
                if (isMouseKeyboard())
                {
                    m_State = eInputState.MouseKeyboard;
                    isKeyboard = true;
                    isController = false;
                    //print("mkb: "+ isKeyboard);
                }
                break;
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
        
        string[] names = Input.GetJoystickNames();
        foreach (string n in names) if (n.Length > 0) return true;
        return false;

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