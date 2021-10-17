using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractPromptUI : MonoBehaviour
{
    public static InteractPromptUI singleton;
    GameObject firstChild;
    RectTransform rectTransform;

    bool interactableInRange = false;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        firstChild = transform.GetChild(0).gameObject;
        singleton = this;

        PlayerInteractorHandler.interactableInRange.AddListener(onInteractableInRange);
        PlayerInteractorHandler.interactableOutOfRange.AddListener(onInteractableOutOfRange);
        PlayerInteractorHandler.interactableActivated.AddListener(onInteractableActivated);
    }

    private void Update()
    {
        if (interactableInRange && PlayerInteractorHandler.singleton.currentInteractable)
        {
            updatePosition(RectTransformUtility.WorldToScreenPoint(Camera.main, PlayerInteractorHandler.singleton.currentInteractable.transform.position + PlayerInteractorHandler.singleton.currentInteractable.interactUIOffset));
        }
    }

    void onInteractableInRange()
    {
        interactableInRange = true;
        showUI(RectTransformUtility.WorldToScreenPoint(Camera.main, PlayerInteractorHandler.singleton.currentInteractable.transform.position + PlayerInteractorHandler.singleton.currentInteractable.interactUIOffset));
    }

    void onInteractableOutOfRange()
    {
        interactableInRange = false;
        hideUI();
    }

    void onInteractableActivated()
    {
        hideUI();
    }

    public void updatePosition(Vector2 pos)
    {
        rectTransform.position = pos;
    }

    public void showUI()
    {
        firstChild.SetActive(true);
    }

    public void showUI(Vector2 pos)
    {
        updatePosition(pos);
        showUI();
    }
    
    public void hideUI()
    {
        firstChild.SetActive(false);
    }
}
