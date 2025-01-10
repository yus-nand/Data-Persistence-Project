using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;
using System.IO;
using TMPro;





#if UNITY_EDITOR
using UnityEditor;
#endif    

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int score;
    [SerializeField] private TextMeshProUGUI highScoreText;
    private Button playButton;
    private Button exitButton;
    public void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        if(score >= 96)
        {
            score = 0;
        }
        LoadScore();
    }
    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }
    public void ExitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif            
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupDependencies();
        LoadScore();
    }
    private void SetupDependencies()
    {
        playButton = GameObject.Find("PlayButton")?.GetComponent<Button>();         //used NULL CONDITIONAL OPERATOR ( ? )
        exitButton = GameObject.Find("Exit")?.GetComponent<Button>();               //used NULL CONDITIONAL OPERATOR ( ? )

        if(playButton != null)
        {
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(StartNew);
        }

        if(exitButton != null)  
        {
            exitButton.onClick.RemoveAllListeners();    
            exitButton.onClick.AddListener(ExitGame);
        }
    }
    [System.Serializable]
    public class SaveData
    {
        public int score;
    }
    public void SaveScore()
    {
        SaveData data = new SaveData();
        data.score = score;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/score.json", json);
        Debug.Log("Application.persistentDataPath: " + Application.persistentDataPath);
    }
    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/score.json";
        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            score = data.score;
            highScoreText = GameObject.Find("Canvas/HighScoreText")?.GetComponent<TextMeshProUGUI>(); 
            highScoreText.text = "High Score: " + score;
        }
    }
}

