using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDeathHandler : MonoBehaviour
{
    public static PlayerDeathHandler singleton;
    public static UnityEvent onDeath = new UnityEvent();
    public GameObject explodingPlayerPrefab;
    public UnityEvent onDied;

    private void Start()
    {
        singleton = this;
    }

    void Update()
    {
        if (Input.GetButtonDown("ResetCharacter"))
        {
            Die();
        }
    }

    public void Die()
    {
        GameObject explodingPlayer = Instantiate(explodingPlayerPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        Explodable explodable = explodingPlayer.GetComponent<Explodable>();
        explodable.fragmentInEditor();
        explodable.explode();
        explodable.setVelocity(GetComponent<Rigidbody2D>().velocity);
        //Destroy(this.gameObject);
        onDied.Invoke();
        onDeath.Invoke();
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
        this.gameObject.SetActive(false);
    }
}
