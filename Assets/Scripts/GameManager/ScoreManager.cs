using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public Text scoreText;
    public Text highScoreText;

    private int score = 0;
    private int highScore = 0;

    void Start()
    {
        scoreText.text = "Score: " + score;
        // Load the high score from player prefs
        highScore = PlayerPrefs.GetInt("HighScore");
        UpdateHighScoreText();
    }

    public void AddScore()
    {
        score++;
        UpdateScoreText();
    }

    public void OnPlayerDeath()
    {
        // Check if the current score is higher than the high score
        if (score > highScore)
        {
            highScore = score;
            UpdateHighScoreText();

            // Save the new high score to player prefs
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        // Reset the score for the next round
        score = 0;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = string.Format("Score:{0}", score);
    }

    void UpdateHighScoreText()
    {
        highScoreText.text = string.Format("High Score:{0}", highScore);
    }
}