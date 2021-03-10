using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    int _currentHealth = 4;
    int currentHealth
    {
        get { return _currentHealth; }
        set {
            _currentHealth = Mathf.Clamp(value, 0, 4);
            if (_currentHealth == 0)
            {
                GetComponent<PlayerDeathHandler>().Die();
            }
        }
    }

    float lastDamageTime = 0;

    public void Damage(int dmg)
    {
        if (Time.realtimeSinceStartup - lastDamageTime > 0.1f)
        {
            lastDamageTime = Time.realtimeSinceStartup;
            currentHealth -= dmg;
        }
    }

    public int GetHealth()
    {
        return currentHealth;
    }
}
