using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject clear;
    [SerializeField]
    private GameObject clockNeedle;

    [SerializeField]
    private Image img;

    private float fillAmount = 0f;


    private Animator animator;
    private GameObject realPlayer = null;
    void Awake()
    {
        realPlayer = GameObject.FindGameObjectWithTag("Player");
        animator = realPlayer.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !menu.activeSelf)
        {
            menu.SetActive(true);
            Time.timeScale = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && menu.activeSelf) 
        {
            menu.SetActive(false);
            Time.timeScale = 1;
        }

        // 시계 관련
        {
            fillAmount = 1 - ((int)PlayerController.Instance.clockDuration / 15f);
            clockNeedle.transform.rotation = Quaternion.Euler(0, 0, -360 * (1 - ((int)PlayerController.Instance.clockDuration / 15f)));

            // 소리만을 위한 코드
            {
                if (PlayerController.Instance.sleeping && (int)PlayerController.Instance.clockDuration != 15 && animator.GetInteger("PlayerAnimation") != 4 && animator.GetInteger("PlayerAnimation") != 5)
                {
                    if ((int)PlayerController.Instance.clockDuration % 2 != 1)
                        AudioManager.Instance.SFX_ClockTicPlay();
                    else
                        AudioManager.Instance.SFX_ClockTocPlay();
                }
                else
                {
                    AudioManager.Instance.SFX_ClockTic.Stop();
                    AudioManager.Instance.SFX_ClockTok.Stop();
                }
            }

            if (PlayerController.Instance.clockDuration <= 0 || PlayerController.Instance.awake)
            {
                fillAmount = 0;
                clockNeedle.transform.rotation = Quaternion.identity;
                PlayerController.Instance.clockDuration = 15;
            }

            img.fillAmount = fillAmount;
        }
        
    }
    private void OnClickContinueBtn() 
    {
        menu.SetActive(false);
        Time.timeScale = 1;
    }
    private void OnClickHomeBtn() 
    {
        menu.SetActive(false);
    }

    private void StageClear() 
    {
        clear.SetActive(true);
    }

    private void OnClickNextBtn() 
    {
        clear.SetActive(false);
        
    }
}
