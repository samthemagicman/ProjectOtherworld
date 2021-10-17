using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;
public class PlayerInteractorHandler : MonoBehaviour
{
    [HideInInspector]
    public Interactable currentInteractable;
    DialogueRunner dialogueRunner;
    public static PlayerInteractorHandler singleton;
    public static UnityEvent interactableInRange = new UnityEvent();
    public static UnityEvent interactableOutOfRange = new UnityEvent();
    public static UnityEvent interactableActivated = new UnityEvent();

    void Start()
    {
        singleton = this;
    }

    private void Awake()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Interactable interactable;
        collision.gameObject.TryGetComponent<Interactable>(out interactable);
        if (interactable)
        {
            currentInteractable = interactable;
            if (currentInteractable != null) interactableInRange.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Interactable interactable = collision.gameObject.GetComponent<Interactable>();
        if (interactable == currentInteractable)// currentNPCDialogue && currentNPCDialogue.GetComponent<Collider2D>() && collision == currentNPCDialogue.GetComponent<Collider2D>())
        {
            currentInteractable = null;
            interactableOutOfRange.Invoke();
        }
    }

    void OnInteractButton()
    {
        if (currentInteractable == null) return;
        currentInteractable.Activate();
        interactableActivated.Invoke();
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            OnInteractButton();
        }
    }
}
