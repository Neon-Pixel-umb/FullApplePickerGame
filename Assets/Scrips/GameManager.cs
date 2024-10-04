using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    public enum GameMode { Catcher, Dropper } // Enum for game modes
    public GameMode currentGameMode; // Variable to store the current game mode

    public int score = 0;
    public int lives = 3;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;

    private int nextLifeScoreThreshold = 100; // The score needed to gain the next extra life

    private void Awake()
    {
        // Ensure there's only one instance of the GameManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the sceneLoaded event
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe from the sceneLoaded event to prevent memory leaks
    }

    private void Start()
    {
        // Initialize the UI text
        UpdateScoreText();
        UpdateLivesText();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    if (scene.name == "Scene 0") 
    {
        currentGameMode = GameMode.Catcher;
        ResetGameState(); 
    }
    else if (scene.name == "Dropper") 
    {
        currentGameMode = GameMode.Dropper;
        ResetGameState(); 
    }

    scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
    livesText = GameObject.Find("LivesText")?.GetComponent<TextMeshProUGUI>();

    // Check and initialize ScoreManager instance
    if (ScoreManager.instance != null)
    {
        ScoreManager.instance.SetScoreText(scoreText);
    }
    else
    {
        Debug.LogError("ScoreManager instance is null! Trying to find or create it.");
        ScoreManager.instance = FindObjectOfType<ScoreManager>();

        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.SetScoreText(scoreText);
        }
        else
        {
            Debug.LogError("ScoreManager could not be found or created.");
        }
    }

    UpdateScoreText();
    UpdateLivesText();
}


    public void AddScore(int amount)
    {
        score += amount;
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(amount); // Update the ScoreManager as well
        }
        else
        {
            Debug.LogError("ScoreManager instance is null!");
        }
        UpdateScoreText();

        if (currentGameMode == GameMode.Catcher && score >= nextLifeScoreThreshold)
        {
            GainExtraLife();
        }
    }

    public void LoseLife()
    {
        if (currentGameMode == GameMode.Catcher)
        {
            lives--;
            UpdateLivesText();

            if (lives <= 0)
            {
                GameOver();
            }
        }
        else if (currentGameMode == GameMode.Dropper)
        {
            GameOver();
        }
    }

    private void GainExtraLife()
    {
        lives++;
        UpdateLivesText();
        Debug.Log("Gained an extra life!");

        AudioManager.instance.PlayExtraLifeSound();
        nextLifeScoreThreshold += 200;
    }

    private void UpdateScoreText()
    {
        if (scoreText != null) // Add null check
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    private void UpdateLivesText()
    {
        if (livesText != null) // Add null check
        {
            if (currentGameMode == GameMode.Catcher)
            {
                livesText.text = "X: " + lives.ToString();
            }
            else if (currentGameMode == GameMode.Dropper)
            {
                livesText.text = ""; // No lives to display in Dropper mode
            }
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");

        // Check if ScoreManager instance is not null before calling SaveScore
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.SaveScore(); // Save the final score
        }
        else
        {
            Debug.LogError("ScoreManager instance is null! Cannot save score.");
        }

        SceneManager.LoadScene("GameOverScene");
    }

    private void ResetGameState()
    {
        // Reset the game state
        score = 0;
        
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.ResetScore(); // Ensure the score is reset for both modes
        }
        else
        {
            Debug.LogError("ScoreManager instance is null! Cannot reset score.");
        }

        lives = (currentGameMode == GameMode.Catcher) ? 3 : 0; // Lives are only relevant for Catcher mode
        nextLifeScoreThreshold = 200;
    }

    public void RestartGame()
    {
        ResetGameState();
        SceneManager.LoadScene("Start Screen"); // Return to the Start Screen
    }
}
