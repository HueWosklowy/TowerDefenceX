using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Vector3 positionOffset;
    private GameObject turretInstance;
    private Turret turretScript;

    public Vector3 BuildPosition => transform.position + positionOffset;
    public Turret CurrentTurret => turretScript;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (turretInstance != null)
        {
            // Open UPGRADE/SELL menu because a turret exists here
            BuildManager.Instance.OpenUpgradeMenu(this);
            return;
        }

        // Open SHOP menu because the tile is empty
        BuildManager.Instance.OpenShopMenu(this);
    }

    public void SetTurret(GameObject spawned)
    {
        turretInstance = spawned;
        turretScript = spawned != null ? spawned.GetComponent<Turret>() : null;
    }

    public void ClearNode()
    {
        if (turretInstance != null) Destroy(turretInstance);
        SetTurret(null);
    }
}