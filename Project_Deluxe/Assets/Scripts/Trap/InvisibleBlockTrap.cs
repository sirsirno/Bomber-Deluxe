using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleBlockTrap : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerController")
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }
}
