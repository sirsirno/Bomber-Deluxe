using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI : MonoBehaviour
{
    [SerializeField]
    private Text startTxt = null;
    [SerializeField]
    private GameObject startPanel = null;
    [SerializeField]
    private GameObject mainCamera = null;
    [SerializeField]
    private GameObject backgroundCamera = null;
    [SerializeField]
    private GameObject TitleObjects = null;

    private ScoreManager scoreManager = null;

    // 함수용 변수

    [SerializeField]
    private float cameraMoveSpeed=1f;
    [SerializeField]
    private float cameraMoveSlowSpeed = 1f;
    [SerializeField]
    private float cameraMoveSlowDistance = 50f;
    [SerializeField]
    private float cameraMoveDistance = 800f;

    private void Awake()
    {
        JsonSave.Instance.LoadGameData();
        Time.timeScale = 1;
    }

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        scoreManager.ScoreValueSet(ScoreManager.ScoreType.LIFE, ScoreManager.SetType.SET, 5);
        startTxt.DOColor(new Color(1f, 1f, 1f, 10f), 0.8f).SetLoops(-1, LoopType.Yoyo);

    }
    private void Update()
    {
        if (Input.anyKeyDown && scoreManager.isTitleBegin) 
        {
            startPanel.GetComponent<RectTransform>().DOLocalMoveY(1000, 2);
            scoreManager.isTitleBegin = false;
            backgroundCamera.transform.DOMoveY(-33.9f, 2f);
            TitleObjects.transform.DOLocalMoveY(30f, 2);
            TitleObjects.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 1);

            mainCamera.transform.DOMoveY(-cameraMoveSlowDistance, cameraMoveSlowSpeed);
        }

        if (mainCamera.transform.localPosition.y <= -cameraMoveSlowDistance)
        {
            mainCamera.transform.DOMoveY(-cameraMoveDistance, cameraMoveSpeed);
        }
    }
}
