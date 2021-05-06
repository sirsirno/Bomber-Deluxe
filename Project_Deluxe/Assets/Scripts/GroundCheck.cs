using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField]
    private GameObject player = null;

    public List<Collider2D> colliders = new List<Collider2D>();
    private Rigidbody2D rigid = null;
    private bool isLandDestiny = false;

    private void Start()
    {
        rigid = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<SpriteRenderer>() != null && collision.GetComponent<InvisibleBlockTrap>() == null)
            if (collision.gameObject.GetComponent<SpriteRenderer>().color.a == 0)
                return;
        if (collision.gameObject.tag == "JumpTrigger")
            colliders.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<SpriteRenderer>() != null && collision.GetComponent<InvisibleBlockTrap>() == null)
            if (collision.gameObject.GetComponent<SpriteRenderer>().color.a == 0)
                return;
        if (collision.gameObject.tag == "JumpTrigger")
            colliders.Remove(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (player.GetComponent<Animator>().GetInteger("PlayerAnimation") == 2)
        {
            if (rigid.gameObject.GetComponent<PlayerController>().state == PlayerController.PlayerState.Grounded)
            {
                if (isLandDestiny)
                {
                    isLandDestiny = false;
                    player.GetComponent<Animator>().Play("Player_AfterJumpWait");
                    player.GetComponent<Animator>().SetInteger("PlayerAnimation", 3);
                    PlayerController.Instance.landAudio.Play();
                }
                else
                {
                    player.GetComponent<Animator>().SetInteger("PlayerAnimation", 0);
                }
            }
        }
    }

    private void Update()
    {
        if (rigid.velocity.y < -15)
        {
            isLandDestiny = true;
        }

        if (colliders.Count==0)
        {
            PlayerController.Instance.state = PlayerController.PlayerState.Jumping;
        }
        else
        {
            if(rigid.velocity.y <= 0f)
                PlayerController.Instance.state = PlayerController.PlayerState.Grounded;
        }


    }

}
