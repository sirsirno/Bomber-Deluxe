using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public List<Collider2D> colliders = new List<Collider2D>();
    private Rigidbody2D rigid = null;

    private void Start()
    {
        rigid = transform.parent.gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "JumpTrigger")
            colliders.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "JumpTrigger")
            colliders.Remove(collision);
    }

    private void Update()
    {
        if(colliders.Count==0)
        {
            PlayerController.Instance.state = PlayerController.PlayerState.Jumping;
        }
        else
        {
            if(rigid.velocity.y < 0)
                PlayerController.Instance.state = PlayerController.PlayerState.Grounded;
        }
    }

}
