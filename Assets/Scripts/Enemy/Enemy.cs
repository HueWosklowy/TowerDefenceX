using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData data;

    private Vector3 target;
    private int waypointIndex;
    private int currentHealth;

    private void Start()
    {
        currentHealth = data.MaxHealth;
        target = TilemapPath.Points[0];
        transform.position = target;
    }

    private void OnEnable() => GameManager.Instance.EnemySpawned();
    private void OnDestroy() => GameManager.Instance.EnemyKilled();
    private void Update() => Move();

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(
            transform.position, target, data.MoveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.01f)
            GetNextWaypoint();
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