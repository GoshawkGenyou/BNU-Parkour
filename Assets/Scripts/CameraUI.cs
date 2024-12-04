
using UnityEngine;
using UnityEngine.UI;

public class CameraUI : MonoBehaviour
{
    public RectTransform uiElement; // Reference to the UI element (e.g., a panel or HUD)
    public Text speedText; // Speed multiplier text
    public Text timeText; // Time remaining text
    public Text scoreText; // Score text

    public GameTimeManager gameTimeManager; // Reference to the GameTimeManager to get game data

    void Start()
    {
        // Set the UI element position
        uiElement.anchorMin = new Vector2(1, 1);
        uiElement.anchorMax = new Vector2(1, 1);
        uiElement.pivot = new Vector2(1, 1);
        uiElement.anchoredPosition = new Vector2(-20, -20);
    }

    public void UpdateUI()
    {
        // Update the UI elements with data from the GameTimeManager
        if (speedText != null)
        {
            speedText.text = "Speed: " + gameTimeManager.speedMultiplier.ToString("F2");
        }

        if (timeText != null)
        {
            timeText.text = "Time Remaining: " + gameTimeManager.gameTime.ToString("F1");
        }

        if (scoreText != null)
        {
            scoreText.text = "Score: " + gameTimeManager.score.ToString("F0");
        }
    }
}