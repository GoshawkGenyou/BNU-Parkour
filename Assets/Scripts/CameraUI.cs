using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraUI : MonoBehaviour
{
    public GameObject canvas;
    public Text speedText; // Speed multiplier text
    public Text timeText; // Time remaining text
    public Text scoreText; // Score text

    public GameObject gameOverPanel; // Game Over screen panel (to show/hide)
    public Text finalScoreText; // Final score text on Game Over screen
    public Text finalSpeedText; // Final speed multiplier text on Game Over screen
    public Button restartButton; // Restart button reference

    public GameTimeManager gameTimeManager; // Reference to the GameTimeManager to get game data

    void Start()
    {
        // Initially, hide the Game Over panel
        gameOverPanel.SetActive(false);
        // Attach button listeners
        restartButton.onClick.AddListener(RestartGame);
    }

    void Update()
    {
        // Show UI updates while game is still active
        if (!gameTimeManager.isGameOver)
        {
            UpdateUI();
        }
        else
        {
            // Display the Game Over screen when the game is over
            ShowGameOverScreen();
        }
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

    // Restart the game (reset relevant values and reload the scene)
    private void RestartGame()
    {
        gameTimeManager.isGameOver = false;
        gameTimeManager.gameTime = 25f;
        gameTimeManager.score = 0f;
        gameTimeManager.speedMultiplier = 1f;

        // Reset or reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Show the Game Over screen and display final stats
    private void ShowGameOverScreen()
    {
        if (gameOverPanel.activeSelf && gameTimeManager.isGameOver)
        {
            return;
        }
        canvas.SetActive(false);
        // Activate the Game Over screen
        gameOverPanel.SetActive(true);

        finalScoreText.text = "Final Score: " + gameTimeManager.score.ToString("F0");
        finalSpeedText.text = "Final Speed: " + gameTimeManager.speedMultiplier.ToString("F2");
    }

}
