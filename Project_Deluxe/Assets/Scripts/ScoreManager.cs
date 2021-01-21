using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int feed = 0;
    private int life = 5;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public enum ScoreType
    {
        FEED,
        LIFE
    }

    public enum SetType
    {
        SET,
        ADD,
        REMOVE
    }

    public void ScoreValueSet(ScoreType type, SetType setType, int value)
    {
        if (type == ScoreType.FEED)
        {
            if (setType == SetType.SET)
                feed = value;
            else if (setType == SetType.ADD)
                feed += value;
            else if (setType == SetType.REMOVE)
                feed -= value;
        }
        else if (type == ScoreType.LIFE)
        {
            if (setType == SetType.SET)
                life = value;
            else if (setType == SetType.ADD)
                life += value;
            else if (setType == SetType.REMOVE)
                life -= value;
        }
    }

    public int ScoreValueGet(ScoreType type)
    {
        if (type == ScoreType.FEED)
        {
            return feed;
        }
        else //if (type == StageValueType.SCORE)
        {
            return life;
        }
    }
}
