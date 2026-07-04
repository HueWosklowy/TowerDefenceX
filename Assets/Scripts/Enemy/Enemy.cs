using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData data;

    [Header("Health Bar")]
    [SerializeField] private GameObject healthBarRoot;
    [SerializeField] private Transform healthBarFill;

    private Vector3 target;
    private int waypointIndex;
    private int currentHealth;

    private Vector3 originalFillScale;
    private Vector3 originalFillPosition;

    private void Start()
    {
        currentHealth = data.MaxHealth;

        target = TilemapPath.Points[0];
        transform.position = target;

        if (healthBarRoot != null)
        {
            healthBarRoot.SetActive(false);
        }

        if (healthBarFill != null)
        {
            originalFillScale = healthBarFill.localScale;
            originalFillPosition = healthBarFill.localPosition;
        }
    }

    private void OnEnable() => GameManager.Instance.EnemySpawned();

    private void OnDestroy() => GameManager.Instance.EnemyKilled();

    private void Update() => Move();

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarRoot != null)
        {
            healthBarRoot.SetActive(true);
        }

        if (healthBarFill == null)
            return;

        float healthPercent = (float)currentHealth / data.MaxHealth;

        healthBarFill.localScale = new Vector3(
            originalFillScale.x * healthPercent,
            originalFillScale.y,
            originalFillScale.z
        );

        float lostWidth = originalFillScale.x * (1f - healthPercent);

        healthBarFill.localPosition = new Vector3(
            originalFillPosition.x - lostWidth / 2f,
            originalFillPosition.y,
            originalFillPosition.z
        );
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            data.MoveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            GetNextWaypoint();
        }
    }

    private void GetNextWaypoint()
    {
        if (waypointIndex >= TilemapPath.Points.Length - 1)
        {
            ReachEnd();
            return;
        }

        waypointIndex++;
        target = TilemapPath.Points[waypointIndex];
    }

    private void ReachEnd()
    {
        GameManager.Instance.LoseLife();
        Destroy(gameObject);
    }

    private void Die()
    {
        GameManager.Instance.AddMoney(data.Reward);
        Destroy(gameObject);
    }
}