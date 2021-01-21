using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandCheck : MonoBehaviour
{
    [SerializeField]
    private GameObject player = null;
    [SerializeField]
    private GameObject playerController = null;

    private Rigidbody2D rigid = null;

    private void Start()
    {
        rigid = playerController.GetComponent<Rigidbody2D>();    
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "JumpTrigger")
        {
            if (player.GetComponent<Animator>().GetInteger("PlayerAnimation") == 2)
            {
                if (playerController.GetComponent<PlayerController>().state == PlayerController.PlayerState.Grounded)
                {
                    if (rigid.velocity.y < -8f)
                    {
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
    }
}
