using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCoin : MonoBehaviour
{
    private ScoreManager scoreManager;

    private void Awake()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    public float offEffectTime=2.5f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerController")
        {
            if (!PlayerController.Instance.sleeping)
            {
                if (gameObject.tag == "BadCoin")
                {
                    PlayerController.Instance.badCoinGetAudio.Play();

                    UIManager.Instance.badEatEffect.gameObject.SetActive(true);
                    Invoke("OffBadEffect", offEffectTime);
                    gameObject.SetActive(false);
                    return;
                }
                PlayerController.Instance.coinGetAudio.Play();

                UIManager.Instance.parEatEffect.gameObject.SetActive(true);
                Invoke("OffEffect", offEffectTime);

                scoreManager.ScoreValueSet(ScoreManager.ScoreType.FEED, ScoreManager.SetType.ADD, 1);
                PlayerController.Instance.ShowFeedRemaining();

                gameObject.SetActive(false);
                Invoke("DeleteObject", offEffectTime);
            }
        }
    }
    private void OffBadEffect(){
        UIManager.Instance.badEatEffect.gameObject.SetActive(false);
        return;
    }
    private void OffEffect(){
        UIManager.Instance.parEatEffect.gameObject.SetActive(false);
        
        return;
    }
    private void DeleteObject(){
        Destroy(gameObject);
    }
}
