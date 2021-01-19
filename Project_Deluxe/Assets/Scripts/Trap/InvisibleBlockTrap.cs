using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleBlockTrap : MonoBehaviour
{
    [SerializeField]
    private GameObject player = null;

    private bool respawn = false;

    private void Update()
    {
        if (player.GetComponent<PlayerController>().awake != false && respawn != false) // 함정 리셋
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            respawn = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerController")
        {
            if (player.GetComponent<PlayerController>().sleeping != false)
            {
                respawn = true;
            }
            else
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }
}
