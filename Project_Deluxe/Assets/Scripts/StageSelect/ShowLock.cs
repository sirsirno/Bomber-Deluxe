using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowLock : MonoBehaviour
{
    [SerializeField]
    private int stageNumber = 0;

    private SceneMoveManager sceneMoveManager = null;
    private void Start()
    {
        sceneMoveManager = FindObjectOfType<SceneMoveManager>();

        if(stageNumber > JsonSave.Instance.gameData.BestStageGet() + 1)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            GetComponent<Button>().interactable = false;
            GetComponent<Image>().sprite = sceneMoveManager.GetStageButtonSprites(0);
        }
    }
}
