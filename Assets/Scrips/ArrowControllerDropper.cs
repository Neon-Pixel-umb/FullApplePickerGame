using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowControllerDropper : MonoBehaviour
{
    public float speed = 3f; // Speed of the arrow movement
    public float boundary = 8f; // Define the boundary for arrow movement
    public GameObject greenArrow; // Reference to the green arrow GameObject (apple)
    public GameObject redArrow; // Reference to the red arrow GameObject (stick)
    public GameObject applePrefab; // Prefab for the apple
    public GameObject stickPrefab; // Prefab for the stick

    private bool movingRight = true; // Determines the direction of movement
    private bool isAppleNext = true; // Determines if the next drop will be an apple

    void Start()
    {
        InitializeArrowState(); // Initialize the arrow state at the start
    }

    void Update()
    {
        MoveArrows();
        CheckInput();
    }

    private void InitializeArrowState()
    {
        // Set initial state to green (apple) by default
        isAppleNext = Random.value > 0.3f; // 70% chance to start with apple, 30% for stick
        UpdateArrowVisibility();
    }

    private void MoveArrows()
    {
        // Move both arrows back and forth
        float moveStep = speed * Time.deltaTime;
        if (movingRight)
        {
            greenArrow.transform.Translate(Vector2.right * moveStep);
            redArrow.transform.Translate(Vector2.right * moveStep);

            if (greenArrow.transform.position.x > boundary)
            {
                movingRight = false; // Change direction
            }
        }
        else
        {
            greenArrow.transform.Translate(Vector2.left * moveStep);
            redArrow.transform.Translate(Vector2.left * moveStep);

            if (greenArrow.transform.position.x < -boundary)
            {
                movingRight = true; // Change direction
            }
        }
    }

    private void CheckInput()
    {
        // Check if the player presses the drop button (space, enter, or A on the controller)
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Submit"))
        {
            DropObject();
        }
    }

    private void DropObject()
    {
        // Instantiate the apple or stick prefab based on the next drop state
        GameObject objToDrop = Instantiate(isAppleNext ? applePrefab : stickPrefab, greenArrow.transform.position, Quaternion.identity);
        
        // Determine the next drop: 70% chance for apple, 30% chance for stick
        isAppleNext = Random.value > 0.3f; // 0.3 represents 30% chance for stick

        // Update the arrow visibility to reflect the next drop
        UpdateArrowVisibility();
    }

    private void UpdateArrowVisibility()
    {
        // Show the green arrow if the next drop is an apple, otherwise show the red arrow
        greenArrow.SetActive(isAppleNext);
        redArrow.SetActive(!isAppleNext);
    }
}
