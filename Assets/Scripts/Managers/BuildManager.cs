using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    [Header("Available Turrets")]
    [SerializeField] GameObject standardTurretPrefab;
    [SerializeField] GameObject fastTurretPrefab;

    [Header("UI References")]
    [SerializeField] GameObject shopMenuUI; 
    [SerializeField] GameObject upgradeMenuUI; // Assign your new Upgrade Panel UI here

    [Header("Upgrade Balancing")]
    [SerializeField] int upgradeCost = 30;
    [SerializeField] int damageIncrease = 100;

    private Node targetNode;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        
        if (shopMenuUI != null) shopMenuUI.SetActive(false);
        if (upgradeMenuUI != null) upgradeMenuUI.SetActive(false);
    }

    public void OpenShopMenu(Node node)
    {
        if (upgradeMenuUI.activeSelf) upgradeMenuUI.SetActive(false); // Close other menu
        targetNode = node;
        if (shopMenuUI != null) shopMenuUI.SetActive(true);
    }

    public void OpenUpgradeMenu(Node node)
    {
        if (shopMenuUI.activeSelf) shopMenuUI.SetActive(false); // Close other menu
        targetNode = node;
        if (upgradeMenuUI != null) upgradeMenuUI.SetActive(true);
    }

    public void SelectStandardTurret() => BuildTurret(standardTurretPrefab);
    public void SelectFastTurret() => BuildTurret(fastTurretPrefab);

    private void BuildTurret(GameObject turretPrefab)
    {
        if (targetNode == null || turretPrefab == null) return;
        Turret turretScript = turretPrefab.GetComponent<Turret>();
        if (turretScript == null) return;

        if (!GameManager.Instance.TrySpendMoney(turretScript.Cost))
        {
            Debug.Log("Sir, more money please!");
            shopMenuUI.SetActive(false);
            return;
        }

        GameObject spawnedTurret = Instantiate(turretPrefab, targetNode.BuildPosition, Quaternion.identity);
        targetNode.SetTurret(spawnedTurret);

        shopMenuUI.SetActive(false);
        targetNode = null;
    }

    // --- NEW UPGRADE & SELL METHODS ---

    public void UpgradeSelectedTurret()
    {
        if (targetNode == null || targetNode.CurrentTurret == null) return;

        if (!GameManager.Instance.TrySpendMoney(upgradeCost))
        {
            Debug.Log("Not enough money to upgrade!");
            upgradeMenuUI.SetActive(false);
            return;
        }

        targetNode.CurrentTurret.UpgradeTurret(damageIncrease);
        CloseUpgradeMenu();
    }

    public void SellSelectedTurret()
    {
        if (targetNode == null || targetNode.CurrentTurret == null) return;

        int goldEarned = targetNode.CurrentTurret.GetSellValue();
        
        // Assuming your GameManager has an AddMoney or similar function:
        // GameManager.Instance.AddMoney(goldEarned); 
        Debug.Log($"Sold turret for ${goldEarned}!");

        targetNode.ClearNode();
        CloseUpgradeMenu();
    }

    public void CloseShopMenu()
    {
        shopMenuUI.SetActive(false);
        targetNode = null;
    }

    public void CloseUpgradeMenu()
    {
        upgradeMenuUI.SetActive(false);
        targetNode = null;
    }
}