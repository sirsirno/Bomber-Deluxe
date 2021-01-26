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
        if (PlayerController.Instance.sleeping && PlayerController.Instance.state != PlayerController.PlayerState.Dead)
        {
            GetComponent<Animator>().SetInteger("PlayerAnimation", 0);
            GetComponent<Animator>().Play("Player_Idle");
            isFutureDead = true;
        }
        else
        {
            GameManager.Instance.PlayerDeadState();
        }
    }

    private void WingSoundPlay()
    {
        if(!PlayerController.Instance.jumpWingAudio.isPlaying)
        PlayerController.Instance.jumpWingAudio.Play();
    }
}
