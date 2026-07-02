using UnityEngine;

[CreateAssetMenu(fileName = "TurretData", menuName = "Tower Defense/Turret Data")]
public class TurretData : ScriptableObject
{
    [field: SerializeField] public float Range { get; private set; } = 3f;
    [field: SerializeField] public float FireRate { get; private set; } = 1f;   // strzały/sek
    [field: SerializeField] public int Damage { get; private set; } = 50;
    [field: SerializeField] public float BulletSpeed { get; private set; } = 10f;
    [field: SerializeField] public int Cost { get; private set; } = 50;
}