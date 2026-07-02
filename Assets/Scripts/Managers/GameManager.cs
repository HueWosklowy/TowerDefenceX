using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public event Action<int> MoneyChanged = delegate { };
    public event Action<int> LivesChanged = delegate { };
    public event Action<bool> GameEnded = delegate { };

    public static GameManager Instance { get; private set; }
    public int Money { get; private set; }
    public int Lives { get; private set; }
    public int EnemiesAlive { get; private set; }

    [SerializeField] int startingMoney = 100;
    [SerializeField] int startingLives = 10;

    bool isGameOver;
    bool isGameWon;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        isGameOver = false;
        isGameWon = false;
        Time.timeScale = 1f;
        Money = startingMoney;
        Lives = startingLives;
        MoneyChanged?.Invoke(Money);
        LivesChanged?.Invoke(Lives);
    }

    public void AddMoney(int amount)
    {
        Money += amount;
        MoneyChanged?.Invoke(Money);
    }

    public bool TrySpendMoney(int amount)
    {
        if (Money < amount)
            return false;

        Money -= amount;
        MoneyChanged?.Invoke(Money);
        return true;
    }

    public void LoseLife()
    {
        if (isGameOver)
            return;

        Lives--;
        LivesChanged?.Invoke(Lives);

        if (Lives <= 0)
        {
            Lives = 0;
            LivesChanged?.Invoke(Lives);
            EndGame();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void WinGame()
    {
        if (isGameOver || isGameWon) return;
        isGameWon = true;
        GameEnded?.Invoke(true);
        Time.timeScale = 0f;
    }

    private void EndGame()
    {
        isGameOver = true;
        GameEnded?.Invoke(false);
        Time.timeScale = 0f;
    }

    public void EnemySpawned() => EnemiesAlive++;

    public void EnemyKilled()
    {
        EnemiesAlive = Mathf.Max(0, EnemiesAlive - 1);
    }
}