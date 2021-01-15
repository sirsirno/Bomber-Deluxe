using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "JumpTrigger")
            PlayerController.Instance.state = PlayerController.PlayerState.Grounded;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "JumpTrigger")
            PlayerController.Instance.state = PlayerController.PlayerState.Jumping;
    }
}
