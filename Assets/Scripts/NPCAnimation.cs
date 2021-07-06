using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimation : MonoBehaviour
{
    public string isTalkingParameter = "is_talking";

    public static void SetIsTalking(bool talking)
    {
        NPCAnimation anim;
        Animator animator;
        PlayerDialogueInteractor.singleton.currentNPCDialogue.TryGetComponent<NPCAnimation> (out anim);
        PlayerDialogueInteractor.singleton.currentNPCDialogue.TryGetComponent<Animator> (out animator);

        if (animator && anim)
        {
            animator.SetBool(anim.isTalkingParameter, talking);
        }
    }

}
