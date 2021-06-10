using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDeathHandler : MonoBehaviour
{
    public GameObject explodingPlayerPrefab;
    public UnityEvent onDied;
    public GameObject bone1;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
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
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
        this.gameObject.SetActive(false);
        bone1.transform.eulerAngles = new Vector3(0, 0, 90);
    }
}
