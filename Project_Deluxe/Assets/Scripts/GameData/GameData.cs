using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    [SerializeField]
    private int[] stageStar = new int[30];
    [SerializeField]
    private int[] stageStamp = new int[30];
    [SerializeField]
    private int bestStage = 0;
    public enum StageValueType
    {
        STAR,
        STAMP
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
        else if (type == StageValueType.STAMP)
        {
            if (stageStamp[stage] < value)
                stageStamp[stage] = value;
            else
                return;
        }

        JsonSave.Instance.SaveGameData();
    }

    public int StageGetValueSave(StageValueType type, int stage, bool isRealCount = false)
    {
        if (type == StageValueType.STAR)
        {
            return stageStar[stage];
        }
        else //if (type == StageValueType.SCORE)
        {
            if(isRealCount)
            {
                if (stageStamp[stage] == 1 || stageStamp[stage] == 2 || stageStamp[stage] == 4)
                    return 1;
                else if (stageStamp[stage] == 3 || stageStamp[stage] == 5 || stageStamp[stage] == 6)
                    return 2;
                else if (stageStamp[stage] == 7)
                    return 3;
                else
                    return 0;
            }
            else
                return stageStamp[stage];
        }
    }

    public void BestStageSet(int value)
    {
        if (bestStage < value)
        {
            bestStage = value;
        }
    }

    public int BestStageGet() => bestStage;
}