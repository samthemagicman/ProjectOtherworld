using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
public class Parascope : MonoBehaviour
{
    public PlayerMovement player;
    public RectTransform interactPromptUI;
    public GameObject cameraArea;
    private BoxCollider2D interactArea;
    private bool inInteractArea;
    public Camera mainCam;
    public Camera paraCam;
    private CameraPositionHandler cameraBounds;
    public float panSpeed = 10;
    public void Awake()
    {
        interactArea = GetComponent<BoxCollider2D>();
        cameraBounds = GetComponentInParent<CameraPositionHandler>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        interactPromptUI.gameObject.SetActive(true);
        inInteractArea = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        interactPromptUI.gameObject.SetActive(false);
        inInteractArea = false;
    }

    private void Update()
    {
        if (inInteractArea)
        {
            if (Input.GetButtonDown("Interact"))
            {
                Debug.Log("input detected");
                //paraCam.enabled = !paraCam.enabled;
                //mainCam.enabled = !mainCam.enabled;

                
                if (PlayerMovement.controlsEnabled == true)
                {
                    paraCam.transform.position = transform.position;
                    paraCam.enabled = true;
                    mainCam.enabled = false;
                    PlayerMovement.DisableControls();
                    Debug.Log(PlayerMovement.controlsEnabled);
                }
                else if (PlayerMovement.controlsEnabled == false)
                {
                    mainCam.enabled = true;
                    paraCam.enabled = false;

                    PlayerMovement.EnableControls();
                    Debug.Log(PlayerMovement.controlsEnabled);
                }
            }
            if (paraCam.enabled && PlayerMovement.controlsEnabled == false)
            {
                float moveHorizontal = Input.GetAxisRaw("Horizontal");
                float moveVertical = Input.GetAxisRaw("Vertical");
                Vector3 wantedPos = paraCam.transform.position;
                //wantedPos = new Vector3(Mathf.Clamp(wantedPos.x + moveHorizontal, cameraBounds.xLimitMin, cameraBounds.xLimitMax), Mathf.Clamp(wantedPos.y + moveVertical, cameraBounds.yLimitMin, cameraBounds.yLimitMax));
                wantedPos +=new Vector3(moveHorizontal*panSpeed, moveVertical*panSpeed, 0);
                paraCam.transform.position = wantedPos;
                Debug.Log("moveH:" + moveHorizontal);
                Debug.Log("moveV:" + moveVertical);
                Debug.Log("wantedPos:" + wantedPos);
                Debug.Log("paraCamPos:" +paraCam.transform.position);
            }
        }
        
    }
}