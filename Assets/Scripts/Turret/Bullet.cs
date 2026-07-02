using UnityEngine;

public class Bullet : MonoBehaviour
{
    Transform target;
    int damage;
    float speed;

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        MoveTowardsTarget();
    }

    public void Initialize(Transform newTarget, int newDamage, float newSpeed)
    {
        target = newTarget;
        damage = newDamage;
        speed = newSpeed;
    }

    void MoveTowardsTarget()
    {
        Vector2 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
            enemy.TakeDamage(damage);

        Destroy(gameObject);
    }
}