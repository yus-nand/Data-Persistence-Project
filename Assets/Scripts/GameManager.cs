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
public int highScore;    // PLAYER PREFS SETTINGS
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
        // #if UNITY_WEBGL
        //     LoadHighScore();
        // #endif
    }
    // private void Start()
    // {
    //     // Load the saved high score
    //     int highScore = PlayerPrefs.GetInt("HighScore", 0);
    //     Debug.Log("High Score Loaded: " + highScore);
    // }
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
        // #if UNITY_WEBGL
        //     LoadHighScore();
        // #endif
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
    // PLAYER PREFS SETTINGS
    public void SaveHighScore()
    {
        // Check if the current score is greater than the saved high score
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > highScore)
        {
            Debug.Log("New High Score: " + score);
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
        }
    }
    public void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText = GameObject.Find("Canvas/HighScoreText")?.GetComponent<TextMeshProUGUI>();
        highScoreText.text = "High Score: " + highScore;
    }
}

