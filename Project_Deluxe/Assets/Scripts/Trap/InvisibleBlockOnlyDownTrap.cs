using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleBlockOnlyDownTrap : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerController")
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerController" && collision.gameObject.GetComponent<Rigidbody2D>().velocity.y > 0)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            GetComponent<BoxCollider2D>().isTrigger = false;
            GetComponent<BoxCollider2D>().offset = Vector2.zero;
            GetComponent<BoxCollider2D>().size = Vector2.one;
        }
    }
}
