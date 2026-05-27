using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "TowerDefense/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int maxHP = 20;
    public float moveSpeed = 2f;
    public int cost = 10;
    public int rewardGold = 10;
    public int baseDamage = 1;
    public bool ignoresFreeze = false;
}
