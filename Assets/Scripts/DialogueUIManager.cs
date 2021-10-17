using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Yarn;
using Yarn.Unity;
public class DialogueUIManager : MonoBehaviour
{
    public TMP_Text characterName;
    Animator setBool;

    public void Awake()
    {
        FindObjectOfType<DialogueRunner>().AddCommandHandler("name", SetCharacterName);
        //FindObjectOfType<DialogueRunner>().AddCommandHandler("NPCAnimation", PlayNPCAnimation);
        FindObjectOfType<DialogueRunner>().AddFunction("NPCAnimation", -1, PlayNPCAnimation);
        // Ex.: NPCAnimation(SetBool,TalkingToPlayer,true)
    }

    public void SetCharacterName(string[] parameters)
    {
        characterName.text = parameters[0];
        LayoutRebuilder.ForceRebuildLayoutImmediate(characterName.GetComponentInParent<RectTransform>());
    }
    public void PlayNPCAnimation(Yarn.Value[] parameters)
    {
        string op = parameters[0].AsString.ToLower();
        if (op == "setbool")
        {
            NPCDialogue curNpc = (NPCDialogue) PlayerDialogueInteractor.singleton.currentInteractable;
            if (curNpc != null)
            {
                Animator animator = curNpc.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.SetBool(parameters[1].AsString, parameters[2].AsString.ToLower() == "true");
                }
            }
        }
    }

    /*public void PlayNPCAnimation(string[] parameters)
    {
        string op = parameters[0].ToLower();
        if (op == "setbool")
        {
            NPCDialogue curNpc = PlayerDialogueInteractor.singleton.currentNPCDialogue;
            if (curNpc != null)
            {
                Animator animator = curNpc.GetComponent<Animator>();
                animator.SetBool(parameters[1], parameters[2].ToLower() == "true");
            }
        }
    }*/
}
