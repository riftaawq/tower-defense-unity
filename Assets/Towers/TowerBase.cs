using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    protected TowerData data;
    protected float fireTimer;

    public TowerData Data => data;

    public void Configure(TowerData towerData)
    {
        data = towerData;
        fireTimer = 0f;
    }

    protected virtual void Update()
    {
        if (data == null || GameManager.Instance.CurrentState != GameState.Battle) return;

        fireTimer -= Time.deltaTime;
        if (fireTimer > 0f) return;

        Enemy target = FindBestTarget();
        if (target == null) return;

        Shoot(target);
        fireTimer = 1f / Mathf.Max(0.01f, data.fireRate);
    }

    protected Enemy FindBestTarget()
    {
        List<Enemy> enemies = EnemySpawner.Instance.GetAliveEnemies();
        Enemy best = null;
        float bestProgress = float.MinValue;

        foreach (var enemy in enemies)
        {
            if (enemy == null) continue;
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance > data.range) continue;

            if (enemy.ProgressScore > bestProgress)
            {
                bestProgress = enemy.ProgressScore;
                best = enemy;
            }
        }

        return best;
    }

    protected virtual void Shoot(Enemy target)
    {
        GameObject projectileObj = Instantiate(data.projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        projectile.Launch(target, data, transform.position);
    }
}