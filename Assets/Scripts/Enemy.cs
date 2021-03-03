using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public int _health = 4;
    public int health { get { return _health; } set { _health = value; } }

    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void damage(int amount)
    {
        health -= amount;
        animator.Play("DummyDamage");
        animator.SetTrigger("New Trigger");
        /*
        if (health == 0)
        {
            Destroy(this.gameObject);
        }*/
    }
}
