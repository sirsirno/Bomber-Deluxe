using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandCheck : MonoBehaviour
{
    [SerializeField]
    private GameObject player = null;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "JumpTrigger")
        {
            if (player.GetComponent<Animator>().GetInteger("PlayerAnimation") == 2)
            {
                if (player.GetComponent<PlayerController>().state == PlayerController.PlayerState.Grounded)
                {
                    player.GetComponent<Animator>().Play("Player_AfterJumpWait");
                    player.GetComponent<Animator>().SetInteger("PlayerAnimation", 3);
                    Debug.Log("ㅎㅇ");
                }
            }
        }
    }
}
