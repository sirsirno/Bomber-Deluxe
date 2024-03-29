using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDeath : MonoBehaviour
{
    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private ParticleSystem Par0;
    [SerializeField]
    private GameObject sleepingPlayer = null;
    public float effectDelay = 3f;
    public float DeathPanelDelayTime = 4f;
    private GroundCheck groundCheck = null;
    private GameObject player = null;
    private GameObject playerController = null;

    void Start()
    {
        groundCheck = FindObjectOfType<GroundCheck>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = GameObject.FindGameObjectWithTag("PlayerController");
    }

    public void HitDeathPlay()
    {
        if (player.GetComponent<Animator>().GetInteger("PlayerAnimation") != 4)
            PlayerController.Instance.ouchAudio.Play();
        player.GetComponent<Animator>().SetInteger("PlayerAnimation", 4);
        player.GetComponent<Animator>().Play("Player_Death");
        PlayerController.Instance.jumpWingAudio.Stop();
        groundCheck.GetComponent<GroundCheck>().enabled = false;

        if (!PlayerController.Instance.sleeping)
        {
            Par0.gameObject.GetComponent<ParticleSystemRenderer>().sortingOrder = 22;
            player.GetComponent<SpriteRenderer>().sortingOrder = 22;
            playerController.GetComponent<PlayerController>().state = PlayerController.PlayerState.Dead;
            canvas.SetActive(true);
            Invoke("CheckEffect", 0.65f);
            Invoke("DeathPanelFalse", DeathPanelDelayTime);
        }
    }

    [ContextMenu("미래여도 죽이기")]
    public void RealPlayerHitDeathPlay()
    {
        sleepingPlayer.GetComponent<Animator>().Play("SleepingPlayer_Dead");
        sleepingPlayer.transform.GetChild(0).gameObject.SetActive(false);
            PlayerController.Instance.ouchAudio.Play();
        player.GetComponent<Animator>().SetInteger("PlayerAnimation", 4);
        player.GetComponent<Animator>().Play("Player_Death");
        PlayerController.Instance.jumpWingAudio.Stop();
        groundCheck.GetComponent<GroundCheck>().enabled = false;
        Par0.gameObject.GetComponent<ParticleSystemRenderer>().sortingOrder = 22;
        player.GetComponent<SpriteRenderer>().sortingOrder = 22;
        playerController.GetComponent<PlayerController>().state = PlayerController.PlayerState.Dead;
        canvas.SetActive(true);
        Invoke("CheckEffect", 0.65f);
        Invoke("DeathPanelFalse", DeathPanelDelayTime);
    }

    private void CheckEffect()
    {
        if (Par0 != null)
        {
            Par0.gameObject.SetActive(true);
            Invoke("EffectFalseDelay", DeathPanelDelayTime);
            ParticleSystem.MainModule main = Par0.main;
            main.startLifetime = effectDelay;
        }

    }
    private void DeathPanelFalse()
    {
        canvas.SetActive(false);
        EffectFalseDelay();
        return;
    }
    private void EffectFalseDelay()
    {
        Par0.gameObject.SetActive(false);
    }
}
