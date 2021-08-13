using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class OnTriggerEvent : MonoBehaviour
{
    public UnityEvent<Collider2D> OnTriggerEnter2DEvent = new UnityEvent<Collider2D>();
    public UnityEvent<Collider2D> OnTriggerExit2DEvent = new UnityEvent<Collider2D>();


    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEnter2DEvent.Invoke(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnTriggerExit2DEvent.Invoke(collision);

    }
}
