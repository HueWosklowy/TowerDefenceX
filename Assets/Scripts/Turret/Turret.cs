using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] TurretData data;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] TMPro.TextMeshPro upgradeIndicator;

    public int Cost => data.Cost;
    // Dynamic property that adds bonus damage if upgraded
    public int CurrentDamage => data.Damage + damageBonus;

    private int damageBonus = 0;
    private bool isUpgraded = false;

    Transform target;
    float fireCountdown;

    void Start()
    {
        if (upgradeIndicator != null) upgradeIndicator.gameObject.SetActive(false);
    }

    void Update()
    {
        FindTarget();
        if (target == null)
            return;

        AimAt(target);

        if (fireCountdown <= 0f)
        {
            // Pass CurrentDamage instead of data.Damage
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
            // CRITICAL CHANGE: Pass CurrentDamage here so upgraded damage applies!
            bullet.Initialize(target, CurrentDamage, data.BulletSpeed); 
    }

    // --- UPGRADE & SELL SYSTEM METHODS ---

    public void UpgradeTurret(int damageIncrease)
    {
        if (isUpgraded) return;

        damageBonus += damageIncrease;
        isUpgraded = true;

        if (upgradeIndicator != null) upgradeIndicator.gameObject.SetActive(true);
    }

    public int GetSellValue()
    {
        // Return 50% of base cost, plus a bit more if it was upgraded
        int baseValue = data.Cost / 2;
        return isUpgraded ? baseValue + 20 : baseValue;
    }
}