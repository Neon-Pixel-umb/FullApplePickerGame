using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Import SceneManager to handle scene loading

public class StartScreenManager : MonoBehaviour
{
    public GameObject catcherSelected;
    public GameObject catcherNotSelected;
    public GameObject dropperSelected;
    public GameObject dropperNotSelected;
    public GameObject ArrowCatcher;
    public GameObject ArrowDropper;
    public TextMeshProUGUI rulesText;

    private int selectedModeIndex = 0; // 0 for Catcher, 1 for Dropper
    private string[] modeRules = new string[]
    {
        "Catcher Mode:\nCatch apples in a basket. Move with Left/Right or Joystick. Avoid sticks. Gain extra life every 200 points.",
        "Dropper Mode:\nDrop objects into the Basket. Watch for the red arrow to avoid dropping sticks in the basket. One Stick and the Game is Over"
    };

    private float inputCooldown = 0.2f; // Cooldown time between input reads
    private float nextInputTime = 0f;   // Time when the next input is allowed

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        // Check if enough time has passed since the last input
        if (Time.time >= nextInputTime)
        {
            // Handle input for navigating between modes
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxisRaw("Vertical") < 0)
            {
                selectedModeIndex = (selectedModeIndex + 1) % 2;
                UpdateUI();
                nextInputTime = Time.time + inputCooldown; // Set the next allowed input time
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxisRaw("Vertical") > 0)
            {
                selectedModeIndex = (selectedModeIndex == 0) ? 1 : 0;
                UpdateUI();
                nextInputTime = Time.time + inputCooldown; // Set the next allowed input time
            }
        }

        // Handle input for selecting a mode
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Submit"))
        {
            StartSelectedMode();
        }

        // Handle input for exiting the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    private void UpdateUI()
    {
        // Toggle the visibility of sprites based on the selected mode
        if (selectedModeIndex == 0) // Catcher is selected
        {
            catcherSelected.SetActive(true);
            catcherNotSelected.SetActive(false);
            ArrowCatcher.SetActive(true);
            ArrowDropper.SetActive(false);
            dropperSelected.SetActive(false);
            dropperNotSelected.SetActive(true);
        }
        else // Dropper is selected
        {
            catcherSelected.SetActive(false);
            catcherNotSelected.SetActive(true);
            ArrowCatcher.SetActive(false);
            ArrowDropper.SetActive(true);
            dropperSelected.SetActive(true);
            dropperNotSelected.SetActive(false);
        }

        // Update rules text based on selected mode
        rulesText.text = modeRules[selectedModeIndex];
    }

    private void StartSelectedMode()
    {
        if (selectedModeIndex == 0)
        {
            // Load Catcher mode scene
            SceneManager.LoadScene("Scene 0");
        }
        else if (selectedModeIndex == 1)
        {
            // Load Dropper mode scene (to be implemented later)
            SceneManager.LoadScene("Dropper"); // Placeholder, replace with your Dropper scene name
        }
    }

    private void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit(); // This will quit the application
    }
}
