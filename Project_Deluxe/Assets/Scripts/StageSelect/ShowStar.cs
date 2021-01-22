using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowStar : MonoBehaviour
{
    [SerializeField]
    private int stageNumber = 0;
    private SceneMoveManager sceneMoveManager = null;
    private void Start()
    {
        sceneMoveManager = FindObjectOfType<SceneMoveManager>();
        ShowStars();
    }

    public void ShowStars()
    {
        if (JsonSave.Instance.gameData.StageGetValueSave(GameData.StageValueType.STAR, stageNumber) == 3)
        {
            transform.GetChild(0).gameObject.GetComponent<Image>().sprite = sceneMoveManager.GetStarSprites(false);
            transform.GetChild(1).gameObject.GetComponent<Image>().sprite = sceneMoveManager.GetStarSprites(false);
            transform.GetChild(2).gameObject.GetComponent<Image>().sprite = sceneMoveManager.GetStarSprites(false);
        }
        else if (JsonSave.Instance.gameData.StageGetValueSave(GameData.StageValueType.STAR, stageNumber) == 2)
        {
            transform.GetChild(0).gameObject.GetComponent<Image>().sprite = sceneMoveManager.GetStarSprites(false);
            transform.GetChild(1).gameObject.GetComponent<Image>().sprite = sceneMoveManager.GetStarSprites(false);
            transform.GetChild(2).gameObject.GetComponent<Image>().sprite = sceneMoveManager.GetStarSprites(true);
        }
        else if (JsonSave.Instance.gameData.StageGetValueSave(GameData.StageValueType.STAR, stageNumber) == 1)
        {
            transform.GetChild(0).gameObject.GetComponent<Image>().sprite = sceneMoveManager.GetStarSprites(false);
            transform.GetChild(1).gameObject.GetComponent<Image>().sprite = sceneMoveManager.GetStarSprites(true);
            transform.GetChild(2).gameObject.GetComponent<Image>().sprite = sceneMoveManager.GetStarSprites(true);
        }
        else
        {
            transform.GetChild(0).gameObject.GetComponent<Image>().sprite = sceneMoveManager.GetStarSprites(true);
            transform.GetChild(1).gameObject.GetComponent<Image>().sprite = sceneMoveManager.GetStarSprites(true);
            transform.GetChild(2).gameObject.GetComponent<Image>().sprite = sceneMoveManager.GetStarSprites(true);
        }
    }

    public void StageNumberSet(int value) => stageNumber = value;
}
