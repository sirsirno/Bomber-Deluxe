using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTrap : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerController")
        {
            GameObject.FindGameObjectWithTag("PlayerController").GetComponent<HitDeath>().HitDeathPlay();
        }
        if (collision.gameObject.tag == "SleepingPlayer")
        {
            GameObject.FindGameObjectWithTag("PlayerController").GetComponent<HitDeath>().RealPlayerHitDeathPlay();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerController")
        {
            GameObject.FindGameObjectWithTag("PlayerController").GetComponent<HitDeath>().HitDeathPlay();
        }
        if (collision.gameObject.tag == "SleepingPlayer")
        {
            GameObject.FindGameObjectWithTag("PlayerController").GetComponent<HitDeath>().RealPlayerHitDeathPlay();
        }
    }
}
