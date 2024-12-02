using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeManager : MonoBehaviour
{
    public float tickInterval = 0.05f; // Initial time interval for each tick (in seconds)
    private float timeSinceLastTick = 0f; // Tracks time since last tick
    private int currentTick = 0; // The current tick number

    public float speedMultiplier = 1.0f; // The speed multiplier (starts at 1.0)
    private float timeElapsed = 0f; // Tracks the total time passed in the game
    public float multiplierIncreaseTime = 60f; // Time intervals after which the speed multiplier should increase (e.g., 60s)
    public float maxSpeedMultiplier = 7.0f; // The maximum speed multiplier
    public float multiplierIncrement = 0.2f; // The amount by which the multiplier increases at each step
    
    public Text speedText;


    void Start()
    {
        // Start tick updates when the game starts via the coroutine
        StartCoroutine(TickUpdate());
    }

    // Coroutine to handle tick updates
    IEnumerator TickUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(tickInterval / speedMultiplier); // Adjust tick interval based on the speed multiplier
            OnTick(); // Trigger game tick logic
        }
    }

    // This function is called on every tick
    void OnTick()
    {
        currentTick++;
        timeElapsed += tickInterval; // Update the total time elapsed in the game

        // Update the speed multiplier if necessary
        UpdateSpeedMultiplier();

        // Update the UI Text with the current speed multiplier
        UpdateSpeedUIText();

        // Log or use the current tick and speed multiplier as needed
        // Debug.Log($"Tick: {currentTick}, Speed Multiplier: {speedMultiplier}");
    }

    // Updates the speed multiplier based on the total elapsed time
    void UpdateSpeedMultiplier()
    {
        if (timeElapsed >= multiplierIncreaseTime)
        {
            // Increase the speed multiplier in larger steps
            speedMultiplier += multiplierIncrement;

            // Ensure the speed multiplier does not exceed the max speed
            if (speedMultiplier > maxSpeedMultiplier)
            {
                speedMultiplier = maxSpeedMultiplier;
            }

            // Reset the timer for the next increment (every `multiplierIncreaseTime` seconds)
            timeElapsed = 0f;
        }
    }

    // Update the Text UI element with the current speed multiplier
    void UpdateSpeedUIText()
    {
        // Set the speed multiplier text to display the current value
        if (speedText != null)
        {
            speedText.text = "Speed: " + speedMultiplier.ToString("F2"); // Display with 2 decimal points
        }
    }
}