using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Tower Defense/Enemy Data")]
public class EnemyData : ScriptableObject
{
    // --- Pola i właściwości ---
    // [field: SerializeField] = edytowalne w Inspektorze, czytane publicznie, ustawiane tylko tu
    [field: SerializeField] public float MoveSpeed { get; private set; } = 2f;
    [field: SerializeField] public int MaxHealth { get; private set; } = 100;
    [field: SerializeField] public int Reward { get; private set; } = 20;
}