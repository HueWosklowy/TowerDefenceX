using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI waveText;

    [Header("Panels")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject gameWonPanel;

    [Header("References")]
    [SerializeField] WaveSpawner waveSpawner;

    void OnEnable()
    {
        var game = GameManager.Instance;

        game.MoneyChanged += UpdateMoney;
        game.LivesChanged += UpdateLives;
        game.GameEnded += OnGameEnded;

        UpdateMoney(game.Money);
        UpdateLives(game.Lives);

        if (waveSpawner != null)
        {
            waveSpawner.WaveChanged += UpdateWave;
            UpdateWave(waveSpawner.CurrentWaveNumber, waveSpawner.TotalWaves);
        }
    }

    void OnDisable()
    {
        var game = GameManager.Instance;

        game.MoneyChanged -= UpdateMoney;
        game.LivesChanged -= UpdateLives;
        game.GameEnded -= OnGameEnded;

        if (waveSpawner != null)
        {
            waveSpawner.WaveChanged -= UpdateWave;
        }
    }

    void UpdateMoney(int money)
    {
        moneyText.text = "MONEY: " + money;
    }

    void UpdateLives(int lives)
    {
        livesText.text = "HP: " + lives;
    }

    void UpdateWave(int currentWave, int totalWaves)
    {
        if (waveText == null)
            return;

        waveText.text = "WAVE: " + currentWave + " / " + totalWaves;
    }

    void OnGameEnded(bool isGameWon)
    {
        if (isGameWon)
        {
            ShowGameWon();
        }
        else
        {
            ShowGameOver();
        }
    }

    void ShowGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    void ShowGameWon()
    {
        if (gameWonPanel != null)
            gameWonPanel.SetActive(true);
    }
}