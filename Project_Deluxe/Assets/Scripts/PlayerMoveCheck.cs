using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveCheck : MonoBehaviour
{
    /// <summary>
    /// 플레이어 양옆으로 최고 속도
    /// </summary>
    public float moveSpeed = 5f;

    private GameObject realPlayer; //직접 보이는 플레이어 오브젝트

    bool cantGoLeft = false;
    bool cantGoRight = false;

    void Awake()
    {
        realPlayer = transform.parent.gameObject;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "JumpTrigger")
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                cantGoLeft = true;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                cantGoRight = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "JumpTrigger")
        {
            cantGoLeft = false;
            cantGoRight = false;
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))    //왼쪽화살표 입력시 실행함
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

            if (!cantGoLeft)
            {
                realPlayer.transform.position = new Vector3(realPlayer.transform.position.x + transform.localPosition.x, realPlayer.transform.position.y + transform.localPosition.y, realPlayer.transform.position.z + transform.localPosition.z);
                transform.localPosition = new Vector3(0, 0, 0);
            }
           realPlayer.GetComponent<SpriteRenderer>().flipX = true;
        }

        if (Input.GetKey(KeyCode.RightArrow))    //오른쪽화살표 입력시 실행함
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

            if (!cantGoRight)
            {
                realPlayer.transform.position = new Vector3(realPlayer.transform.position.x + transform.localPosition.x, realPlayer.transform.position.y + transform.localPosition.y, realPlayer.transform.position.z + transform.localPosition.z);
                transform.localPosition = new Vector3(0, 0, 0);
            }

            realPlayer.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
}