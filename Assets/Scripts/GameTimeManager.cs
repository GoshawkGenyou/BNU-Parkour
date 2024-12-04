using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameTimeManager : MonoBehaviour
{
    public float tickInterval = 0.05f; // Initial time interval for each tick (in seconds)
    public float gameTime = 10f;
    public float score = 0f;
    public bool isGameOver = false;

    private float timeSinceLastTick = 0f; // Tracks time since last tick
    private int ticksSinceLastMultiplierIncrease = 0; // Ticks since last multiplier increase
    private int currentTick = 0; // The current tick number

    public float speedMultiplier = 3.0f; // Current speed multiplier
    private float minSpeedMultiplier = 1f;
    private float maxSpeedMultiplier = 7.0f; // Maximum speed multiplier
    public float multiplierIncrement = 0.1f; // Increment value for the multiplier

    private float timeElapsed = 0f; // Tracks the total time passed in the game

    public float multiplierIncreaseTime = 1f; // Time intervals after which the speed multiplier should increase (e.g., 60s)

    public int ticksToMultiplierIncrease = 5; // Initial ticks for multiplier increase
    public int maxTicksToMultiplierIncrease = 200; // Maximum ticks for multiplier increase


    // UI Elements
    public Text speedText;
    public Text timeText;
    public Text scoreText;


    void Start()
    {
        StartCoroutine(TickUpdate());
    }

    // Coroutine to handle tick updates
    IEnumerator TickUpdate()
    {
        float fixedElapsedTime = 0f; // Tracks total fixed elapsed time

        while (!isGameOver)
        {
            yield return null; // Wait for the next frame

            // Increment fixedElapsedTime by real-world delta time
            fixedElapsedTime += Time.deltaTime;

            // If enough time has elapsed for a tick, trigger a tick
            while (fixedElapsedTime >= tickInterval)
            {
                fixedElapsedTime -= tickInterval; // Subtract the tick interval
                OnTick(); // Trigger game tick logic
            }
        }
    }

    // This function is called on every tick
    void OnTick()
    {
        if (isGameOver)
        {
            return;
        }
        currentTick++;
        timeElapsed += tickInterval; // Update the total time elapsed in the game
        gameTime -= tickInterval;
        ticksSinceLastMultiplierIncrease++;

        score += tickInterval * speedMultiplier;

        // Update the speed multiplier if necessary
        UpdateSpeedMultiplier();

        // Update the UI Text with the current speed multiplier
        UpdateUIText();

        

        if (gameTime <= 0)
        {
            isGameOver = true;
            EndGame();
        }

        Debug.Log($"Tick: {currentTick}, Multiplier: {speedMultiplier}, Next Increase At: {ticksToMultiplierIncrease}");
    }

    void UpdateSpeedMultiplier()
    {
        if (ticksSinceLastMultiplierIncrease >= ticksToMultiplierIncrease)
        {
            ticksSinceLastMultiplierIncrease = 0; // Reset the tick counter

            // Increment the speed multiplier
            speedMultiplier = Mathf.Min(speedMultiplier + multiplierIncrement, maxSpeedMultiplier);

            // Gradually increase the ticks required for the next multiplier increase
            ticksToMultiplierIncrease = Mathf.Min(ticksToMultiplierIncrease + 10, maxTicksToMultiplierIncrease);
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
        speedMultiplier = Mathf.Max(speedMultiplier - 0.5f, minSpeedMultiplier); // Reduce multiplier but keep it >= minSpeed

        yield return new WaitForSecondsRealtime(1f); // Wait for 1 second in real time

        // Restore original time scale and multiplier
        Time.timeScale = originalTimeScale;
        speedMultiplier = originalMultiplier;
    }

    void UpdateUIText()
    {
        // Set the speed multiplier text to display the current value
        if (speedText != null)
        {
            speedText.text = "Speed: " + speedMultiplier.ToString("F2"); // Display with 2 decimal points
        }

        if (timeText != null)
        {
            timeText.text = "Time Remaining: " + gameTime.ToString("F1");
        }

        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString("F0");
        }
    }

    void EndGame()
    {
        Debug.Log("Game Over");
        // Handle game-over logic here (e.g., show game-over screen)
    }

}