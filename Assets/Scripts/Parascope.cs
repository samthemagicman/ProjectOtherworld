using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
public class Parascope : MonoBehaviour
{
    public PlayerMovement player;
    public RectTransform interactPromptUI;
    private BoxCollider2D interactArea;
    private bool inInteractArea;
    public Camera mainCam;
    private CameraPositionHandler cameraBounds;
    private bool parascopeControl;
    public float panSpeed = 100;
    private Vector3 holdplayer = Vector3.zero; //this is just to fix a weird bug where the player will slide away from the interactable

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
                if(player == null)
                {
                    player = FindObjectOfType<PlayerMovement>();// to fix it breaking on death
                }
                if (PlayerMovement.controlsEnabled)
                {
                    parascopeControl = true;
                    PlayerMovement.DisableControls();
                    holdplayer = player.transform.position;
                    cameraBounds.wantedPositionOverride = player.transform.position;
                }
                else
                {
                    parascopeControl = false;
                    PlayerMovement.EnableControls();
                    cameraBounds.wantedPositionOverride = null;
                }
            }
            if (parascopeControl)
            {
                player.transform.position = holdplayer;
                float moveVerticalRaw = Input.GetAxisRaw("Vertical");
                float moveHorizontalRaw = Input.GetAxisRaw("Horizontal");
                Vector3 wantedPos = (Vector3) cameraBounds.wantedPositionOverride + new Vector3 (moveHorizontalRaw * panSpeed * Time.deltaTime, moveVerticalRaw * panSpeed * Time.deltaTime,0);
                wantedPos = new Vector3(Mathf.Clamp(wantedPos.x, cameraBounds.xLimitMin+1, cameraBounds.xLimitMax + 1), Mathf.Clamp(wantedPos.y, cameraBounds.yLimitMin, cameraBounds.yLimitMax));//+1 plus fixes weird vibrating bug?
                cameraBounds.wantedPositionOverride = wantedPos;
            }
        }
    }
}