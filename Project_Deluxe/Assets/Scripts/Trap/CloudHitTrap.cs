using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudHitTrap : MonoBehaviour
{
    [SerializeField]
    private Sprite cloudTrap = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerController")
        {
            GetComponent<SpriteRenderer>().sprite = cloudTrap;
            GameObject.FindGameObjectWithTag("PlayerController").GetComponent<HitDeath>().HitDeathPlay();
        }
    }
}
