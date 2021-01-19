using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 나타낼 함정들의 부모(또는 자신)에 이 스크립트를 넣을것.
/// </summary>
public class AppearTrap : MonoBehaviour
{
    [SerializeField]
    private GameObject player = null;

    private bool respawn = false;
    private void Start()
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
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }
        
        if (GetComponent<BoxCollider2D>() == null)
            Debug.Log("박스 컬라이더가 없음요");
        if (GetComponent<SpriteRenderer>() != null)
            GetComponent<BoxCollider2D>().size = GetComponent<SpriteRenderer>().size;
    }
    private void Update()
    {
        if(player.GetComponent<PlayerController>().awake != false && respawn != false) // 함정 리셋
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerController")
        {
            if (player.GetComponent<PlayerController>().sleeping != false)
            {
                respawn = true;
                //Invoke("Respawn", 0.01f);
            }
            else
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
            if (transform.childCount != 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
            }
            else
            {
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
        }
    }
}
