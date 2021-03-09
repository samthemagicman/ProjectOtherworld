using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IDamageable
{
    public int _health = 4;
    public int health { get { return _health; } set { _health = value; } }

    public UnityEvent onDamaged = new UnityEvent();

    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void damage(int amount)
    {
        health -= amount;
        onDamaged.Invoke();

        animator.SetTrigger("Damage");

        if (health == 0)
        {
            Destroy(this.gameObject);
        }
    }
}
