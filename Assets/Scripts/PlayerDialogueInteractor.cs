using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
public class PlayerDialogueInteractor : MonoBehaviour
{
    public RectTransform interactPromptUI;
    public NPCDialogue currentNPCDialogue;
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
        collision.gameObject.TryGetComponent<NPCDialogue>(out currentNPCDialogue);
        if (currentNPCDialogue && !dialogueRunner.IsDialogueRunning)
        {
            interactPromptUI.gameObject.SetActive(true);
            //Debug.Log(currentNPCDialogue.yarnDialogue.GetProgram().Nodes["String.Paul"].Tags);
        }
        else
        {
            interactPromptUI.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (currentNPCDialogue && currentNPCDialogue.GetComponent<Collider2D>() && collision == currentNPCDialogue.GetComponent<Collider2D>())
        {
            interactPromptUI.gameObject.SetActive(false);
            currentNPCDialogue = null;
        }
    }

    void Update()
    {
        if (currentNPCDialogue != null)
        {
            interactPromptUI.position = RectTransformUtility.WorldToScreenPoint(Camera.main, currentNPCDialogue.transform.position + currentNPCDialogue.interactUIOffset);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!FindObjectOfType<DialogueRunner>().IsDialogueRunning)
            {
                interactPromptUI.gameObject.SetActive(false);
                if (currentNPCDialogue != null)
                {
                    FindObjectOfType<DialogueRunner>().StartDialogue(currentNPCDialogue.nodeTitle);

                }
            } else
            {
                FindObjectOfType<DialogueUI>().MarkLineComplete();
            }
        }
    }
}