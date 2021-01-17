using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStopEvent : MonoBehaviour
{
    private GameObject player = null;
    private Animator animator = null;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = player.GetComponent<Animator>();
    }
    private void PlayerStop()
    {
        transform.parent.gameObject.GetComponent<PlayerController>().controlEnabled = false;
    }

    private void PlayerResume()
    {
        transform.parent.gameObject.GetComponent<PlayerController>().controlEnabled = true;
        animator.SetInteger("PlayerAnimation", 0);
    }
}
