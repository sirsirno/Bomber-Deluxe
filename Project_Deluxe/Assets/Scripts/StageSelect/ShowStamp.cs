using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowStamp : MonoBehaviour
{
    [SerializeField]
    private int stageNumber = 0;
    private SceneMoveManager sceneMoveManager = null;

    private void Start()
    {
        sceneMoveManager = FindObjectOfType<SceneMoveManager>();
        ShowStamps();
    }

    public void ShowStamps()
    {
        int stamp1 = 3 * (stageNumber - 1);
        int stamp2 = stamp1 + 1;
        int stamp3 = stamp1 + 2;
        transform.GetChild(0).gameObject.GetComponent<Image>().sprite = sceneMoveManager.GetStampSprites(true, stamp1);
        transform.GetChild(1).gameObject.GetComponent<Image>().sprite = sceneMoveManager.GetStampSprites(true, stamp2);
        transform.GetChild(2).gameObject.GetComponent<Image>().sprite = sceneMoveManager.GetStampSprites(true, stamp3);

        int stampByte = JsonSave.Instance.gameData.StageGetValueSave(GameData.StageValueType.STAMP, stageNumber);

        if (stampByte == 1 || stampByte == 3 || stampByte == 5 || stampByte == 7)
            transform.GetChild(0).gameObject.GetComponent<Image>().sprite = sceneMoveManager.GetStampSprites(false, stamp1);
        if (stampByte == 2 || stampByte == 3 || stampByte == 6 || stampByte == 7)
            transform.GetChild(1).gameObject.GetComponent<Image>().sprite = sceneMoveManager.GetStampSprites(false, stamp2);
        if (stampByte == 4 || stampByte == 5 || stampByte == 6 || stampByte == 7)
            transform.GetChild(2).gameObject.GetComponent<Image>().sprite = sceneMoveManager.GetStampSprites(false, stamp3);
    }

    public void StageNumberSet(int value) => stageNumber = value;
}
