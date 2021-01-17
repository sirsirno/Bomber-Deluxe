using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private GroundCheck groundCheck = null;
    private GameObject player = null;
    private GameObject playerController = null;
    void Start()
    {
        groundCheck = FindObjectOfType<GroundCheck>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = GameObject.FindGameObjectWithTag("PlayerController");

        GetComponent<BoxCollider2D>().size = GetComponent<SpriteRenderer>().size;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerController")
        {
            player.GetComponent<Animator>().SetInteger("PlayerAnimation", 5);
            player.GetComponent<Animator>().Play("Player_FallenDeath");
            playerController.GetComponent<Rigidbody2D>().simulated = false;
            groundCheck.GetComponent<GroundCheck>().enabled = false;
            playerController.GetComponent<PlayerController>().state = PlayerController.PlayerState.Dead;
        }
    }
}