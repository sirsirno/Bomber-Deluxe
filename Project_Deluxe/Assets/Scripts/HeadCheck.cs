using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCheck : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "JumpTrigger" && transform.parent.transform.parent.GetComponent<Rigidbody2D>().velocity.y > -0.1f && PlayerController.Instance.state == PlayerController.PlayerState.Jumping)
        {
            PlayerController.Instance.headBlockAudio.Play();
            PlayerController.Instance.isHeadBlocked = true;
        }
    }
}
