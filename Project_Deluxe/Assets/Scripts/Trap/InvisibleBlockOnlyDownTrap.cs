using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleBlockOnlyDownTrap : MonoBehaviour
{
    [SerializeField]
    private GameObject player = null;

    private bool respawn = false;

    private Vector2 defaultOffset = Vector2.zero;
    private Vector2 defaultsize = Vector2.zero;
    private void Awake()
    {
        defaultOffset = GetComponent<BoxCollider2D>().offset;
        defaultsize = GetComponent<BoxCollider2D>().size;
    }
    private void Update()
    {
        if (player.GetComponent<PlayerController>().awake != false && respawn) // 함정 리셋
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            GetComponent<BoxCollider2D>().isTrigger = true;
            GetComponent<BoxCollider2D>().offset = defaultOffset;
            GetComponent<BoxCollider2D>().size = defaultsize;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerHead" && collision.transform.parent.transform.parent.GetComponent<Rigidbody2D>().velocity.y > -0.1f)
        {
            if (player.GetComponent<PlayerController>().sleeping != false)
            {
                respawn = true;
                Invoke("Respawn", 0.01f);
            }
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            GetComponent<BoxCollider2D>().isTrigger = false;
            GetComponent<BoxCollider2D>().offset = Vector2.zero;
            GetComponent<BoxCollider2D>().size = Vector2.one;
        }
    }
    private void Respawn()
    {
        respawn = false;
    }
}
