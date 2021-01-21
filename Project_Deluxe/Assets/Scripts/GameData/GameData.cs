using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    [SerializeField]
    private int[] stageStar = new int[30];
    [SerializeField]
    private int[] stageScore = new int[30];
    [SerializeField]
    private int bestStage = 0;
    public enum StageValueType
    {
        STAR,
        SCORE
    }

    public void StageSetValueSave(StageValueType type, int stage, int value)
    {
        if (type == StageValueType.STAR)
        {
            if (stageStar[stage] < value)
                stageStar[stage] = value;
            else
                return;
        }
        else if (type == StageValueType.SCORE)
        {
            if (stageScore[stage] < value)
                stageScore[stage] = value;
            else
                return;
        }

        JsonSave.Instance.SaveGameData();
    }

    public int StageGetValueSave(StageValueType type, int stage)
    {
        if (type == StageValueType.STAR)
        {
            return stageStar[stage];
        }
        else //if (type == StageValueType.SCORE)
        {
            return stageScore[stage];
        }
    }
}