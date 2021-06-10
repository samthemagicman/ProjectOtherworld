using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public static Dialogue singleton;

    DialogueText dialogueText;
    Animator dialogueAnimator;
    void Start()
    {
        singleton = this;

        dialogueAnimator = GetComponentInChildren<Animator>();
        dialogueText = GetComponentInChildren<DialogueText>();

        dialogueText.SetText("What did you say to me you stupid little motherfucker? I'll have you know I murdered 17 people in Al-quada");
        
    }

    public static void OpenDialogueBox(string txt)
    {
        singleton.dialogueAnimator.SetBool("Open", true);
    }

    public static void CloseDialogueBox()
    {
        singleton.dialogueAnimator.SetBool("Open", false);
    }
}
