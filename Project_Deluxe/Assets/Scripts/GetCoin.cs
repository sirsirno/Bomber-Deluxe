using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCoin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerController")
        {
            Destroy(gameObject);
            GameManager.Instance.AccessSetCoin(GameManager.SETTYPE.ADD, 1);
        }
    }
}
