using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

[ RequireComponent( typeof( Collider2D ) ) ]
public class NPCDialogue : Interactable
{
    public string nodeTitle;
    public YarnProgram yarnDialogue;
    bool isTalking = false;
    bool isBeingInteractedWith = false;

    // Start is called before the first frame update
    void Start()
    {
        onInteractectedWith.AddListener(() =>
        {
            if (!FindObjectOfType<DialogueRunner>().IsDialogueRunning)
            {
                FindObjectOfType<DialogueRunner>().StartDialogue(nodeTitle);
            }
            else
            {
                FindObjectOfType<DialogueUI>().MarkLineComplete();
            }
        });

        if (yarnDialogue != null)
        {
            DialogueRunner dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
            dialogueRunner.Add(yarnDialogue);
        }
    }

   public static void SetTalking(bool talking)
    {
        NPCDialogue curDialogue = (NPCDialogue) PlayerDialogueInteractor.singleton.currentInteractable;
        curDialogue.isTalking = talking;
        NPCVoice voice = curDialogue.GetComponent<NPCVoice>();
        if (voice)
        {
            voice.SetTalking(talking);
        }

        NPCAnimation anim = curDialogue.GetComponent<NPCAnimation>();
        if (anim)
        {
            anim.SetTalking(talking);
        }
    }

    public static void SetBeingInteractedWith(bool interactingWith)
    {
        NPCDialogue curDialogue = (NPCDialogue) PlayerDialogueInteractor.singleton.currentInteractable;
        curDialogue.isBeingInteractedWith = interactingWith;
    }
}
