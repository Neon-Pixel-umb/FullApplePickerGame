using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketControllerDropper : MonoBehaviour
{
    public float initialSpeed = 0f; // Start with 0 speed, basket is stationary
    public float speedIncreaseFactor = 0.1f;
    public float maxSpeed = 4.7f; // Set maximum speed slightly less than arrow speed
    public float fluctuationRange = 0.3f; // Range to avoid
    public float leftBoundary = -10f; // Set left boundary for the basket
    public float rightBoundary = 10f; // Set right boundary for the basket
    public int scoreToStartMoving = 50; // Score at which the basket starts moving
    public int scoreToFluctuate = 100; // Score at which the basket speed starts fluctuating

    private float basketSpeed;
    private float fluctuationMin;
    private float fluctuationMax;
    private bool movingRight = true;
    private bool isMoving = false; // Indicates whether the basket should start moving

    private void Start()
    {
        basketSpeed = initialSpeed;
        CalculateFluctuationLimits();
    }

    private void Update()
    {
        if (ScoreManager.instance.GetCurrentScore() >= scoreToStartMoving)
        {
            isMoving = true; // Start moving once the score threshold is reached
        }

        if (isMoving)
        {
            MoveBasket();
        }

        if (ScoreManager.instance.GetCurrentScore() >= scoreToFluctuate)
        {
            AdjustBasketSpeed();
        }
    }

    private void MoveBasket()
    {
        // Move the basket back and forth
        float moveStep = basketSpeed * Time.deltaTime;
        if (movingRight)
        {
            transform.Translate(Vector2.right * moveStep);
            if (transform.position.x >= rightBoundary)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector2.left * moveStep);
            if (transform.position.x <= leftBoundary)
            {
                movingRight = true;
            }
        }
    }

    private void AdjustBasketSpeed()
    {
        // Increase the speed gradually until max speed is reached
        basketSpeed += speedIncreaseFactor * Time.deltaTime;

        // If the basket speed reaches max speed, fluctuate between random speeds
        if (basketSpeed > maxSpeed)
        {
            basketSpeed = Random.Range(fluctuationMin, fluctuationMax);
        }
    }

    private void CalculateFluctuationLimits()
    {
        // Ensure that the fluctuation speed is never equal to the arrow speed (5)
        fluctuationMin = maxSpeed - fluctuationRange / 2;
        fluctuationMax = maxSpeed + fluctuationRange / 2;
    }

    private void OnTriggerEnter2D(Collider2D other)
{
    // Check if the basket catches an apple or a stick
    if (other.CompareTag("Apple"))
    {
        // Add score for catching an apple
        ScoreManager.instance.AddScore(10);
        AudioManager.instance.PlayCatchAppleSound();
        other.gameObject.SetActive(false); // Deactivate apple
    }
    else if (other.CompareTag("Stick"))
    {
        // End the game if the basket catches a stick
        if (GameManager.instance != null)
        {
            AudioManager.instance.PlayHitStickSound();
            GameManager.instance.GameOver();
        }
        else
        {
            Debug.LogError("GameManager instance is null. Ensure it is initialized properly.");
        }
    }
}

}
