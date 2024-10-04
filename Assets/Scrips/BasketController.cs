using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketController : MonoBehaviour
{
    public float speed = 10f;
    private float screenHalfWidthInWorldUnits;

    void Start()
    {
        // Calculate the screen width in world units
        float halfBasketWidth = transform.localScale.x / 2f;
        screenHalfWidthInWorldUnits = Camera.main.aspect * Camera.main.orthographicSize + halfBasketWidth;
    }

    void Update()
    {
        float move = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.Translate(move, 0, 0);

        // Restrict the basket's movement within the screen bounds
        float newXPosition = Mathf.Clamp(transform.position.x, -screenHalfWidthInWorldUnits, screenHalfWidthInWorldUnits);
        transform.position = new Vector2(newXPosition, transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Apple"))
        {
            Destroy(other.gameObject);
            if (GameManager.instance != null)
            {
                GameManager.instance.AddScore(5); // Add score when catching an apple
                AudioManager.instance.PlayCatchAppleSound();
                Debug.Log("Caught an apple!");
            }
            else
            {
                Debug.LogError("GameManager instance is null!");
            }
        }
        else if (other.CompareTag("Stick"))
        {
            Destroy(other.gameObject);
            if (GameManager.instance != null)
            {
                GameManager.instance.LoseLife(); // Lose a life when hitting a stick
                AudioManager.instance.PlayHitStickSound();
                Debug.Log("Hit by a stick!");
            }
            else
            {
                Debug.LogError("GameManager instance is null!");
            }
        }
        else if (other.CompareTag("GoldApple"))
        {
            Destroy(other.gameObject);
            if (GameManager.instance != null)
            {
                GameManager.instance.AddScore(10); // Add more points for the Golden Apple
                AudioManager.instance.PlayExtraLifeSound();
                Debug.Log("Caught a Golden Apple!");

                // Trigger the special spawning behavior
                Spawner spawner = FindObjectOfType<Spawner>();
                if (spawner != null)
                {
                    spawner.StartGoldenAppleEffect(); // Trigger the effect on the spawner
                }
                else
                {
                    Debug.LogError("Spawner not found in the scene!");
                }
            }
            else
            {
                Debug.LogError("GameManager instance is null!");
            }
        }
    }
}
