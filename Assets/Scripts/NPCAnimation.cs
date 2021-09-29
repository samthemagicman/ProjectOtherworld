using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimation : MonoBehaviour
{
    public string isTalkingParameter = "is_talking";

    public void SetTalking(bool talking)
    {
        NPCAnimation anim;
        Animator animator;
        TryGetComponent<NPCAnimation> (out anim);
        TryGetComponent<Animator> (out animator);

        if (animator && anim)
        {
            animator.SetBool(anim.isTalkingParameter, talking);
        }
    }

}
