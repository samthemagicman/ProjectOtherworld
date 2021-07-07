using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

[ RequireComponent( typeof( Collider2D ) ) ]
public class NPCDialogue : MonoBehaviour
{
    public string nodeTitle;
    public YarnProgram yarnDialogue;
    public Vector3 interactUIOffset;
    bool isTalking = false;
    bool isBeingInteractedWith = false;

    // Start is called before the first frame update
    void Start()
    {
        if (yarnDialogue != null)
        {
            DialogueRunner dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
            dialogueRunner.Add(yarnDialogue);
        }
    }

    public static void SetTalking(bool talking)
    {
        NPCDialogue curDialogue = PlayerDialogueInteractor.singleton.currentNPCDialogue;
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
        NPCDialogue curDialogue = PlayerDialogueInteractor.singleton.currentNPCDialogue;
        curDialogue.isBeingInteractedWith = interactingWith;
    }

    private void OnDrawGizmos()
    {
        if (UnityEditor.Selection.activeGameObject != this.gameObject) return;
        Gizmos.DrawCube(transform.position + interactUIOffset, new Vector3(1.5f, 0.5f, 1));
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position + interactUIOffset, new Vector3(1.5f, 0.1f, 1));
    }
}
