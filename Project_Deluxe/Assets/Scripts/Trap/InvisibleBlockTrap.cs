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
        if (player.GetComponent<PlayerController>().awake != false && respawn) // 함정 리셋
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerController")
        {
            if (player.GetComponent<PlayerController>().sleeping != false)
            {
                respawn = true;
                Invoke("Respawn", 0.01f);
            }
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }
    private void Respawn()
    {
        respawn = false;
    }
}
