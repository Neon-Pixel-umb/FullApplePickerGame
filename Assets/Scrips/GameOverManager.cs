using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;

    void Start()
    {
        // Retrieve and display the final score
        AudioManager.instance.PlayGameOverMusic();
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0); // Retrieve the score from PlayerPrefs
        finalScoreText.text = "Your Score: " + finalScore.ToString();
    }

    void Update()
    {
        // Restart game input
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Submit"))
        {
            RestartToStartScreen();
        }
    }

    private void RestartToStartScreen()
    {
        // Stop all music and load the Start Screen scene
        AudioManager.instance.StopAllMusic();
        SceneManager.LoadScene("Start Screen");
    }
}
