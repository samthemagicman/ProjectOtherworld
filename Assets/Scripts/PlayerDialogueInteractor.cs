using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
public class PlayerDialogueInteractor : MonoBehaviour
{
    public RectTransform interactPromptUI;
    [HideInInspector]
    public Interactable currentInteractable;
    DialogueRunner dialogueRunner;
    public static PlayerDialogueInteractor singleton;

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
        Interactable npcDial;
        collision.gameObject.TryGetComponent<Interactable>(out npcDial);
        if (npcDial && !dialogueRunner.IsDialogueRunning)
        {
            currentInteractable = npcDial;
            interactPromptUI.gameObject.SetActive(true);
            //Debug.Log(currentNPCDialogue.yarnDialogue.GetProgram().Nodes["String.Paul"].Tags);
        }
        else
        {
            //interactPromptUI.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Interactable interactable;
        collision.gameObject.TryGetComponent<Interactable>(out interactable);
        if (interactable == currentInteractable)// currentNPCDialogue && currentNPCDialogue.GetComponent<Collider2D>() && collision == currentNPCDialogue.GetComponent<Collider2D>())
        {
            interactPromptUI.gameObject.SetActive(false);
            currentInteractable = null;
        }
    }

    void OnInteractButton()
    {
        /*if (!FindObjectOfType<DialogueRunner>().IsDialogueRunning)
        {
            interactPromptUI.gameObject.SetActive(false);
            if (currentNPCDialogue != null)
            {
                FindObjectOfType<DialogueRunner>().StartDialogue(currentNPCDialogue.nodeTitle);
            }
        }
        else
        {
            FindObjectOfType<DialogueUI>().MarkLineComplete();
        }*/
        interactPromptUI.gameObject.SetActive(false);
        currentInteractable.Activate();
    }

    void Update()
    {
        if (currentInteractable != null)
        {
            interactPromptUI.position = RectTransformUtility.WorldToScreenPoint(Camera.main, currentInteractable.transform.position + currentInteractable.interactUIOffset);
        }
        if (Input.GetButtonDown("Interact"))
        {
            OnInteractButton();
        }
    }
}
