using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float minX = -8f; // Minimum X position
    public float maxX = 8f;  // Maximum X position
    public float flickerInterval = 0.5f; // Time interval between flickers

    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Start the flickering coroutine
        StartCoroutine(Flicker());

        // Randomize the position of the arrow within the range
        SetRandomPosition();
        SetRandomPosition();
    }

    public void SetRandomPosition()
    {
        float randomX = Random.Range(minX, maxX);
        transform.position = new Vector2(randomX, transform.position.y);
    }
    
    IEnumerator Flicker()
    {
        while (true)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; // Toggle visibility
            yield return new WaitForSeconds(flickerInterval); // Wait for the next flicker interval
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
