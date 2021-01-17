using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DropTrap : MonoBehaviour
{
    GameObject Trap = null;

    private void Awake()
    {
        Trap = transform.parent.gameObject;
        GetComponent<BoxCollider2D>().size = GetComponent<SpriteRenderer>().size; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Trap.AddComponent<Rigidbody2D>();
        Trap.GetComponent<Rigidbody2D>().gravityScale = 1f;
        Trap.GetComponent<Rigidbody2D>().mass = 0;
        Trap.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX + 4;
    }
}
