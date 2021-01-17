using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DeathZone : MonoBehaviour
{
    public GameObject canvas;
    public ParticleSystem Par0;
    public float effectDelay = 3f;
    public float DeathPanelDelayTime=4f;
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
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerController")
        {
            canvas.SetActive(true);
            Invoke("CheckEffect",0.65f);
            Invoke("DeathPanelFalse",DeathPanelDelayTime);
            player.GetComponent<Animator>().SetInteger("PlayerAnimation", 5);
            player.GetComponent<Animator>().Play("Player_FallenDeath");
            playerController.GetComponent<Rigidbody2D>().simulated = false;
            groundCheck.GetComponent<GroundCheck>().enabled = false;
            playerController.GetComponent<PlayerController>().state = PlayerController.PlayerState.Dead;
        }
    }

    
    private void CheckEffect(){
        if(Par0!=null) {
            Par0.gameObject.SetActive(true);
            Invoke("EffectFalseDelay",DeathPanelDelayTime);
            ParticleSystem.MainModule main = Par0.main;
            main.startLifetime = effectDelay;
        }
        
    }
    private void DeathPanelFalse(){
    canvas.SetActive(false);
    EffectFalseDelay();
    return; 
    }
    private void EffectFalseDelay(){
        Par0.gameObject.SetActive(false);
    }
}