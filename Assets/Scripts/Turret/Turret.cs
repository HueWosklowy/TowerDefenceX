using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] TurretData data;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;

    public int Cost => data.Cost;

    Transform target;
    float fireCountdown;

    void Update()
    {
        FindTarget();
        if (target == null)
            return;

        AimAt(target);

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / data.FireRate;
        }
        fireCountdown -= Time.deltaTime;
    }

    void OnDrawGizmosSelected()
    {
        if (data == null)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.Range);
    }

    void FindTarget()
    {
        Targetable[] targets = Object.FindObjectsByType<Targetable>(FindObjectsSortMode.None);

        float shortestDistance = Mathf.Infinity;
        Targetable nearest = null;

        foreach (Targetable t in targets)
        {
            float distance = Vector2.Distance(transform.position, t.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = t;
            }
        }

        target = (nearest != null && shortestDistance <= data.Range) ? nearest.transform : null;
    }

    private void AimAt(Transform t)
    {
        Vector2 dir = t.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    private void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if (bullet != null)
            bullet.Initialize(target, data.Damage, data.BulletSpeed);
    }
}