using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakawayPlatform : MonoBehaviour
{
    public float regenTime = 0;
    public float breakawayTime = 1;
    Color regularColor;
    public Color breakawayColor = new Color(1, 0, 0, 0.4f);

    public bool regenOnDeath = true;

    bool debounce = false;

    Collider2D collider;
    SpriteRenderer sprite;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !debounce)
        {
            debounce = true;
            StartCoroutine("RegenPlatform");
        }
    }

    IEnumerator RegenPlatform()
    {
        yield return new WaitForSeconds(breakawayTime);
        collider.enabled = false;
        regularColor = sprite.color;
        sprite.color = breakawayColor;
        yield return new WaitForSeconds(regenTime);
        Regen();
    }

    void Regen()
    {
        collider.enabled = true;
        sprite.color = regularColor;
        debounce = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        if (regenOnDeath) PlayerDeathHandler.onDeath.AddListener(Regen);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
