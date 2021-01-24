using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetStamp : MonoBehaviour
{
    [SerializeField]
    private int stampNumber = 0;
    private ScoreManager scoreManager = null;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = GameManager.Instance.GetStampSprite(false, stampNumber);
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    public float offEffectTime = 2.5f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerController")
        {
            if (!PlayerController.Instance.sleeping)
            {
                PlayerController.Instance.stampGetAudio.Play();

                UIManager.Instance.parEatEffect.gameObject.SetActive(true);
                Invoke("OffEffect", offEffectTime);

                if (stampNumber == 0)
                {
                    scoreManager.ScoreValueSet(ScoreManager.ScoreType.STAMPTEMP, ScoreManager.SetType.ADD, 1);
                }
                else if (stampNumber == 1)
                {
                    scoreManager.ScoreValueSet(ScoreManager.ScoreType.STAMPTEMP, ScoreManager.SetType.ADD, 2);
                }
                else
                {
                    scoreManager.ScoreValueSet(ScoreManager.ScoreType.STAMPTEMP, ScoreManager.SetType.ADD, 4);
                }
                UIManager.Instance.StampSpriteSet(false, stampNumber);

                gameObject.SetActive(false);
                Invoke("DeleteObject", offEffectTime);
            }
        }
    }

    private void OffEffect()
    {
        UIManager.Instance.parEatEffect.gameObject.SetActive(false);

        return;
    }
    private void DeleteObject()
    {
        Destroy(gameObject);
    }
}
