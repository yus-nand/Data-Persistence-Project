using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public GameObject pauseScreen;
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    
    [SerializeField] private TextMeshProUGUI HighScoreText;
    [SerializeField] private TextMeshProUGUI NewHighScoreText;
    [SerializeField] private TextMeshProUGUI PressQ_text;
    [SerializeField] private ParticleSystem NewHighScoreParticles;
    

    private bool m_Started = false;
    private int m_Points;    
    private bool m_GameOver = false;
    public bool isGamePaused = false;

    
    // Start is called before the first frame update
    void Start()
    {
        NewHighScoreText = GameObject.Find("Canvas/NewHighScoreText")?.GetComponent<TextMeshProUGUI>();          // what a headache
        HighScoreText = GameObject.Find("Canvas/HighScoreText")?.GetComponent<TextMeshProUGUI>();
        
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started && !isGamePaused)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PressQ_text.enabled = false;
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);   
                // #if UNITY_WEBGL
                //     HighScoreText.text = "High Score: " + GameManager.Instance.highScore;
                // #else
                    HighScoreText.text = "High Score: " + GameManager.Instance.score;   
                // #endif                    
            }                                                                       
            
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            TogglePause();
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        
        // #if UNITY_WEBGL
        //     if(m_Points > GameManager.Instance.highScore)
        //     {
        //         GameManager.Instance.highScore = m_Points;
        //         GameManager.Instance.SaveHighScore();
        //         Instantiate(NewHighScoreParticles, NewHighScoreParticles.transform.position, Quaternion.identity);
        //         NewHighScoreParticles.Play();
        //         NewHighScoreText.enabled = true;  
        //     }
        // #else
            if(m_Points > GameManager.Instance.score)
            {
                GameManager.Instance.score = m_Points;
                GameManager.Instance.SaveScore();
                Instantiate(NewHighScoreParticles, NewHighScoreParticles.transform.position, Quaternion.identity);
                NewHighScoreParticles.Play();
                NewHighScoreText.enabled = true;            // GameObject.Find().GetComponent<>() does not work on disabled objects
            }
        // #endif
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
    private void TogglePause()
    {
        if(isGamePaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    private void ResumeGame()
    {
        isGamePaused = false;
        pauseScreen.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
    private void PauseGame()
    {
        isGamePaused = true;
        pauseScreen.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
    public void QuitGame()
    {
        isGamePaused = false;
        pauseScreen.gameObject.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}