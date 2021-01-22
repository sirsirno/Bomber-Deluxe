using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class JsonSave : MonoBehaviour
{
    public bool debugLogPrint = false;

    //싱글톤====================
    static GameObject _container;
    static GameObject Container
    {
        get
        {
            return _container;
        }
    }
    static JsonSave _instance;
    public static JsonSave Instance
    {
        get
        {
            if (!_instance)
            {
                _container = new GameObject();
                _container.name = "JsonSave";
                _instance = _container.AddComponent(typeof(JsonSave)) as JsonSave;
                DontDestroyOnLoad(_container);
            }
            return _instance;
        }
    }
    // =================================================

    public string GameDataFileName = ".json";

    private string filePath = "";

    public GameData _gameData;
    public GameData gameData
    {
        get
        {
            if (_gameData == null)
            {
                LoadGameData();
                SaveGameData();
            }
            return _gameData;
        }
    }
    private void Awake()
    {
        filePath = string.Concat(Application.persistentDataPath, GameDataFileName);
    }

    public void LoadGameData()
    {
        if (File.Exists(filePath))
        {
            Debug.Log("불러오기");
            string code = File.ReadAllText(filePath);

            byte[] bytes = System.Convert.FromBase64String(code);
            string FromJsonData = System.Text.Encoding.UTF8.GetString(bytes);
            _gameData = JsonUtility.FromJson<GameData>(FromJsonData);
        }
        else
        {
            Debug.Log("새로운 파일 생성");
            _gameData = new GameData();
        }
    }

    public void SaveGameData()
    {
        Debug.Log("저장");
        string ToJsonData = JsonUtility.ToJson(gameData, true);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(ToJsonData);
        string code = System.Convert.ToBase64String(bytes);

        File.WriteAllText(filePath, code);
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }

    private void OnApplicationPause()
    {
        SaveGameData();
    }

    private void Update()
    {
        if (debugLogPrint)
        {
            debugLogPrint = false;
            for (int i = 0; i < 30; i++)
            {
                Debug.Log("스테이지 " + i + " : 별 " + gameData.StageGetValueSave(GameData.StageValueType.STAR, i) + "개, 점수 " + gameData.StageGetValueSave(GameData.StageValueType.STAMP, i) + "점\n");
            }
        }
    }
}
