using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Vector3 positionOffset;
    private GameObject turret;

    public Vector3 BuildPosition => transform.position + positionOffset;

    public void OnPointerClick(PointerEventData eventData)
    {
        // If UI is clicked over the node, EventSystem usually blocks it, 
        // but we double-check if the node already has a turret.
        if (turret != null)
        {
            Debug.Log("Cannot add another turret here!");
            return;
        }

        // Open the shop selection menu at this node
        BuildManager.Instance.OpenShopMenu(this);
    }

    public void SetTurret(GameObject turretInstance)
    {
        turret = turretInstance;
    }
}