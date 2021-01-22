using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int feed = 0;
    [SerializeField]
    private int life = 5;
    private int abilityUseCount = 0;
    private int stampTemp = 0;

    [HideInInspector]
    public bool isTitleBegin = true;
    /// <summary>
    /// 타이틀로 돌아갈때, 스테이지로 다시 돌아가기 위해 있는 변수
    /// </summary>
    private int worldType = -1;
    private ScoreManager[] scoreManagers;


    private void Awake()
    {
        scoreManagers = FindObjectsOfType<ScoreManager>();

        if(scoreManagers.Length >=2)
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
        STAMPTEMP
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
        else if (type == ScoreType.STAMPTEMP)
        {
            if (setType == SetType.SET)
                stampTemp = value;
            else if (setType == SetType.ADD)
                stampTemp += value;
            else if (setType == SetType.REMOVE)
                stampTemp -= value;
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
            return stampTemp;
        }
    }

    public int WorldTypeGet => worldType;
    public void WorldTypeSet(int value) => worldType = value;
}
