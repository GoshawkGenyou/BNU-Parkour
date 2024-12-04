using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    public float gameTime = 10f;
    public float score = 0f;
    public bool isGameOver = false;
    public int difficulty = 0;

    public float speedMultiplier = 1.0f; // Current speed multiplier
    private float minSpeedMultiplier = 1f;
    private float maxSpeedMultiplier = 12.0f; // Maximum speed multiplier
    public float multiplierIncrement = 0.1f; // Increment value for the multiplier

    public float timeElapsed = 0f; // Tracks the total time passed in the game
    private int ticksSinceLastMultiplierIncrease = 0; // Track how many frames have passed for multiplier logic

    public float multiplierIncreaseTime = 1f; // Time intervals after which the speed multiplier should increase (e.g., 60s)

    public int ticksToMultiplierIncrease = 600; // Initial ticks for multiplier increase
    public int maxTicksToMultiplierIncrease = 2000; // Maximum ticks for multiplier increase

    public GroundSpawner gSpawner;

    void Update()
    {
        if (isGameOver)
        {
            return; // Don't update game logic if game is over
        }

        // Update the game time based on elapsed time
        timeElapsed += Time.deltaTime;
        gameTime -= Time.deltaTime;

        // Increase score based on speed multiplier and time passed
        score += Time.deltaTime * speedMultiplier;

        ticksSinceLastMultiplierIncrease++;

        if (speedMultiplier > 3 && difficulty < 1)
        {
            difficulty++;
            gSpawner.incDiffulculty();
        }

        if (speedMultiplier > 10 && difficulty < 2)
        {
            difficulty++;
            gSpawner.incDiffulculty();
        }


        // Update the speed multiplier based on the elapsed time
        UpdateSpeedMultiplier();


        // Check for game over condition
        if (gameTime <= 0)
        {
            EndGame();
        }
    }

    void UpdateSpeedMultiplier()
    {
        // Logic to increase the multiplier at regular intervals
        if (ticksSinceLastMultiplierIncrease >= ticksToMultiplierIncrease)
        {
            ticksSinceLastMultiplierIncrease = 0; // Reset the tick counter

            // Increment the speed multiplier
            speedMultiplier = Mathf.Min(speedMultiplier + multiplierIncrement, maxSpeedMultiplier);

            // Gradually increase the ticks required for the next multiplier increase
            ticksToMultiplierIncrease = Mathf.Min(ticksToMultiplierIncrease + 100, maxTicksToMultiplierIncrease);
        }

        if (speedMultiplier > 3.0f)
        {
            minSpeedMultiplier = 3.0f;
        }
    }

    public void AddTimeOnReward()
    {
        if (speedMultiplier < 3f)
        {
            gameTime += 10f;
        }
        else if (speedMultiplier < 6f)
        {
            gameTime += 5f;
        }
        else
        {
            gameTime += 2f;
        }
    }

    public void SlowDownOnCollision()
    {
        StartCoroutine(SlowDownRoutine());
    }

    private IEnumerator SlowDownRoutine()
    {
        // Temporarily slow down game time and reduce speed multiplier
        float originalTimeScale = Time.timeScale;
        float originalMultiplier = speedMultiplier;

        Time.timeScale = 0.5f; // Slow down time
        speedMultiplier *= 0.8f;
        speedMultiplier = Mathf.Round(speedMultiplier * 10f) / 10f;
        speedMultiplier = Mathf.Max(speedMultiplier, minSpeedMultiplier); // Reduce multiplier but keep it >= minSpeed

        yield return new WaitForSecondsRealtime(1f); // Wait for 1 second in real time

        // Restore original time scale and multiplier
        Time.timeScale = originalTimeScale;
    }

    void EndGame()
    {
        isGameOver = true;

        Debug.Log("Game Over");
    }
}
