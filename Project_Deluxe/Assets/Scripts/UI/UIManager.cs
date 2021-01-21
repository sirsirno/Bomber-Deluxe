using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private Text lifeTxt;
    [SerializeField]
    private Text feedTxt;
    [SerializeField]
    private GameObject fail;
    [SerializeField]
    private GameObject[] stars = null;

    [SerializeField]
    private Image img;

    private float fillAmount = 0f;


    private Animator animator;
    private GameObject realPlayer = null;
    private GameManager gameManager;
    void Awake()
    {
        realPlayer = GameObject.FindGameObjectWithTag("Player");
        animator = realPlayer.GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
    public void OnClickContinueBtn() 
    {
        menu.SetActive(false);
        Time.timeScale = 1;
    }
    public void OnClickHomeBtn() 
    {
        SceneManager.LoadScene(0);
        menu.SetActive(false);
        clear.SetActive(false);
    }

    public void StageClear() 
    {
        // 별 계산
        {
            stars[0].SetActive(true);
            stars[1].SetActive(true);
            stars[2].SetActive(true);
            if (gameManager.coin < 4 && gameManager.life < 2)
            {
                stars[1].SetActive(false);
                stars[2].SetActive(false);
                // TO DO : 별 저장
                JsonSave.Instance.gameData.StageSetValueSave(GameData.StageValueType.STAR, GameManager.Instance.GetCurrentStage(), 1);
            }
            else if (gameManager.coin == 8 && gameManager.life >= 4)
            {
                JsonSave.Instance.gameData.StageSetValueSave(GameData.StageValueType.STAR, GameManager.Instance.GetCurrentStage(), 3);
            }
            else
            {
                stars[2].SetActive(false);
                JsonSave.Instance.gameData.StageSetValueSave(GameData.StageValueType.STAR, GameManager.Instance.GetCurrentStage(), 2);
            }
        }


        lifeTxt.text ="x " +gameManager.life;
        feedTxt.text = "x " + gameManager.coin;
        Time.timeScale = 0;
        clear.SetActive(true);
    }

    public void OnClickNextBtn() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        clear.SetActive(false);
        
    }
    public void OncClickRetryBtn() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
        clear.SetActive(false);
    }
    public void OnClickExitBtn() 
    {
        Application.Quit();
    }
    public void StageFail() 
    {
        fail.SetActive(true);
    }
}
