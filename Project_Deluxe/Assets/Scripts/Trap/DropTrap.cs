using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DropTrap : MonoBehaviour
{
    public enum TrapType
    {
        PARENT,
        CHILD
    }

    GameObject Trap = null;
    public TrapType trapType = TrapType.PARENT;

    private void Awake()
    {
        if(trapType == TrapType.PARENT)
            Trap = transform.parent.gameObject;
        if (GetComponent<SpriteRenderer>() != null)
            GetComponent<BoxCollider2D>().size = GetComponent<SpriteRenderer>().size; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerController")
        {
            if (trapType == TrapType.PARENT)
            {
                Trap.AddComponent<Rigidbody2D>();
                Trap.GetComponent<Rigidbody2D>().gravityScale = 1f;
                Trap.GetComponent<Rigidbody2D>().mass = 0;
                Trap.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX + 4;
            }
            else
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.AddComponent<Rigidbody2D>();
                    transform.GetChild(i).gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.5f;
                    transform.GetChild(i).gameObject.GetComponent<Rigidbody2D>().mass = 0;
                    transform.GetChild(i).gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX + 4;
                }
            }
        }
    }
}
