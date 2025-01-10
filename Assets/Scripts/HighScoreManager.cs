using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    public int currentScore;

    void Start()
    {
        // Load the saved high score
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        Debug.Log("High Score Loaded: " + highScore);
    }

    public void SaveHighScore()
    {
        // Check if the current score is greater than the saved high score
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (currentScore > highScore)
        {
            Debug.Log("New High Score: " + currentScore);
            PlayerPrefs.SetInt("HighScore", currentScore);
            PlayerPrefs.Save();
        }
    }
}
