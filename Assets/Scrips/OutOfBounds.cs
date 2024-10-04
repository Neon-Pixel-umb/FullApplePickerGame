using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Apple"))
        {
            Destroy(other.gameObject);
            // Increment score or other actions
            Debug.Log("Dropped an apple");
        }
        else if (other.CompareTag("Stick"))
        {
            Destroy(other.gameObject);
            // Decrement life or other actions
            Debug.Log("Aviod a Stick!");
        }
        else if (other.CompareTag("GoldApple"))
        {
            Destroy(other.gameObject);
            // Increment score or other actions
            Debug.Log("Dropped a Golden apple");
        }
    }
}
