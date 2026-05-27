using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData data;

    private int currentHP;
    private int waypointIndex;
    private float currentSpeed;
    private float slowTimer;

    public float ProgressScore =>
        waypointIndex +
        Vector3.Distance(
            transform.position,
            WaypointPath.Instance.GetPoint(Mathf.Min(waypointIndex + 1, WaypointPath.Instance.Count - 1))
        ) * 0.001f;

    public void Init(EnemyData enemyData)
    {
        data = enemyData;
        currentHP = data.maxHP;
        currentSpeed = data.moveSpeed;
        slowTimer = 0f;
        waypointIndex = 0;

        if (WaypointPath.Instance != null && WaypointPath.Instance.Count > 0)
        {
            transform.position = WaypointPath.Instance.GetPoint(0);
        }

        gameObject.tag = "Enemy";
    }

    private void Update()
    {
        if (data == null) return;
        if (WaypointPath.Instance == null || WaypointPath.Instance.Count == 0) return;

        if (slowTimer > 0f)
        {
            slowTimer -= Time.deltaTime;
            if (slowTimer <= 0f)
            {
                currentSpeed = data.moveSpeed;
            }
        }

        if (waypointIndex >= WaypointPath.Instance.Count)
        {
            ReachBase();
            return;
        }

        Vector3 target = WaypointPath.Instance.GetPoint(waypointIndex);
        transform.position = Vector3.MoveTowards(transform.position, target, currentSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.05f)
        {
            waypointIndex++;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void ApplySlow(float multiplier, float duration)
    {
        if (data == null) return;
        if (data.ignoresFreeze) return;

        currentSpeed = data.moveSpeed * multiplier;
        slowTimer = duration;
    }

    private void ReachBase()
    {
        if (GameManager.Instance != null && GameManager.Instance.baseHealth != null)
        {
            GameManager.Instance.baseHealth.DamageBase(data.baseDamage);
        }

        if (EnemySpawner.Instance != null)
        {
            EnemySpawner.Instance.DespawnEnemy(this);
        }
    }

    private void Die()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.NotifyEnemyKilled(data.rewardGold);
        }

        if (EnemySpawner.Instance != null)
        {
            EnemySpawner.Instance.DespawnEnemy(this);
        }
    }
}