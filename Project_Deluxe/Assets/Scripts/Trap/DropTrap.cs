using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DropTrap : MonoBehaviour
{
    [SerializeField]
    private GameObject player = null;

    private bool respawn = false;

    private Vector3[] defaultposition = new Vector3[2];

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
        if(trapType == TrapType.PARENT)
        {
            defaultposition[0] = Trap.transform.position;
        }
        else
        {
            for(int i = 0; i <transform.childCount; i++)
            {
                defaultposition[i] = transform.GetChild(i).transform.position;
            }
        }
    }

    private void Update()
    {
        if (player.GetComponent<PlayerController>().awake != false && respawn != false) // 함정 리셋
        {
            if (trapType == TrapType.PARENT)
            {
                if (Trap.GetComponent<Rigidbody2D>() == null)
                    Trap.AddComponent<Rigidbody2D>();
                Trap.GetComponent<Rigidbody2D>().gravityScale = 0f;
                Trap.GetComponent<Rigidbody2D>().mass = 1;
                Trap.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                Trap.transform.position = defaultposition[0];
            }
            else
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).gameObject.GetComponent<Rigidbody2D>() == null)
                        transform.GetChild(i).gameObject.AddComponent<Rigidbody2D>();
                    transform.GetChild(i).gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
                    transform.GetChild(i).gameObject.GetComponent<Rigidbody2D>().mass = 1;
                    transform.GetChild(i).gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
                    transform.GetChild(i).gameObject.transform.position = defaultposition[i];
                }
            }
            respawn = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
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

            if (trapType == TrapType.PARENT)
            {
                if (Trap.GetComponent<Rigidbody2D>() == null)
                    Trap.AddComponent<Rigidbody2D>();
                Trap.GetComponent<Rigidbody2D>().gravityScale = 1f;
                Trap.GetComponent<Rigidbody2D>().mass = 0;
                Trap.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX + 4;
            }
            else
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).gameObject.GetComponent<Rigidbody2D>() == null)
                        transform.GetChild(i).gameObject.AddComponent<Rigidbody2D>();
                    transform.GetChild(i).gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.5f;
                    transform.GetChild(i).gameObject.GetComponent<Rigidbody2D>().mass = 0;
                    transform.GetChild(i).gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX + 4;
                }
            }
        }
    }
}
