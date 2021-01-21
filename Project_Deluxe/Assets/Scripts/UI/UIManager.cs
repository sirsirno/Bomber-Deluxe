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
    private Text FailfeedTxt;
    [SerializeField]
    private GameObject fail;
    [SerializeField]
    private GameObject[] stars = null;

    [SerializeField]
    private Image img;

    private float fillAmount = 0f;
    [Header("스타트 스크린 아웃페이드 시간")]
    [SerializeField]
    private float startScreenOutWaitTime = 1f;
    [SerializeField]
    private float startScreenOutFadeTime = 5f;
    [SerializeField]
    private GameObject startScreenPanel = null;
    private bool isPlayingStartScreen = false;
    [SerializeField]
    private Text startScreenLifeTxt = null;
    [SerializeField]
    private Text startScreenWorldTxt = null;

    private Animator animator;
    private GameObject realPlayer = null;
    private ScoreManager scoreManager;

    private float clockDefaultVolume;

    void Awake()
    {
        startScreenPanel.SetActive(true);
        realPlayer = GameObject.FindGameObjectWithTag("Player");
        animator = realPlayer.GetComponent<Animator>();
        scoreManager = FindObjectOfType<ScoreManager>();
        isPlayingStartScreen = true;
        startScreenLifeTxt.text = string.Format("x    {0}", scoreManager.ScoreValueGet(ScoreManager.ScoreType.LIFE));
        StartScreen_WorldTxtPrint();
        clockDefaultVolume = AudioManager.Instance.SFX_ClockTic.volume;
    }

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
                    {
                        AudioManager.Instance.SFX_ClockTicPlay();
                        if (!menu.activeSelf)
                            AudioManager.Instance.SFX_ClockTok.volume = clockDefaultVolume;
                    }
                    else
                    {
                        AudioManager.Instance.SFX_ClockTocPlay();
                        if (!menu.activeSelf)
                            AudioManager.Instance.SFX_ClockTic.volume = clockDefaultVolume;
                    }
                }
                else
                {
                    AudioManager.Instance.SFX_ClockTic.Stop();
                    AudioManager.Instance.SFX_ClockTok.Stop();
                }

                if (menu.activeSelf)
                {
                    AudioManager.Instance.SFX_ClockTic.volume = 0;
                    AudioManager.Instance.SFX_ClockTok.volume = 0;
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
        
        // 스타트 스크린 아웃페이드
        {
            if (isPlayingStartScreen)
            {
                PlayerController.Instance.controlEnabled = false;
                if (Time.timeSinceLevelLoad >= startScreenOutWaitTime)
                {
                    if (1 - ((Time.timeSinceLevelLoad - startScreenOutWaitTime) / startScreenOutFadeTime) <= 0.88f)
                        startScreenPanel.GetComponent<Image>().color = new Color(0, 0, 0, 1 - ((Time.timeSinceLevelLoad - startScreenOutWaitTime) / startScreenOutFadeTime));
                    for (int i = 0; i < startScreenPanel.transform.childCount; i++)
                    {
                        if (startScreenPanel.transform.GetChild(i).gameObject.GetComponent<Image>() != null)
                        {
                            startScreenPanel.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1 - ((Time.timeSinceLevelLoad - startScreenOutWaitTime) / startScreenOutFadeTime));
                        }
                        else if (startScreenPanel.transform.GetChild(i).gameObject.GetComponent<Text>() != null)
                        {
                            startScreenPanel.transform.GetChild(i).gameObject.GetComponent<Text>().color = new Color(1, 1, 1, 1 - ((Time.timeSinceLevelLoad - startScreenOutWaitTime) / startScreenOutFadeTime));
                        }
                    }

                    if ((Time.timeSinceLevelLoad - startScreenOutWaitTime) >= startScreenOutFadeTime)
                    {
                        isPlayingStartScreen = false;
                        PlayerController.Instance.controlEnabled = true;
                        startScreenPanel.SetActive(false);
                        AudioManager.Instance.BGM_World.Play();
                    }
                }
            }
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
        scoreManager.ScoreValueSet(ScoreManager.ScoreType.FEED, ScoreManager.SetType.SET, 0);
        scoreManager.ScoreValueSet(ScoreManager.ScoreType.LIFE, ScoreManager.SetType.SET, 5);
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
            if (scoreManager.ScoreValueGet(ScoreManager.ScoreType.FEED) < 4 && scoreManager.ScoreValueGet(ScoreManager.ScoreType.LIFE) < 2)
            {
                stars[1].SetActive(false);
                stars[2].SetActive(false);
                // TO DO : 별 저장
                JsonSave.Instance.gameData.StageSetValueSave(GameData.StageValueType.STAR, GameManager.Instance.GetCurrentStage(), 1);
            }
            else if (scoreManager.ScoreValueGet(ScoreManager.ScoreType.FEED) == 8 && scoreManager.ScoreValueGet(ScoreManager.ScoreType.LIFE) >= 4)
            {
                JsonSave.Instance.gameData.StageSetValueSave(GameData.StageValueType.STAR, GameManager.Instance.GetCurrentStage(), 3);
            }
            else
            {
                stars[2].SetActive(false);
                JsonSave.Instance.gameData.StageSetValueSave(GameData.StageValueType.STAR, GameManager.Instance.GetCurrentStage(), 2);
            }
        }


        lifeTxt.text ="x " + scoreManager.ScoreValueGet(ScoreManager.ScoreType.LIFE);
        feedTxt.text = "x " + scoreManager.ScoreValueGet(ScoreManager.ScoreType.FEED);
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
        Time.timeScale = 1;
        scoreManager.ScoreValueSet(ScoreManager.ScoreType.FEED, ScoreManager.SetType.SET, 0);
        scoreManager.ScoreValueSet(ScoreManager.ScoreType.LIFE, ScoreManager.SetType.SET, 5);
        clear.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnClickExitBtn() 
    {
        Application.Quit();
    }
    public void StageFail() 
    {
        FailfeedTxt.text = "x " + scoreManager.ScoreValueGet(ScoreManager.ScoreType.FEED);
        Time.timeScale = 0;
        fail.SetActive(true);
    }

    private void StartScreen_WorldTxtPrint()
    {
        int currentStage = GameManager.Instance.CurrentStage;

        startScreenWorldTxt.text = "WORLD ";
        if (currentStage == 0)
        {
            startScreenWorldTxt.text += "PROTOTYPE";
            return;
        }
        else if(currentStage <= 5)
            startScreenWorldTxt.text += "하늘";
        else if (currentStage <= 10)
            startScreenWorldTxt.text += "숲";
        else if (currentStage <= 15)
            startScreenWorldTxt.text += "사원";
        else if (currentStage <= 20)
            startScreenWorldTxt.text += "기계 사원";
        else if (currentStage <= 25)
            startScreenWorldTxt.text += "지하감옥";

        startScreenWorldTxt.text += "-";

        if (currentStage % 5 == 1)
            startScreenWorldTxt.text += "1";
        else if (currentStage % 5 == 2)
            startScreenWorldTxt.text += "2";
        else if (currentStage % 5 == 3)
            startScreenWorldTxt.text += "3";
        else if (currentStage % 5 == 4)
            startScreenWorldTxt.text += "4";
        else if (currentStage % 5 == 0)
            startScreenWorldTxt.text += "5";
    }
}
