using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField]
    private float jumpSpeed = 5;

    [Header("true일경우 스프링 작동안함")]
    [SerializeField]
    private bool isTrapSpring = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerController" && !isTrapSpring)
        {
            GetComponent<Animator>().Play("Trap_Spring");
            PlayerController.Instance.springAudio.Play();
            GameObject.FindGameObjectWithTag("PlayerController").GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GameObject.FindGameObjectWithTag("PlayerController").GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        }
    }
}
