using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleBlockTrap : MonoBehaviour
{
    private GameObject player = null;

    private bool respawn = false;
    private bool isShow = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerController");
    }

    private void Update()
    {
        if (player.GetComponent<PlayerController>().awake != false && respawn != false) // 함정 리셋
        {
            if (isShow != false)
                return;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            respawn = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerController")
        {
            if (player.GetComponent<PlayerController>().sleeping != false)
            {
                respawn = true;
            }
            else
            {
                isShow = true;
            }
            if(GetComponent<SpriteRenderer>().color == new Color(1,1,1,0))
                PlayerController.Instance.headBlockAudio.Play();
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }
}
