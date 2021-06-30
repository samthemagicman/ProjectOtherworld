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
    // Start is called before the first frame update
    void Start()
    {
        if (yarnDialogue != null)
        {
            DialogueRunner dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
            dialogueRunner.Add(yarnDialogue);
        }
    }
    private void OnDrawGizmos()
    {
        if (UnityEditor.Selection.activeGameObject != this.gameObject) return;
        Gizmos.DrawCube(transform.position + interactUIOffset, new Vector3(1.5f, 0.5f, 1));
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position + interactUIOffset, new Vector3(1.5f, 0.1f, 1));
    }
}
