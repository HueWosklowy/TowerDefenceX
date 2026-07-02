using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    [Header("Available Turrets")]
    [SerializeField] GameObject standardTurretPrefab;
    [SerializeField] GameObject fastTurretPrefab;

    [Header("UI Reference")]
    [SerializeField] GameObject shopMenuUI; // Assign your Canvas UI panel here

    private Node targetNode;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        // Hide shop menu at start
        if (shopMenuUI != null) shopMenuUI.SetActive(false);
    }

    public void OpenShopMenu(Node node)
    {
        targetNode = node;
        
        if (shopMenuUI != null)
        {
            shopMenuUI.SetActive(true);
            
            // Optional: Move UI to the node's screen position if using a World Space canvas
            //shopMenuUI.transform.position = node.BuildPosition;
        }
    }

    // Call this method from your UI Button for the Standard Turret
    public void SelectStandardTurret() => BuildTurret(standardTurretPrefab);

    // Call this method from your UI Button for the Fast Turret
    public void SelectFastTurret() => BuildTurret(fastTurretPrefab);

    // Call this method from your UI "Back" Button to close the menu
    public void CloseShopMenu()
    {
        shopMenuUI.SetActive(false);
        targetNode = null; // Clear the node reference so nothing gets built by accident
    }

    private void BuildTurret(GameObject turretPrefab)
    {
        if (targetNode == null || turretPrefab == null) return;

        Turret turretScript = turretPrefab.GetComponent<Turret>();
        if (turretScript == null) return;

        int cost = turretScript.Cost;

        if (!GameManager.Instance.TrySpendMoney(cost))
        {
            Debug.Log("Sir, more money please!");
            shopMenuUI.SetActive(false);
            return;
        }

        GameObject spawnedTurret = Instantiate(turretPrefab, targetNode.BuildPosition, Quaternion.identity);
        targetNode.SetTurret(spawnedTurret);

        // Close the menu after successfully building
        shopMenuUI.SetActive(false);
        targetNode = null;
    }
}