using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject turretPrefab;
    [SerializeField] Vector3 positionOffset;

    GameObject turret;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (turret != null)
        {
            Debug.Log("Cannot add another turret here!");
            return;
        }
        BuildTurret();
    }

    void BuildTurret()
    {
        int cost = turretPrefab.GetComponent<Turret>().Cost;

        if (!GameManager.Instance.TrySpendMoney(cost))
        {
            Debug.Log("Sir, more money please!");
            return;
        }

        turret = Instantiate(turretPrefab, transform.position + positionOffset, Quaternion.identity);
    }
}