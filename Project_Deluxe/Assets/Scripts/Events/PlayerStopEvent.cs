using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStopEvent : Singleton<PlayerStopEvent>
{
    private GameObject playerController = null;
    private Animator animator = null;
    private GroundCheck groundCheck = null;
    public bool isFutureDead = false;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("PlayerController");
        animator = GetComponent<Animator>();
        groundCheck = FindObjectOfType<GroundCheck>();
    }
    private void PlayerStop()
    {
        playerController.GetComponent<PlayerController>().controlEnabled = false;
    }

    private void PlayerResume()
    {
        playerController.GetComponent<PlayerController>().controlEnabled = true;
        animator.SetInteger("PlayerAnimation", 0);
    }

    private void BackToNow()
    {
        if (PlayerController.Instance.sleeping)
        {
            GetComponent<Animator>().SetInteger("PlayerAnimation", 0);
            GetComponent<Animator>().Play("Player_Idle");
            groundCheck.GetComponent<GroundCheck>().enabled = true;
            playerController.GetComponent<Rigidbody2D>().simulated = true;
            isFutureDead = true;
        }
    }

    private void WingSoundPlay()
    {
        if(!PlayerController.Instance.jumpWingAudio.isPlaying)
        PlayerController.Instance.jumpWingAudio.Play();
    }
}
