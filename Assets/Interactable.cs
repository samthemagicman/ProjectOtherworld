using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent onInteractectedWith = new UnityEvent();
    public Vector3 interactUIOffset;
    public void Activate()
    {
        onInteractectedWith.Invoke();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (UnityEditor.Selection.activeGameObject != this.gameObject) return;
        Gizmos.DrawCube(transform.position + interactUIOffset, new Vector3(1.5f, 0.5f, 1));
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position + interactUIOffset, new Vector3(1.5f, 0.1f, 1));
    }
#endif
}
