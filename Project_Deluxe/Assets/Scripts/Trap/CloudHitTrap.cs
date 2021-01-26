using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudHitTrap : MonoBehaviour
{
    [SerializeField]
    private Sprite cloudTrap = null;
    private bool respawn = false;
    private GameObject player = null;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("PlayerController");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerController")
        {
            if (PlayerController.Instance.sleeping)
            {
                respawn = true;
                GameObject.FindGameObjectWithTag("PlayerController").GetComponent<HitDeath>().HitDeathPlay();
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = cloudTrap;
                GameObject.FindGameObjectWithTag("PlayerController").GetComponent<HitDeath>().HitDeathPlay();
            }
        }
    }

    private void Update()
    {
        if (player.GetComponent<PlayerController>().awake != false && respawn != false) // 함정 리셋
        {
            if (transform.childCount != 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.Log("에러");
            }
            respawn = false;
        }
    }
}
