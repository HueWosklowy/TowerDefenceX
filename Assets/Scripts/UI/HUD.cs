using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject gameWonPanel;

    void OnEnable()
    {
        var game = GameManager.Instance;
        game.MoneyChanged += UpdateMoney;
        game.LivesChanged += UpdateLives;
        game.GameEnded += OnGameEnded;

        UpdateMoney(game.Money);
        UpdateLives(game.Lives);
    }

    void OnDisable()
    {
        var game = GameManager.Instance;
        game.MoneyChanged -= UpdateMoney;
        game.LivesChanged -= UpdateLives;
        game.GameEnded -= OnGameEnded;
    }

    void UpdateMoney(int money)
    {
        moneyText.text = "MONEY: " + money;
    }

    void UpdateLives(int lives)
    {
        livesText.text = "HP: " + lives;
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