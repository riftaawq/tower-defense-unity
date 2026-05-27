using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "TowerDefense/Tower Data")]
public class TowerData : ScriptableObject
{
    public string towerName;
    public int cost = 100;
    public float range = 3f;
    public float fireRate = 1f;
    public int damage = 10;
    public bool isAoE;
    public float aoeRadius = 1.5f;
    public bool appliesSlow;
    public float slowMultiplier = 0.5f;
    public float slowDuration = 1.5f;
    public GameObject projectilePrefab;
}