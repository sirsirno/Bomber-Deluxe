using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 나타낼 함정들의 부모(또는 자신)에 이 스크립트를 넣을것.
/// </summary>
public class AppearTrap : MonoBehaviour
{
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerController")
        {
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
