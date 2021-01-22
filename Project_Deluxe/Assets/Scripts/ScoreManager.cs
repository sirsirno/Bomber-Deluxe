using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int feed = 0;
    [SerializeField]
    private int life = 5;
    private int abilityUseCount = 0;
    private int scoreTemp = 15000;
    private ScoreManager[] scoreManager;

    private void Awake()
    {
        scoreManager = FindObjectsOfType<ScoreManager>();

        if(scoreManager.Length >=2)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public enum ScoreType
    {
        FEED,
        LIFE,
        ABILITYUSECOUNT,
        SCORETEMP
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
        else if (type == ScoreType.ABILITYUSECOUNT)
        {
            if (setType == SetType.SET)
                abilityUseCount = value;
            else if (setType == SetType.ADD)
                abilityUseCount += value;
            else if (setType == SetType.REMOVE)
                abilityUseCount -= value;
        }
        else if (type == ScoreType.SCORETEMP)
        {
            if (setType == SetType.SET)
                scoreTemp = value;
            else if (setType == SetType.ADD)
                scoreTemp += value;
            else if (setType == SetType.REMOVE)
            {
                if (scoreTemp - value < 0)
                    scoreTemp = 0;
                else
                    scoreTemp -= value;
            }
        }
    }

    public int ScoreValueGet(ScoreType type)
    {
        if (type == ScoreType.FEED)
        {
            return feed;
        }
        else if (type == ScoreType.LIFE)
        {
            return life;
        }
        else if (type == ScoreType.ABILITYUSECOUNT)
        {
            return abilityUseCount;
        }
        else //if (type == ScoreType.SCORETEMP)
        {
            return scoreTemp;
        }
    }
}
