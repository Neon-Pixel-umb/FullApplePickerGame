using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject applePrefab;
    public GameObject stickPrefab;
    public GameObject specialObjectPrefab; // Special object prefab (e.g., Golden Apple)
    public ArrowController arrow;

    public float initialSpawnInterval = 2f; // Initial time between spawns
    public float minSpawnInterval = 0.5f;   // Minimum spawn interval
    public float difficultyIncreaseRate = 0.1f; // Rate at which the spawn interval decreases
    public float multipleAppleProbability = 0.1f; // Initial probability for multiple apples to drop
    public float probabilityIncreaseRate = 0.01f; // Rate at which the probability of multiple apples drops increases
    public float specialDropProbability = 0.05f; // Probability for special object drop
    public float stickDropProbability = 0.4f; // Default probability for stick drop

    private float currentSpawnInterval;
    private float timeSinceLastSpawn = 0f;
    private int maxObjectsToSpawn = 1; // Max number of objects to spawn simultaneously
    private bool isGoldenAppleEffectActive = false; // Track if the Golden Apple effect is active
    private float originalStickDropProbability; // Store the original stick drop probability

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        originalStickDropProbability = stickDropProbability; // Save the original stick drop probability
    }

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (!isGoldenAppleEffectActive && timeSinceLastSpawn >= currentSpawnInterval)
        {
            SpawnObjects();
            timeSinceLastSpawn = 0f;

            // Gradually increase difficulty by decreasing the spawn interval
            currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval - difficultyIncreaseRate);

            // Gradually increase the probability of multiple apples dropping
            multipleAppleProbability = Mathf.Min(1f, multipleAppleProbability + probabilityIncreaseRate);

            // Increase the maximum number of objects to spawn
            maxObjectsToSpawn = Mathf.Min(5, maxObjectsToSpawn + 1); // Cap at 5 objects to prevent excessive spawns
        }
    }

    public void StartGoldenAppleEffect()
    {
        if (!isGoldenAppleEffectActive)
        {
            StartCoroutine(GoldenAppleEffectCoroutine());
        }
    }

    private IEnumerator GoldenAppleEffectCoroutine()
    {
        isGoldenAppleEffectActive = true;

        // Temporarily set special drop probability to 0
        float originalSpecialDropProbability = specialDropProbability;
        specialDropProbability = 0f;

        // Reduce the stick drop probability to 10% during the golden apple effect
        stickDropProbability = 0.1f; 

        float effectDuration = 5f; // Duration of the effect
        float spawnIntervalDuringEffect = 0.2f; // Spawn interval during the effect to create the "rain" effect

        // Continuously spawn objects for 5 seconds without Golden Apples
        while (effectDuration > 0f)
        {
            SpawnObjects(); // Spawn objects at a faster rate
            yield return new WaitForSeconds(spawnIntervalDuringEffect); // Wait before spawning the next set of objects
            effectDuration -= spawnIntervalDuringEffect;
        }

        // Restore the original special drop probability
        specialDropProbability = originalSpecialDropProbability;

        // Restore the original stick drop probability
        stickDropProbability = originalStickDropProbability;

        isGoldenAppleEffectActive = false;
    }

    void SpawnObjects()
    {
        int numberOfObjectsToSpawn = Random.Range(1, maxObjectsToSpawn + 1); // Dynamic number of objects to spawn

        for (int i = 0; i < numberOfObjectsToSpawn; i++)
        {
            GameObject objectToSpawn;

            float randomValue = Random.value;
            if (!isGoldenAppleEffectActive && randomValue < specialDropProbability)
            {
                objectToSpawn = specialObjectPrefab; // Spawn a special object (Golden Apple)
            }
            else if (randomValue > stickDropProbability)
            {
                objectToSpawn = applePrefab;
            }
            else
            {
                objectToSpawn = stickPrefab;
            }

            // Set the spawn position to the arrow's position
            Vector2 spawnPosition = new Vector2(arrow.transform.position.x, transform.position.y);
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

            // Set the arrow to a new random position
            arrow.SetRandomPosition();
        }
    }
}
