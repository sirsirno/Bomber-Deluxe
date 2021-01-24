using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("UI")]
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject clear;
    [SerializeField]
    private GameObject clockNeedle;
    [SerializeField]
    private Text futureCountText;
    [SerializeField]
    private Image[] stampUI;
    [SerializeField]
    private Text result_clearTxt;
    [SerializeField]
    private Text result_lifeTxt;
    [SerializeField]
    private Text result_feedTxt;
    [SerializeField]
    private Text result_futureCountTxt;
    [SerializeField]
    private Image[] resultStampUI;
    [SerializeField]
    private Sprite[] resultStampSprite;
    [SerializeField]
    private GameObject[] stars = null;
    [SerializeField]
    private GameObject goHomeScreen = null;

    [Header("이거 시계 빨간 면적임.")]
    [SerializeField]
    private Image img;

    private float fillAmount = 0f;
    [Header("스타트 스크린 아웃페이드 관련")]
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
    [Header("타이머 텍스트인데 UI라 여기다 놈")]
    [SerializeField]
    private Text timerText = null;
    [Header("파티클 이펙트")]
    public ParticleSystem parEatEffect;
    public ParticleSystem badEatEffect;
    [Header("소리 관련")]
    [SerializeField]
    private GameObject mainCamera = null;
    [SerializeField]
    private GameObject soundBtn = null;
    private bool isMute = false;

    private Animator animator;
    private GameObject realPlayer = null;
    private ScoreManager scoreManager;

    private float clockDefaultVolume;

    void Awake()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        startScreenPanel.SetActive(true);
        isPlayingStartScreen = true;

        realPlayer = GameObject.FindGameObjectWithTag("Player");
        animator = realPlayer.GetComponent<Animator>();
        startScreenLifeTxt.text = string.Format("x   {0}", scoreManager.ScoreValueGet(ScoreManager.ScoreType.LIFE));
        StartScreen_WorldTxtPrint();
        clockDefaultVolume = AudioManager.Instance.SFX_ClockTic.volume;

        for (int i = 0; i < stampUI.Length; i++)
        {
            stampUI[i].sprite = GameManager.Instance.GetStampSprite(true, i);
        }

        if (PlayerPrefs.GetInt("Mute") == 0)
        {
            isMute = false;
            soundBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            mainCamera.GetComponent<AudioListener>().enabled = true;
        }
        else if (PlayerPrefs.GetInt("Mute") == 1)
        {
            isMute = true;
            soundBtn.GetComponent<Image>().color = new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.5019608f);
            mainCamera.GetComponent<AudioListener>().enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !menu.activeSelf && !isPlayingStartScreen)
        {
            menu.SetActive(true);
            Time.timeScale = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && menu.activeSelf && !isPlayingStartScreen)
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

    public void OnClickMenuBtn()
    {
        AllAudioManager.Instance.uiClick.Play();
        if (!menu.activeSelf && !isPlayingStartScreen)
        {
            menu.SetActive(true);
            Time.timeScale = 0;
        }
        else if (menu.activeSelf && !isPlayingStartScreen)
        {
            menu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void OnClickContinueBtn() 
    {
        AllAudioManager.Instance.uiClick.Play();
        menu.SetActive(false);
        Time.timeScale = 1;
    }

    public void OnClickHomeBtn() 
    {
        AllAudioManager.Instance.uiClick.Play();
        menu.SetActive(false);
        clear.SetActive(false);
        AudioManager.Instance.BGM_World.Stop();
        AudioManager.Instance.BGM_Future.Stop();
        AudioManager.Instance.BGM_Future2.Stop();
        goHomeScreen.SetActive(true);
        goHomeScreen.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        goHomeScreen.GetComponent<Image>().DOFade(1, 1).OnComplete(LoadMenu).SetUpdate(true).timeScale = 1;
    }

    private void LoadMenu()
    {
        if (GameManager.Instance.GetCurrentStage() <= 5)
            scoreManager.WorldTypeSet(0);
        else if (GameManager.Instance.GetCurrentStage() <= 10)
            scoreManager.WorldTypeSet(1);
        else if (GameManager.Instance.GetCurrentStage() <= 15)
            scoreManager.WorldTypeSet(2);
        else if (GameManager.Instance.GetCurrentStage() <= 20)
            scoreManager.WorldTypeSet(3);
        else
            scoreManager.WorldTypeSet(4);
        SceneManager.LoadScene(0);
    }

    public void StageClear() 
    {
        Time.timeScale = 0;
        result_lifeTxt.text = "x " + scoreManager.ScoreValueGet(ScoreManager.ScoreType.LIFE);
        result_feedTxt.text = "x " + scoreManager.ScoreValueGet(ScoreManager.ScoreType.FEED);
        result_futureCountTxt.text = "x " + scoreManager.ScoreValueGet(ScoreManager.ScoreType.ABILITYUSECOUNT);

        // 별 계산
        {
            stars[0].SetActive(true);
            stars[1].SetActive(true);
            stars[2].SetActive(true);
            if (scoreManager.ScoreValueGet(ScoreManager.ScoreType.FEED) < 4 || scoreManager.ScoreValueGet(ScoreManager.ScoreType.ABILITYUSECOUNT) >= 10)
            {
                stars[1].SetActive(false);
                stars[2].SetActive(false);
                result_clearTxt.text = "도착!";
                JsonSave.Instance.gameData.StageSetValueSave(GameData.StageValueType.STAR, GameManager.Instance.GetCurrentStage(), 1);
            }
            else if (scoreManager.ScoreValueGet(ScoreManager.ScoreType.FEED) == 8 && scoreManager.ScoreValueGet(ScoreManager.ScoreType.ABILITYUSECOUNT) <= 5)
            {
                result_clearTxt.text = "대단해요!";
                JsonSave.Instance.gameData.StageSetValueSave(GameData.StageValueType.STAR, GameManager.Instance.GetCurrentStage(), 3);
            }
            else
            {
                stars[2].SetActive(false);
                result_clearTxt.text = "잘했어요!";
                JsonSave.Instance.gameData.StageSetValueSave(GameData.StageValueType.STAR, GameManager.Instance.GetCurrentStage(), 2);
            }
        }

        // 스탬프 계산
        {
            int stampByte = scoreManager.ScoreValueGet(ScoreManager.ScoreType.STAMPTEMP);

            if (stampByte == 1 || stampByte == 3 ||stampByte == 5 || stampByte == 7)
                resultStampUI[0].sprite = resultStampSprite[1];
            else
                resultStampUI[0].sprite = resultStampSprite[0];

            if (stampByte == 2 || stampByte == 3 || stampByte == 6 || stampByte == 7)
                resultStampUI[1].sprite = resultStampSprite[1];
            else
                resultStampUI[1].sprite = resultStampSprite[0];

            if (stampByte == 4 || stampByte == 5 || stampByte == 6 || stampByte == 7)
                resultStampUI[2].sprite = resultStampSprite[1];
            else
                resultStampUI[2].sprite = resultStampSprite[0];

            JsonSave.Instance.gameData.StageSetValueSave(GameData.StageValueType.STAMP, GameManager.Instance.GetCurrentStage(), scoreManager.ScoreValueGet(ScoreManager.ScoreType.STAMPTEMP));
        }

        JsonSave.Instance.gameData.BestStageSet(GameManager.Instance.GetCurrentStage());

        clear.SetActive(true);
    }

    public void OnClickNextBtn() 
    {
        AllAudioManager.Instance.uiClick.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        clear.SetActive(false);
        
    }

    public void SoundOnOffBtn()
    {
        if (isMute)
        {
            isMute = false;
            PlayerPrefs.SetInt("Mute", 0);
            soundBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            mainCamera.GetComponent<AudioListener>().enabled = true;
            AllAudioManager.Instance.uiClick.Play();
        }
        else
        {
            isMute = true;
            PlayerPrefs.SetInt("Mute", 1);
            soundBtn.GetComponent<Image>().color = new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.5019608f);
            mainCamera.GetComponent<AudioListener>().enabled = false;
        }
    }

    public void OncClickRetryBtn() 
    {
        AllAudioManager.Instance.uiClick.Play();
        Time.timeScale = 1;
        clear.SetActive(false);
        scoreManager.ScoreValueSet(ScoreManager.ScoreType.LIFE, ScoreManager.SetType.SET, 5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OncClickStageRetryBtn()
    {
        AllAudioManager.Instance.uiClick.Play();
        Time.timeScale = 1;
        clear.SetActive(false);
        scoreManager.ScoreValueSet(ScoreManager.ScoreType.LIFE, ScoreManager.SetType.REMOVE, 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnClickExitBtn() 
    {
        AllAudioManager.Instance.uiClick.Play();
        Application.Quit();
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

    public void TimerTimeOutput()
    {
        if (PlayerController.Instance.sleeping || GameManager.Instance.GetRealTimer() <= 60)
            timerText.color = Color.red;
        else
            timerText.color = Color.black;
        timerText.text = string.Format("TIME\n{0}", GameManager.Instance.GetRealTimer());
    }

    public void FutureCountOutput()
    {
        futureCountText.text = string.Format("x {0}", scoreManager.ScoreValueGet(ScoreManager.ScoreType.ABILITYUSECOUNT));
    }

    public void StampSpriteSet(bool isEmpty, int number)
    {
        stampUI[number].sprite = GameManager.Instance.GetStampSprite(isEmpty, number);
    }
}
